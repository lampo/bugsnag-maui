using Bugsnag.Maui.Models;

namespace Bugsnag.Maui;

public partial class BugsnagBuilder
{
    private string? apiKey;
    private ReleaseStage releaseStage;
    private bool autoDetectErrors = true;
    private int? launchDurationMillis;
    private bool attemptDeliveryOnCrash;
    private TransformBugsnagEventDelegate? onSendTransform;

    public BugsnagBuilder WithApiKey(string apiKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey);
        this.apiKey = apiKey;
        return this;
    }

    public BugsnagBuilder WithReleaseStage(ReleaseStage releaseStage)
    {
        this.releaseStage = releaseStage;
        return this;
    }

    public BugsnagBuilder WithAutoDetectErrors(bool autoDetectErrors)
    {
        this.autoDetectErrors = autoDetectErrors;
        return this;
    }

    public BugsnagBuilder WithLaunchDurationMillis(int launchDurationMillis)
    {
        this.launchDurationMillis = launchDurationMillis;
        return this;
    }

    public BugsnagBuilder AttemptDeliveryOnCrash()
    {
        this.attemptDeliveryOnCrash = true;
        return this;
    }
    
    public BugsnagBuilder OnSendTransform(TransformBugsnagEventDelegate onSendTransform)
    {
        this.onSendTransform = onSendTransform;
        return this;
    }

    internal partial BugsnagMaui Build();
}
