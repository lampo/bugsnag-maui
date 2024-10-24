namespace Bugsnag.Maui;

public partial class BugsnagMaui
{
    public void Start() { }

    public void MarkLaunchCompleted() { }

    public void AddFeatureFlag(string name, string variant) { }

    public void ClearFeatureFlag(string name) { }

    public void ClearFeatureFlags() { }

    public void SetUser(string userId) { }

    public void LeaveBreadcrumb(string message) { }

    public void LeaveBreadcrumb(string message, Dictionary<string, object> metadata) { }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled) { }
}
