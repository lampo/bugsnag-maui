namespace Bugsnag.Maui.Models;

public partial record BugsnagError
{
    public partial string? ErrorMessage
    {
        get;
        set => field = value;
    }

    public partial string? ErrorClass
    {
        get;
        set => field = value;
    }
}