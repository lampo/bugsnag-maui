namespace Bugsnag.Maui.Models;

public partial record BugsnagEvent
{
    public partial string? GroupingHash
    {
        get;
        set => field = value;
    }

    public partial string? Context
    {
        get;
        set => field = value;
    }
}
