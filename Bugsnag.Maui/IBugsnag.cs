namespace Bugsnag;

public interface IBugsnag
{
    void Start();
    void MarkLaunchCompleted();
    void AddFeatureFlag(string name, string variant);
    void ClearFeatureFlag(string name);
    void ClearFeatureFlags();
    void NotifyError(string error, Dictionary<string, object> metadata);
    void NotifyError(string error);
    void Notify(Exception exception);
    void SetUser(string userId);
    void LeaveBreadcrumb(string message);
    void LeaveBreadcrumb(string message, Dictionary<string, object> metadata);
}