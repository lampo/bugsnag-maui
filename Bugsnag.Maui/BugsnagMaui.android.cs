using System.Text.Json;
using Com.Bugsnag.Android;
using Org.Json;
using AndroidBreadcrumbType = Com.Bugsnag.Android.BreadcrumbType;
using AndroidBugsnag = Com.Bugsnag.Android.Bugsnag;
using AndroidConfiguration = Com.Bugsnag.Android.Configuration;
using BugsnagWrapper = Com.Bugsnag.Maui.BugsnagMaui;
using Object = Java.Lang.Object;

namespace Bugsnag.Maui;

public partial class BugsnagMaui : Object, IOnSendCallback, IOnErrorCallback
{
    private readonly AndroidConfiguration config;
    private readonly BugsnagWrapper client = new();

    public BugsnagMaui(AndroidConfiguration config)
    {
        this.config = config;
        this.config.AddOnSend(this);
        this.config.AddOnError(this);
        this.ConfigureErrorHandling();
    }

    public bool OnError(Event bugsnagEvent)
    {
        bugsnagEvent.Context = null;

        return true;
    }

    public bool OnSend(Event bugsnagEvent)
    {
        // Maui Errors are handled in the Maui layer, so we do not want to send the
        // native crash reports to Bugsnag. android.runtime.JavaProxyThrowable is
        // the best indicator that the error was handled in the Maui layer.
        return !(bugsnagEvent.Errors?.Count > 0)
            || bugsnagEvent.Errors[0].ErrorClass != "android.runtime.JavaProxyThrowable";
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
