namespace Bugsnag.Maui;

public partial class BugsnagMaui : IBugsnag
{
    public static IBugsnag Current { get; private set; } = null!;

    public void Notify(Exception exception)
    {
        var report = new Payload.Exception(exception);
        PlatformNotify(report, false);
    }

    private partial void PlatformNotify(Dictionary<string, object> report, bool unhandled);

    private void ConfigureErrorHandling()
    {
        Current = this;
        MauiExceptions.UnhandledException += (sender, args) =>
        {
            var report = new Payload.Exception((Exception)args.ExceptionObject);
            PlatformNotify(report, true);
        };
    }
}
