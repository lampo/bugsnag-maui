using Microsoft.Maui.LifecycleEvents;

namespace Bugsnag.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseBugsnag(
        this MauiAppBuilder builder,
        Action<BugsnagBuilder> configureDelegate
    )
    {
        var configurationBuilder = new BugsnagBuilder();
        
        configureDelegate(configurationBuilder);
        
        var instance = configurationBuilder.Build();
        
        builder.Services.AddSingleton<IBugsnag>(_ => instance);
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android =>
                android.OnCreate((activity, bundle) => instance.Start())
            );
#elif IOS
            events.AddiOS(ios =>
                ios.FinishedLaunching(
                    (_, _) =>
                    {
                        instance.Start();
                        return true;
                    }
                )
            );
#endif
        });
        return builder;
    }
}
