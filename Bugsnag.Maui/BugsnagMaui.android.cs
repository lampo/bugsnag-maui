using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Bugsnag.Maui.Models;
using Com.Bugsnag.Android;
using Org.Json;
using AndroidBreadcrumbType = Com.Bugsnag.Android.BreadcrumbType;
using AndroidBugsnag = Com.Bugsnag.Android.Bugsnag;
using AndroidConfiguration = Com.Bugsnag.Android.Configuration;
using BugsnagWrapper = Com.Bugsnag.Maui.BugsnagMaui;
using Object = Java.Lang.Object;

namespace Bugsnag.Maui;

public partial class BugsnagMaui : Object, IOnSendCallback
{
    private readonly AndroidConfiguration config;
    private readonly TransformBugsnagEventDelegate? transform;
    private readonly BugsnagWrapper client = new();

    public static readonly HashSet<string> NativeCrashSignals = new()
    {
        "SIGSEGV", // Segmentation fault
        "SIGABRT", // Abort signal
        "SIGBUS", // Bus error
        "SIGFPE", // Floating point exception
        "SIGILL", // Illegal instruction
        "SIGTRAP", // Trace/breakpoint trap
        "SIGKILL", // Kill signal
        "SIGTERM", // Termination signal
        "SIGQUIT", // Quit signal
        "SIGSYS" // Bad system call
    };

    public BugsnagMaui(AndroidConfiguration config, TransformBugsnagEventDelegate? transform)
    {
        this.config = config;
        this.transform = transform;
        this.config.AddOnSend(this);
        ConfigureErrorHandling();
        AndroidRedirectStdToLogCat.Start();
    }

    public bool OnSend(Event bugsnagEvent)
    {
        var context = Platform.CurrentActivity?.ApplicationContext ?? global::Android.App.Application.Context;
        var crashDirPath = Path.Combine(context.FilesDir?.AbsolutePath ?? "", "crashes");
        var error = bugsnagEvent.Errors?.FirstOrDefault();

        // Maui Errors are handled in the Maui layer, so we do not want to send the
        // native crash reports to Bugsnag. android.runtime.JavaProxyThrowable is
        // the best indicator that the error was handled in the Maui layer.
        if (error?.ErrorClass == "android.runtime.JavaProxyThrowable")
        {
            return false;
        }

        if (!NativeCrashSignals.Contains(error?.ErrorClass ?? "") || !Directory.Exists(crashDirPath))
        {
            return ProcessTransform(bugsnagEvent);
        }

        try
        {
            // bugsnagEvent is Com.Bugsnag.Android.Event
            var when = bugsnagEvent.Device.Time?.Time ?? 0;

            var crashFiles = Directory.GetFiles(crashDirPath);

            // Get the most recent crash file
            var latestCrashFile = crashFiles
                .Where(e => Math.Abs(ExtractTimestamp(e) - when) < 15)
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .FirstOrDefault();

            if (latestCrashFile != null)
            {
                var crashContent = File.ReadAllText(latestCrashFile);

                AddStackTraceToEvent(bugsnagEvent, crashContent);

                File.Delete(latestCrashFile);

                Debug.WriteLine(
                    $"[Bugsnag crash Handler] Added saved crash report to Bugsnag event from: {Path.GetFileName(latestCrashFile)}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("[Bugsnag crash Handler] " + ex);
        }

        return ProcessTransform(bugsnagEvent);
    }

    public void Start()
    {
        if (AndroidBugsnag.IsStarted)
        {
            return;
        }

        var context = Platform.CurrentActivity!;
        client.Start(context, config);
    }

    public void MarkLaunchCompleted()
    {
        AndroidBugsnag.MarkLaunchCompleted();
    }

    public void AddFeatureFlag(string name, string variant)
    {
        AndroidBugsnag.AddFeatureFlag(name, variant);
    }

    public void ClearFeatureFlag(string name)
    {
        AndroidBugsnag.ClearFeatureFlag(name);
    }

    public void ClearFeatureFlags()
    {
        AndroidBugsnag.ClearFeatureFlags();
    }

    public void SetUser(string userId)
    {
        AndroidBugsnag.SetUser(userId, null, null);
    }

    public void LeaveBreadcrumb(string message)
    {
        AndroidBugsnag.LeaveBreadcrumb(message);
    }

    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata)
    {
        AndroidBugsnag.LeaveBreadcrumb(message,
            ConvertJavaDictionary(metadata),
            AndroidBreadcrumbType.State!);
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled)
    {
        var reportJson = JsonSerializer.Serialize(report);

        // Create a new JSONObject
        var jsonObject = new JSONObject(reportJson);

        jsonObject.Put("type", "android");

        var bugsnagEvent = client.CreateEvent(jsonObject, unhandled, false);

        client.DeliverEvent(bugsnagEvent);
    }

    private static Dictionary<string, Object> ConvertJavaDictionary(
        Dictionary<string, object> metadata
    )
    {
        var javaDictionary = new Dictionary<string, Object>();

        foreach (var kvp in metadata)
        {
            var javaObject = ConvertToJavaObject(kvp.Value);
            if (javaObject == null)
            {
                continue;
            }

            javaDictionary[kvp.Key] = javaObject;
        }

        return javaDictionary;
    }

    private static Object? ConvertToJavaObject(object value)
    {
        return value switch
        {
            null => null,
            int intValue => Java.Lang.Integer.ValueOf(intValue),
            long longValue => Java.Lang.Long.ValueOf(longValue),
            float floatValue => Java.Lang.Float.ValueOf(floatValue),
            double doubleValue => Java.Lang.Double.ValueOf(doubleValue),
            bool boolValue => Java.Lang.Boolean.ValueOf(boolValue),
            string stringValue => new Java.Lang.String(stringValue),
            _ => throw new ArgumentException($"Unsupported type: {value.GetType()}")
        };
    }

    private static void AddStackTraceToEvent(Event bugsnagEvent, string crashContent)
    {
        var error = bugsnagEvent.Errors?.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(crashContent) || error == null)
        {
            return;
        }

        var lines = crashContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        string? firstLine = null;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (!trimmedLine.StartsWith("at "))
            {
                continue;
            }

            var stackframe = ParseStackTraceLine(trimmedLine);
            if (stackframe == null)
            {
                continue;
            }

            var (method, file, lineNumber) = stackframe.Value;

            if (string.IsNullOrWhiteSpace(firstLine) && method != "<unknown>")
            {
                firstLine = method;
            }

            error.AddStackframe(method, "<unknown>", lineNumber);
        }

        if (string.IsNullOrWhiteSpace(firstLine))
        {
            return;
        }

        bugsnagEvent.GroupingHash = BugsnagEvent.GenerateHash(firstLine);
        bugsnagEvent.Context = firstLine;
    }

    private static (string? method, string? file, long lineNumber)? ParseStackTraceLine(string line)
    {
        try
        {
            // Remove "at " prefix
            var content = line[3..];

            // Split by the last space to separate the offset
            var lastSpaceIndex = content.LastIndexOf(' ');
            if (lastSpaceIndex == -1)
            {
                return null;
            }

            var methodPart = content[..lastSpaceIndex].Trim();

            // Handle <unknown> case
            if (methodPart == "<unknown>")
            {
                return ("<unknown>", null, 0);
            }

            // Split method part by colon to get class and method
            var colonIndex = methodPart.LastIndexOf(':');
            if (colonIndex == -1)
            {
                return null;
            }

            var className = methodPart[..colonIndex];
            var methodName = methodPart[(colonIndex + 1)..];

            return ($"{className}:{methodName}", methodName, lineNumber: 0);
        }
        catch (Exception)
        {
            // If parsing fails, return null to skip this frame
            return null;
        }
    }

    private bool ProcessTransform(Event bugsnagEvent)
    {
        return transform == null || transform(BugsnagEvent.FromNativeEvent(bugsnagEvent));
    }
    
    private long ExtractTimestamp(string filePath)
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
    private partial Regex TimeStampRegex { get; }
}