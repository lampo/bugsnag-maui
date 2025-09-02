using System.Collections.Generic;

namespace Bugsnag.Maui.Models;

public sealed partial record BugsnagError
{
    public IReadOnlyList<string> StackTrace { get; set; } = [];

    public partial string? ErrorMessage { get; set; }

    public partial string? ErrorClass { get; set; }
}
