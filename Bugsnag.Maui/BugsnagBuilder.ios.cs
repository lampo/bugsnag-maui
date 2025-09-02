using Bugsnag.iOS;

namespace Bugsnag.Maui;

public partial class BugsnagBuilder
{
    internal partial BugsnagMaui Build()
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey);

        var config = new BugsnagConfiguration(apiKey);
        config.AutoDetectErrors = autoDetectErrors;
        config.LaunchDurationMillis = (uint?)launchDurationMillis ?? config.LaunchDurationMillis;
        config.ReleaseStage = releaseStage.ToString().ToLowerInvariant();
        config.AttemptDeliveryOnCrash = attemptDeliveryOnCrash;

        return new BugsnagMaui(config, onSendTransform);
    }
}
