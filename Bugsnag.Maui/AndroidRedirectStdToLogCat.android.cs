using System.Diagnostics;
using Java.IO;
using System.Text;
using System.Text.RegularExpressions;
using static Android.Systems.Os;

namespace Bugsnag.Maui;

internal partial class AndroidRedirectStdToLogCat
{
    private static Thread? thread;

    private static bool IsStarted => thread is not null;

    public static void Start(string tag = "Bugsnag.Maui")
    {
        try
        {
            if (IsStarted)
            {
                return;
            }

            var pipe = Pipe() ?? throw new Exception("Could not create pipe for stdout redirection.");
            var pipeReader = pipe[0];
            var pipeWriter = pipe[1];

            // Redirect stdout (fd 1) and stderr (fd 2) to our pipe
            Dup2(pipeWriter, 1);
            Dup2(pipeWriter, 2);

            thread = new Thread(ThreadLoop)
            {
                IsBackground = true,
                Name = tag,
            };
            thread.Start(new ThreadParameters(tag, pipeReader));
            CleanupCrashDirectory();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Bugsnag redirect start] {ex}");
        }
    }

    private sealed record ThreadParameters(string Tag, FileDescriptor PipeReaderFd);

    private static void ThreadLoop(object? state)
    {
        if (state is not ThreadParameters parameters)
        {
            return;
        }

        try
        {
            var buffer = new byte[1024]; // Increased buffer size
            var bufferSpan = buffer.AsSpan();
            var used = 0;
            var stackTraceBuilder = new StringBuilder();
            var isCapturingStackTrace = false;
            var skipNextDelimiter = false;
            var stackTraceLineCount = 0;

            using var stream = new FileInputStream(parameters.PipeReaderFd);

            while (true)
            {
                var read = stream.Read(buffer, used, buffer.Length - used);
                if (read <= 0)
                {
                    // Handle any remaining stack trace before exiting
                    if (isCapturingStackTrace && stackTraceLineCount > 0)
                    {
                        SaveCrashReportToFile(stackTraceBuilder.ToString(), parameters.Tag);
                    }

                    break;
                }

                var length = used + read;
                var data = bufferSpan[..length];

                int index;
                while ((index = data.IndexOf((byte)'\n')) >= 0)
                {
                    var lineBytes = data[..index];
                    var line = Encoding.UTF8.GetString(lineBytes);

                    Debug.WriteLine($"[{parameters.Tag}] {line}");

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
                            {
                                SaveCrashReportToFile(stackTraceBuilder.ToString(), parameters.Tag);
                            }

                            isCapturingStackTrace = false;
                            stackTraceLineCount = 0;
                        }
                        else if (!skipNextDelimiter)
                        {
                            stackTraceBuilder.AppendLine(line);
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                stackTraceLineCount++;
                            }
                        }
                    }

                    data = data[(index + 1)..];
                }

                // Handle partial line leftover
                if (data.Length > 0)
                {
                    // Prevent buffer overflow
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

    private static bool IsStackTraceEnd(string line, int stackTraceLineCount)
    {
        // End conditions for stack trace:
        // 1. Line starting with = (delimiter)
        // 2. Empty line after we've captured some stack trace lines
        // 3. Line that doesn't look like a stack trace entry after we've started
        return line.StartsWith('=')
               || (string.IsNullOrWhiteSpace(line) && stackTraceLineCount > 0)
               || (!line.Contains("at ") && !string.IsNullOrWhiteSpace(line) && stackTraceLineCount > 0);
    }

    private static void SaveCrashReportToFile(string stackTrace, string tag)
    {
        try
        {
            var context = Platform.CurrentActivity?.ApplicationContext ?? global::Android.App.Application.Context;
            var crashDir = new Java.IO.File(context.FilesDir, "crashes");

            if (!crashDir.Exists())
            {
                crashDir.Mkdirs();
            }

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var crashFile = new Java.IO.File(crashDir, $"crash_{timestamp}.txt");

            var crashReport = stackTrace.Trim();

            using var writer = new FileWriter(crashFile);
            writer.Write(crashReport);
            writer.Flush();

            Debug.WriteLine($"[{tag}] Crash report saved to: {crashFile.AbsolutePath}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[{tag}] Failed to save crash report to file: {ex.Message}");
        }
    }

    private static void CleanupCrashDirectory()
    {
        try
        {
            var context = Platform.CurrentActivity?.ApplicationContext ?? global::Android.App.Application.Context;
            var crashDirPath = Path.Combine(context.FilesDir?.AbsolutePath ?? "", "crashes");
            if (!Directory.Exists(crashDirPath))
            {
                return;
            }

            // Reasonable defaults
            const int maxFiles = 10; // keep at most this many crash files
            const long maxTotalBytes = 512 * 1024; // ~512 KB cap
            var retention = TimeSpan.FromDays(14); // drop files older than this
            const long safetyWindowMs = 30_000; // don't delete files created within last 30s

            var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var files = Directory.GetFiles(crashDirPath, "crash_*.txt");

            // Prepare file metadata
            var items = files
                .Select(path =>
                {
                    var fi = new FileInfo(path);
                    var ts = ExtractTimestamp(fi.Name); // epoch ms from filename, or 0 if not found
                    var effectiveTs = ts > 0 ? ts : new DateTimeOffset(fi.LastWriteTimeUtc).ToUnixTimeMilliseconds();
                    return new
                    {
                        Path = path,
                        Info = fi,
                        TimestampMs = effectiveTs,
                        AgeMs = nowMs - effectiveTs,
                        Length = fi.Exists ? fi.Length : 0L
                    };
                })
                .Where(x => x.Info.Exists)
                .OrderBy(x => x.TimestampMs) // oldest first
                .ToList();

            if (items.Count == 0)
            {
                return;
            }

            bool IsRecent(long ageMs) => ageMs < safetyWindowMs;

            // Pass 1: remove files older than retention (avoid very recent files)
            foreach (var item in items.Where(x => !IsRecent(x.AgeMs) && x.AgeMs > retention.TotalMilliseconds).ToList())
            {
                TryDelete(item.Path);
                items.Remove(item);
            }

            // Pass 2: enforce max file count (first avoid recent files)
            if (items.Count > maxFiles)
            {
                var overBy = items.Count - maxFiles;

                // Delete the oldest non-recent ones first
                var nonRecentOldest = items.Where(x => !IsRecent(x.AgeMs)).OrderBy(x => x.TimestampMs).Take(overBy)
                    .ToList();
                foreach (var item in nonRecentOldest)
                {
                    TryDelete(item.Path);
                    items.Remove(item);
                    overBy--;
                    if (overBy <= 0) break;
                }

                // If still over, delete oldest even if recent (last resort)
                if (overBy > 0)
                {
                    var oldestAny = items.OrderBy(x => x.TimestampMs).Take(overBy).ToList();
                    foreach (var item in oldestAny)
                    {
                        TryDelete(item.Path);
                        items.Remove(item);
                    }
                }
            }

            var totalBytes = items.Sum(x => x.Length);
            if (totalBytes > maxTotalBytes)
            {
                foreach (var item in items
                             .Where(x => !IsRecent(x.AgeMs))
                             .OrderBy(x => x.TimestampMs)
                             .TakeWhile(item => totalBytes > maxTotalBytes)
                             .Where(i => TryDelete(i.Path))
                             .ToList()
                        )
                {
                    totalBytes -= item.Length;
                    items.Remove(item);
                }

                // If still too large, delete oldest remaining (even if recent, last resort)
                if (totalBytes > maxTotalBytes)
                {
                    foreach (var item in items
                                 .OrderBy(x => x.TimestampMs)
                                 .TakeWhile(item => totalBytes > maxTotalBytes)
                                 .Where(i => TryDelete(i.Path))
                                 .ToList()
                            )
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

        return;

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
                Debug.WriteLine($"[Bugsnag crash cleanup] Failed to delete {path}: {delEx.Message}");
                return false;
            }
        }
    }

    private static long ExtractTimestamp(string filePath)
    {
        var match = TimeStampRegex.Match(filePath);
        if (match.Success && long.TryParse(match.Groups[1].Value, out var timestamp))
        {
            return timestamp;
        }

        return 0;
    }

    [GeneratedRegex(@"crash_(\d+)\.txt"
        , RegexOptions.Compiled)]
    private static partial Regex TimeStampRegex { get; }
}