using Bugsnag.Maui;
using Microsoft.Extensions.Logging;

namespace SampleApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBugsnag(bugsnag =>
                    bugsnag
                        .WithApiKey("4fa1861e2c0ed84146c19c8cf248a53a")
                        .WithReleaseStage(ReleaseStage.Local)
                )
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
