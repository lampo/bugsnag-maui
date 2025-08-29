using AndroidConfiguration = Com.Bugsnag.Android.Configuration;

namespace Bugsnag.Maui;

public partial class BugsnagBuilder
{
    internal partial BugsnagMaui Build()
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey);

        var config = new AndroidConfiguration(apiKey);
        config.AutoDetectErrors = autoDetectErrors;
        config.LaunchDurationMillis = launchDurationMillis ?? config.LaunchDurationMillis;
        config.ReleaseStage = releaseStage.ToString().ToLowerInvariant();
        config.AttemptDeliveryOnCrash = attemptDeliveryOnCrash;
        return new BugsnagMaui(config, onSendTransform);
    }
}
