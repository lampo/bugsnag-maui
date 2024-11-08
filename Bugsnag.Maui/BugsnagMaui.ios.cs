using System.Text.Json;
using Bugsnag.iOS;
using Foundation;

namespace Bugsnag.Maui;

public partial class BugsnagMaui
{
    private BugsnagBindingClient? bugsnagBindingClient;
    private readonly BugsnagConfiguration config;

    public BugsnagMaui(BugsnagConfiguration config)
    {
        this.config = config;
        this.config.AddOnSendErrorBlock(ShouldSendError);
        this.ConfigureErrorHandling();
    }
    
    public void Start()
    {
        if (bugsnagBindingClient != null)
        {
            return;
        }
        
        this.bugsnagBindingClient = new BugsnagBindingClient();
        this.bugsnagBindingClient.Start(config);
    }

    private bool ShouldSendError(BugsnagEvent bugsnagEvent)
    {
        if (!bugsnagEvent.Unhandled 
            || !(bugsnagEvent.Errors?.Length > 0) 
            || !(bugsnagEvent.Errors[0].Stacktrace?.Length > 1))
        {
            return true;
        }
        
        // Maui Errors are handled in the Maui layer, so we do not want to send the
        // native crash reports to Bugsnag. only go back 5 frames since the
        // xamaerin_unhandled_exception_handler is the best indicator that the error
        // was handled in the Maui layer.
        var stackTrace = bugsnagEvent.Errors[0].Stacktrace;
        for (int frame = 0; frame < 5; frame++)
        {
            if (frame >= stackTrace.Length)
            {
                return true;
            }
            
            if (stackTrace[frame].Method?.Contains("xamarin_unhandled_exception_handler") == true)
            {
                return false;
            }
        }
        
        return true;
    }

    public void MarkLaunchCompleted()
    {
        this.bugsnagBindingClient?.MarkLaunchCompleted();
    }

    public void AddFeatureFlag(string name, string variant)
    {
        this.bugsnagBindingClient?.AddFeatureFlag(name, variant);
    }

    public void ClearFeatureFlag(string name)
    {
        this.bugsnagBindingClient?.ClearFeatureFlag(name);
    }

    public void ClearFeatureFlags()
    {
        this.bugsnagBindingClient?.ClearFeatureFlags();
    }

    public void SetUser(string userId)
    {
        this.bugsnagBindingClient?.SetUser(userId, null, null);
    }

    public void LeaveBreadcrumb(string message)
    {
        this.bugsnagBindingClient?.LeaveBreadcrumb(message);
    }

    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata)
    {
        
        this.bugsnagBindingClient?.LeaveBreadcrumb(message, ConvertJsonToNsDictionary(JsonSerializer.Serialize(metadata)), BSGBreadcrumbType.State);
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled)
    {
        if (this.bugsnagBindingClient == null)
        {
            return;
        }
        
        var @event = this.bugsnagBindingClient.CreateEvent(ConvertJsonToNsDictionary(JsonSerializer.Serialize(report)), unhandled, false);
        this.bugsnagBindingClient.DeliverEvent(@event);
    }
    
    public static NSDictionary ConvertJsonToNsDictionary(string jsonString)
    {
        NSError error;
        NSData jsonData = NSData.FromString(jsonString, NSStringEncoding.UTF8);
        NSDictionary dictionary = (NSDictionary)NSJsonSerialization.Deserialize(jsonData, NSJsonReadingOptions.MutableContainers, out error);

        if (error != null)
        {
            // Handle error
            Console.WriteLine($"Error converting JSON to NSDictionary: {error.LocalizedDescription}");
            return null;
        }

        return dictionary;
    }

}
