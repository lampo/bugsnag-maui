using System.Diagnostics;
using Foundation;
using UIKit;
using Bugsnag.iOS;

namespace Bugsnag;

public partial class Bugsnag
{
    public void Start()
    {
        DotnetBugsnagBinding.Start();
    }
    
    public void MarkLaunchCompleted()
    {
        DotnetBugsnagBinding.MarkLaunchCompleted();
    }
    
    public void AddFeatureFlag(string name, string variant)
    {
        DotnetBugsnagBinding.AddFeatureFlagWith(name, variant);
    }
    
    public void ClearFeatureFlag(string name)
    {
        DotnetBugsnagBinding.ClearFeatureFlagWith(name);
    }
    
    public void ClearFeatureFlags()
    {
        DotnetBugsnagBinding.ClearFeatureFlags();
    }
    
    public void NotifyError(string error, Dictionary<string, object> metadata)
    {
        var nsError = new NSError(new NSString(error), 0, ConvertToNsDictionary(metadata));
        DotnetBugsnagBinding.NotifyErrorWith(nsError);
    }
    
    public void NotifyError(string error)
    {
        var nsError = new NSError(new NSString(error), 0, null);
        DotnetBugsnagBinding.NotifyErrorWith(nsError);
    }
    
    public void Notify(Exception exception)
    {
        DotnetBugsnagBinding.NotifyWith(ConvertToNsException(exception));
    }
    
    public void SetUser(string userId)
    {
        DotnetBugsnagBinding.SetUserWith(userId);
    }
    
    public void LeaveBreadcrumb(string message)
    {
        DotnetBugsnagBinding.LeaveBreadcrumbWith(message);
    }
    
    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata)
    {
        DotnetBugsnagBinding.LeaveBreadcrumbWith(message, ConvertToNsDictionary(metadata));
    }

    private static NSException ConvertToNsException(Exception exception)
    {
        // Use the message, stack trace, and type of the System.Exception to create an NSException
        var exceptionName = exception.GetType().FullName;
        var exceptionReason = exception.Message;

        var userInfo =
            new NSDictionary(new NSString("StackTrace"), new NSString(exception.StackTrace));

        var nsException = new NSException(exceptionName!, exceptionReason, userInfo);

        return nsException;
    }
    
    private static NSMutableDictionary ConvertToNsDictionary(Dictionary<string, object> dictionary)
    {
        var nsDictionary = new NSMutableDictionary();
        foreach (var kvp in dictionary)
        {
            var key = new NSString(kvp.Key);
            NSObject value;

            switch (kvp.Value)
            {
                case string str:
                    value = new NSString(str);
                    break;
                case int i:
                    value = new NSNumber(i);
                    break;
                case float f:
                    value = new NSNumber(f);
                    break;
                case double d:
                    value = new NSNumber(d);
                    break;
                case bool b:
                    value = new NSNumber(b);
                    break;
                case null:
                    value = NSNull.Null;
                    break;
                default:
                    throw new ArgumentException($"Unsupported type: {kvp.Value.GetType()}");
            }

            nsDictionary.Add(key, value);
        }
        return nsDictionary;
    }
}