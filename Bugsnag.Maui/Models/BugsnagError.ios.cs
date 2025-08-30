using NativeBugsnagError = Bugsnag.iOS.BugsnagError;

namespace Bugsnag.Maui.Models;

public partial record BugsnagError
{
    public required NativeBugsnagError NativeError
    {
        get;
        init
        {
            field = value;
            StackTrace = value.Stacktrace?
                .Select(s => s.Method ?? "<unknown>")
                .ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
        }
    }

    public partial string? ErrorMessage
    {
        get => NativeError.ErrorMessage;
        set => NativeError.ErrorMessage = value;
    }

    public partial string? ErrorClass
    {
        get => NativeError.ErrorClass;
        set => NativeError.ErrorClass = value!;
    }
}