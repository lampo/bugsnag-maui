using AndroidConfiguration = Com.Bugsnag.Android.Configuration;

namespace Bugsnag.Maui;

public partial class BugsnagBuilder
{
    internal partial BugsnagMaui Build()
    {
        ArgumentException.ThrowIfNullOrEmpty(this.apiKey);

        var config = new AndroidConfiguration(this.apiKey);
        config.AutoDetectErrors = this.autoDetectErrors;
        config.LaunchDurationMillis = this.launchDurationMillis ?? config.LaunchDurationMillis;
        config.ReleaseStage = this.releaseStage.ToString().ToLowerInvariant();
        config.AttemptDeliveryOnCrash = this.attemptDeliveryOnCrash;
        return new BugsnagMaui(config);
    }
}
