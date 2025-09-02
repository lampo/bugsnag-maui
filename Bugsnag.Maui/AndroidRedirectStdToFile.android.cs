using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Java.IO;
using static Android.Systems.Os;

namespace Bugsnag.Maui;

internal partial class AndroidRedirectStdToLogCat
{
    // PURPOSE
    // Redirect stdout/stderr to a pipe so we can capture Mono's native crash output,
    // detect the "Managed Stacktrace:" block, and persist it to disk for Bugsnag.
    // example crash report:
    // ================================================================
    // Native Crash Reporting
    // ================================================================
    // Got a SIGSEGV while executing native code. This usually indicates
    // a fatal error in the mono runtime or one of the native libraries
    // used by your application.
    // ================================================================
    //
    // No native Android stacktrace (see debuggerd output).
    //
    // ================================================================
    // Basic Fault Address Reporting
    // ================================================================
    // Memory around native instruction pointer (0x7bd510b73c):
    // 0x7bd510b72c  00 00 80 3d a1 00 9f 3c c0 03 5f d6 c2 00 18 36  ...=...<......6
    // 0x7bd510b73c  26 00 40 f9 87 80 5f f8 06 00 00 f9 a7 80 1f f8  &.@............
    // 0x7bd510b74c  c0 03 5f d6 c2 00 10 36 26 00 40 b9 88 c0 5f b8  ......6&.@....
    // 0x7bd510b75c  06 00 00 b9 a8 c0 1f b8 c0 03 5f d6 02 01 00 b4  ...............
    // ================================================================
    //
    // ================================================================
    // Managed Stacktrace:
    // ================================================================
    // at <unknown> <0xffffffff>
    // at EveryDollar.DebugMenu.DebugMenuPage:memcpy <0x00007>
    // at EveryDollar.DebugMenu.DebugMenuPage:NativeCrashApp <0x00037>
    // at CommunityToolkit.Mvvm.Input.RelayCommand:Execute <0x0003f>
    // at Microsoft.Maui.Controls.ButtonElement:ElementClicked <0x000bb>
    // at Microsoft.Maui.Controls.Button:SendClicked <0x00033>
    // at Microsoft.Maui.Controls.Button:Microsoft.Maui.IButton.Clicked <0x0002b>
    // at Microsoft.Maui.Handlers.ButtonHandler:OnClick <0x00053>
    // at ButtonClickListener:OnClick <0x0005b>
    // at IOnClickListenerInvoker:n_OnClick_Landroid_view_View <0x00097>
    // at Android.Runtime.JNINativeWrapper:Wrap_JniMarshal_PPL_V <0x00053>
    // at Android.Runtime.JNINativeWrapper:Wrap_JniMarshal_PPL_V <0x00067>
    // ================================================================
    //
    // Fatal signal 11 (SIGSEGV), code 1 (SEGV_MAPERR), fault addr 0x0
    //
    private static Thread? thread;

    private static bool IsStarted => thread is not null;

    /// <summary>
    /// Start redirection of stdout (fd 1) and stderr (fd 2) into a pipe and
    /// spawn a background thread that parses Mono crash output.
    /// </summary>
    public static void Start(string tag = "Bugsnag.Maui")
    {
        try
        {
            if (IsStarted)
                return;

            var pipe =
                Pipe() ?? throw new Exception("Could not create pipe for stdout redirection.");
            var pipeReader = pipe[0];
            var pipeWriter = pipe[1];

            // Critical redirection: everything written to stdout/stderr flows into ThreadLoop.
            Dup2(pipeWriter, 1);
            Dup2(pipeWriter, 2);

            thread = new Thread(ThreadLoop) { IsBackground = true, Name = tag };
            thread.Start(new ThreadParameters(tag, pipeReader));

            // Keep storage bounded.
            CleanupCrashDirectory();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Bugsnag redirect start] {ex}");
        }
    }

    // Parameters passed to the background reader.
    private sealed record ThreadParameters(string Tag, FileDescriptor PipeReaderFd);

    /// <summary>
    /// Reads lines from the pipe, mirrors them to Logcat, and captures the
    /// "Managed Stacktrace:" block until it ends, then writes it to a file.
    /// </summary>
    private static void ThreadLoop(object? state)
    {
        if (state is not ThreadParameters parameters)
            return;

        try
        {
            var buffer = new byte[1024]; // Raw pipe buffer
            var bufferSpan = buffer.AsSpan();
            var used = 0;

            var stackTraceBuilder = new StringBuilder();
            var isCapturingStackTrace = false;
            var skipNextDelimiter = false; // skip the ===== line right after header
            var stackTraceLineCount = 0;

            using var stream = new FileInputStream(parameters.PipeReaderFd);

            while (true)
            {
                var read = stream.Read(buffer, used, buffer.Length - used);
                if (read <= 0)
                {
                    if (isCapturingStackTrace && stackTraceLineCount > 0)
                        SaveCrashReportToFile(stackTraceBuilder.ToString(), parameters.Tag);
                    break;
                }

                var length = used + read;
                var data = bufferSpan[..length];

                int index;
                while ((index = data.IndexOf((byte)'\n')) >= 0)
                {
                    var lineBytes = data[..index];
                    var line = Encoding.UTF8.GetString(lineBytes);

                    // Surface everything to Logcat for parity with stdout.
                    Debug.WriteLine($"[{parameters.Tag}] {line}");

                    // Detect start of managed stack section.
                    if (line.Contains("Managed Stacktrace:"))
                    {
                        isCapturingStackTrace = true;
                        skipNextDelimiter = true;
                        stackTraceLineCount = 0;
                        stackTraceBuilder.Clear();
                    }
                    else if (isCapturingStackTrace)
                    {
                        if (skipNextDelimiter && line.StartsWith('='))
                        {
                            skipNextDelimiter = false;
                        }
                        else if (IsStackTraceEnd(line, stackTraceLineCount))
                        {
                            if (stackTraceLineCount > 0)
                                SaveCrashReportToFile(stackTraceBuilder.ToString(), parameters.Tag);
                            isCapturingStackTrace = false;
                            stackTraceLineCount = 0;
                        }
                        else if (!skipNextDelimiter)
                        {
                            stackTraceBuilder.AppendLine(line);
                            if (!string.IsNullOrWhiteSpace(line))
                                stackTraceLineCount++;
                        }
                    }

                    data = data[(index + 1)..];
                }

                if (data.Length > 0)
                {
                    if (data.Length >= buffer.Length)
                    {
                        Debug.WriteLine($"[{parameters.Tag}] Line too long, truncating");
                        used = 0;
                    }
                    else
                    {
                        data.CopyTo(bufferSpan);
                        used = data.Length;
                    }
                }
                else
                {
                    used = 0;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[{parameters.Tag}] Std redirect error: {ex}");
        }
    }

    /// <summary>
    /// End-of-stack detection: '=', or blank after at least one frame, or a non-frame line once started.
    /// </summary>
    private static bool IsStackTraceEnd(string line, int stackTraceLineCount) =>
        line.StartsWith('=')
        || (string.IsNullOrWhiteSpace(line) && stackTraceLineCount > 0)
        || (!line.Contains("at ") && !string.IsNullOrWhiteSpace(line) && stackTraceLineCount > 0);

    /// <summary>
    /// Persist captured managed stack trace to a timestamped file in <files>/crashes.
    /// </summary>
    private static void SaveCrashReportToFile(string stackTrace, string tag)
    {
        try
        {
            var context =
                Platform.CurrentActivity?.ApplicationContext
                ?? global::Android.App.Application.Context;
            var crashDir = new Java.IO.File(context.FilesDir, "crashes");
            if (!crashDir.Exists())
                crashDir.Mkdirs();

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var crashFile = new Java.IO.File(crashDir, $"crash_{timestamp}.txt");

            using var writer = new FileWriter(crashFile);
            writer.Write(stackTrace.Trim());
            writer.Flush();

            Debug.WriteLine($"[{tag}] Crash report saved to: {crashFile.AbsolutePath}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[{tag}] Failed to save crash report to file: {ex.Message}");
        }
    }

    /// <summary>
    /// Retention policy: keep ≤10 files, ≤512KB total, drop files >14 days old,
    /// and ignore files younger than 30s during cleanup to avoid races.
    /// </summary>
    private static void CleanupCrashDirectory()
    {
        try
        {
            var context =
                Platform.CurrentActivity?.ApplicationContext
                ?? global::Android.App.Application.Context;
            var crashDirPath = Path.Combine(context.FilesDir?.AbsolutePath ?? "", "crashes");
            if (!Directory.Exists(crashDirPath))
                return;

            const int maxFiles = 10;
            const long maxTotalBytes = 512 * 1024;
            var retention = TimeSpan.FromDays(14);
            const long safetyWindowMs = 30_000;

            var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var files = Directory.GetFiles(crashDirPath, "crash_*.txt");

            var items = files
                .Select(path =>
                {
                    var fi = new FileInfo(path);
                    var ts = ExtractTimestamp(fi.Name);
                    var effectiveTs =
                        ts > 0
                            ? ts
                            : new DateTimeOffset(fi.LastWriteTimeUtc).ToUnixTimeMilliseconds();
                    return new
                    {
                        Path = path,
                        Info = fi,
                        TimestampMs = effectiveTs,
                        AgeMs = nowMs - effectiveTs,
                        Length = fi.Exists ? fi.Length : 0L,
                    };
                })
                .Where(x => x.Info.Exists)
                .OrderBy(x => x.TimestampMs)
                .ToList();

            if (items.Count == 0)
                return;

            bool IsRecent(long ageMs) => ageMs < safetyWindowMs;

            // Pass 1: remove files older than retention (skip recent files)
            foreach (
                var item in items
                    .Where(x => !IsRecent(x.AgeMs) && x.AgeMs > retention.TotalMilliseconds)
                    .ToList()
            )
            {
                TryDelete(item.Path);
                items.Remove(item);
            }

            // Pass 2: enforce max file count
            if (items.Count > maxFiles)
            {
                var overBy = items.Count - maxFiles;
                // Prefer deleting non-recent oldest first
                foreach (
                    var item in items
                        .Where(x => !IsRecent(x.AgeMs))
                        .OrderBy(x => x.TimestampMs)
                        .ToList()
                )
                {
                    if (overBy <= 0)
                        break;
                    if (TryDelete(item.Path))
                    {
                        items.Remove(item);
                        overBy--;
                    }
                }

                // Still over? delete oldest regardless
                foreach (var item in items.OrderBy(x => x.TimestampMs).ToList())
                {
                    if (items.Count <= maxFiles)
                        break;
                    if (TryDelete(item.Path))
                        items.Remove(item);
                }
            }

            // Pass 3: enforce max total bytes
            var totalBytes = items.Sum(x => x.Length);
            if (totalBytes > maxTotalBytes)
            {
                foreach (
                    var item in items
                        .Where(x => !IsRecent(x.AgeMs))
                        .OrderBy(x => x.TimestampMs)
                        .ToList()
                )
                {
                    if (totalBytes <= maxTotalBytes)
                        break;
                    if (TryDelete(item.Path))
                    {
                        totalBytes -= item.Length;
                        items.Remove(item);
                    }
                }

                foreach (var item in items.OrderBy(x => x.TimestampMs).ToList())
                {
                    if (totalBytes <= maxTotalBytes)
                        break;
                    if (TryDelete(item.Path))
                    {
                        totalBytes -= item.Length;
                        items.Remove(item);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Bugsnag crash cleanup] {ex}");
            global::Android.Util.Log.Error("BugsnagCleanup", $"Crash cleanup failed: {ex.Message}");
        }

        static bool TryDelete(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                Debug.WriteLine($"[Bugsnag crash cleanup] Deleted: {Path.GetFileName(path)}");
                return true;
            }
            catch (Exception delEx)
            {
                Debug.WriteLine(
                    $"[Bugsnag crash cleanup] Failed to delete {path}: {delEx.Message}"
                );
                return false;
            }
        }
    }

    /// <summary>
    /// Extract epoch-ms timestamp from crash_<ts>.txt name; 0 if absent.
    /// </summary>
    private static long ExtractTimestamp(string filePath)
    {
        var match = TimeStampRegex.Match(filePath);
        if (match.Success && long.TryParse(match.Groups[1].Value, out var timestamp))
        {
            return timestamp;
        }

        return 0;
    }

    [GeneratedRegex(@"crash_(\d+)\.txt", RegexOptions.Compiled)]
    private static partial Regex TimeStampRegex { get; }
}
