namespace Bugsnag;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMasterCardConnect(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IBugsnag, Bugsnag>();
        return builder;
    }
}