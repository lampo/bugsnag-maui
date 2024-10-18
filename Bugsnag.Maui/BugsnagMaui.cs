namespace Bugsnag.Maui;

public partial class BugsnagMaui : IBugsnag
{
    private string apiKey;

    public BugsnagMaui(string apiKey)
    {
        MauiExceptions.UnhandledException += (sender, args) =>
        {
            var report = new Payload.Exception((Exception)args.ExceptionObject);
            PlatformNotify(report, true);
        };
        this.apiKey = apiKey;
    }

    public void Notify(Exception exception)
    {
        var report = new Payload.Exception(exception);
        PlatformNotify(report, false);
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled);
}
