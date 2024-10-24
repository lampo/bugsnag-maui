namespace Bugsnag.Maui;

public partial class BugsnagBuilder
{
    internal partial BugsnagMaui Build()
    {
        ArgumentException.ThrowIfNullOrEmpty(this.apiKey);

        return new BugsnagMaui();
    }
}