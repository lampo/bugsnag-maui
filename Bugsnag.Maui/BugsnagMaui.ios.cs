using System.Diagnostics;
using Bugsnag.iOS;
using Foundation;
using UIKit;

namespace Bugsnag.Maui;

public partial class BugsnagMaui
{
    public void Start(ReleaseStage releaseStage)
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

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled) { }

    private static NSException ConvertToNsException(Exception exception)
    {
        // Use the message, stack trace, and type of the System.Exception to create an NSException
        var exceptionName = exception.GetType().FullName;
        var exceptionReason = exception.Message;

        var userInfo = new NSDictionary(
            new NSString("StackTrace"),
            new NSString(exception.StackTrace)
        );

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
