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
        BugsnagBinding.StartBugsnagWithConfiguration(config, "0.0.0");
    }

    public void MarkLaunchCompleted()
    {
        BugsnagBinding.MarkLaunchCompleted();
    }

    public void AddFeatureFlag(string name, string variant)
    {
        BugsnagBinding.AddFeatureFlag(name, variant);
    }

    public void ClearFeatureFlag(string name)
    {
        BugsnagBinding.ClearFeatureFlag(name);
    }

    public void ClearFeatureFlags()
    {
        BugsnagBinding.ClearFeatureFlags();
    }

    public void SetUser(string userId)
    {
        BugsnagBinding.SetUser(userId, null, null);
    }

    public void LeaveBreadcrumb(string message)
    {
        BugsnagBinding.AddBreadcrumb(message, "State", null);
    }

    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata)
    {
        BugsnagBinding.AddBreadcrumb(message, "State", JsonSerializer.Serialize(metadata));
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled)
    {
        
        
        
    }

}
