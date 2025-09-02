using System.Diagnostics;
using System.Text.Json;
using Bugsnag.iOS;
using Bugsnag.Maui.Models;
using Foundation;
using BugsnagEvent = Bugsnag.Maui.Models.BugsnagEvent;
using NativeBugsnagEvent = Bugsnag.iOS.BugsnagEvent;

namespace Bugsnag.Maui;

public partial class BugsnagMaui
{
    private BugsnagBindingClient? bugsnagBindingClient;
    private readonly BugsnagConfiguration config;
    private readonly TransformBugsnagEventDelegate? transform;

    public BugsnagMaui(BugsnagConfiguration config, TransformBugsnagEventDelegate? transform)
    {
        this.config = config;
        this.transform = transform;
        ConfigureErrorHandling();
    }

    public void Start()
    {
        if (bugsnagBindingClient != null)
        {
            return;
        }

        bugsnagBindingClient = new BugsnagBindingClient();
        config.AddOnSendErrorBlock(OnSendError);
        bugsnagBindingClient.Start(config);
    }

    private bool OnSendError(NativeBugsnagEvent bugsnagEvent)
    {
        try
        {
            return transform == null || transform(BugsnagEvent.FromNativeEvent(bugsnagEvent));
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return true;
        }
    }

    public void MarkLaunchCompleted()
    {
        bugsnagBindingClient?.MarkLaunchCompleted();
    }

    public void AddFeatureFlag(string name, string variant)
    {
        bugsnagBindingClient?.AddFeatureFlag(name, variant);
    }

    public void ClearFeatureFlag(string name)
    {
        bugsnagBindingClient?.ClearFeatureFlag(name);
    }

    public void ClearFeatureFlags()
    {
        bugsnagBindingClient?.ClearFeatureFlags();
    }

    public void SetUser(string userId)
    {
        bugsnagBindingClient?.SetUser(userId, null, null);
    }

    public void LeaveBreadcrumb(string message)
    {
        bugsnagBindingClient?.LeaveBreadcrumb(message);
    }

    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata)
    {
        bugsnagBindingClient?.LeaveBreadcrumb(
            message,
            ConvertJsonToNsDictionary(JsonSerializer.Serialize(metadata)),
            BSGBreadcrumbType.State
        );
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled)
    {
        if (bugsnagBindingClient == null)
        {
            return;
        }

        var @event = bugsnagBindingClient.CreateEvent(
            ConvertJsonToNsDictionary(JsonSerializer.Serialize(report))!,
            unhandled,
            false
        );
        bugsnagBindingClient.DeliverEvent(@event);
    }

    public static NSDictionary? ConvertJsonToNsDictionary(string jsonString)
    {
        NSData jsonData = NSData.FromString(jsonString, NSStringEncoding.UTF8);
        NSDictionary dictionary = (NSDictionary)
            NSJsonSerialization.Deserialize(
                jsonData,
                NSJsonReadingOptions.MutableContainers,
                out var error
            );

        if (error == null!)
        {
            return dictionary;
        }

        // Handle error
        Console.WriteLine($"Error converting JSON to NSDictionary: {error.LocalizedDescription}");
        return null;
    }
}
