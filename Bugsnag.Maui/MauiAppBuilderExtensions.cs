using Microsoft.Maui.LifecycleEvents;

namespace Bugsnag.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseBugsnag(
        this MauiAppBuilder builder,
        ReleaseStage releaseStage,
        string apiKey
    )
    {
        var instance = new BugsnagMaui(apiKey);
        builder.Services.AddSingleton<IBugsnag>(_ => instance);
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android =>
                android.OnCreate((activity, bundle) => instance.Start(releaseStage))
            );
#elif IOS
            events.AddiOS(ios =>
                ios.FinishedLaunching(
                    (_, _) =>
                    {
                        instance.Start(releaseStage);
                        return true;
                    }
                )
            );
#endif
        });
        return builder;
    }
}
