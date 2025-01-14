using Bugsnag.iOS;

namespace Bugsnag.Maui;

public partial class BugsnagBuilder
{
    internal partial BugsnagMaui Build()
    {
        ArgumentException.ThrowIfNullOrEmpty(this.apiKey);

        var config = new BugsnagConfiguration(this.apiKey);
        config.AutoDetectErrors = this.autoDetectErrors;
        config.LaunchDurationMillis =
            (uint?)this.launchDurationMillis ?? config.LaunchDurationMillis;
        config.ReleaseStage = this.releaseStage.ToString().ToLowerInvariant();
        config.AttemptDeliveryOnCrash = this.attemptDeliveryOnCrash;

        return new BugsnagMaui(config);
    }
}
