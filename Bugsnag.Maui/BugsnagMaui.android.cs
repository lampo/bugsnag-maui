using System.Text.Json;
using Org.Json;
using AndroidBreadcrumbType = Com.Bugsnag.Android.BreadcrumbType;
using AndroidBugsnag = Com.Bugsnag.Android.Bugsnag;
using AndroidConfiguration = Com.Bugsnag.Android.Configuration;
using BugsnagWrapper = Com.Bugsnag.Maui.BugsnagMaui;
using Object = Java.Lang.Object;

namespace Bugsnag.Maui;

public partial class BugsnagMaui
{
    private readonly BugsnagWrapper client = new();

    public void Start(ReleaseStage releaseStage)
    {
        if (AndroidBugsnag.IsStarted)
        {
            return;
        }

        var context = Platform.CurrentActivity!;
        var config = new AndroidConfiguration(this.apiKey);
        config.AutoDetectErrors = false;
        config.LaunchDurationMillis = 0;
        config.ReleaseStage = releaseStage.ToString().ToLowerInvariant();
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
        AndroidBugsnag.LeaveBreadcrumb(
            message,
            ConvertJavaDictionary(metadata),
            AndroidBreadcrumbType.State!
        );
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

    private static IDictionary<string, Object> ConvertJavaDictionary(
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
}
