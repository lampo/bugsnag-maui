using System.Text.Json;
using Bugsnag.iOS;

namespace Bugsnag.Maui;

public partial class BugsnagMaui
{
    public void Start(ReleaseStage releaseStage)
    {
        var config = new BugsnagConfiguration();
        config.ApiKey = this.apiKey;
        config.ReleaseStage = releaseStage.ToString().ToLowerInvariant();
        config.AutoDetectErrors = true;
        BugsnagBindingClient.StartBugsnagWithConfiguration(config, "0.0.0");
    }

    public void MarkLaunchCompleted()
    {
        BugsnagBindingClient.MarkLaunchCompleted();
    }

    public void AddFeatureFlag(string name, string variant)
    {
        BugsnagBindingClient.AddFeatureFlag(name, variant);
    }

    public void ClearFeatureFlag(string name)
    {
        BugsnagBindingClient.ClearFeatureFlag(name);
    }

    public void ClearFeatureFlags()
    {
        BugsnagBindingClient.ClearFeatureFlags();
    }

    public void SetUser(string userId)
    {
        BugsnagBindingClient.SetUser(userId, null, null);
    }

    public void LeaveBreadcrumb(string message)
    {
        BugsnagBindingClient.AddBreadcrumb(message, "State", null);
    }

    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata)
    {
        BugsnagBindingClient.AddBreadcrumb(message, "State", JsonSerializer.Serialize(metadata));
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled)
    {
        
        
        
    }

}
