using System.Security.Cryptography;
using System.Text;

namespace Bugsnag.Maui.Models;

public delegate bool TransformBugsnagEventDelegate(BugsnagEvent eventToTransform);

public sealed partial record BugsnagEvent
{
    /// <summary>
    /// Sets a custom grouping hash for this event. This can be used to override the default
    /// grouping behavior, for example to group events by a specific property or value.
    /// Note that the grouping hash is hashed using SHA-1 before being sent to Bugsnag, as a consumer,
    /// you do not have to create the hash yourself, simply concat the desired strings.
    /// </summary>
    public partial string? GroupingHash { get; set; }

    public partial string? Context { get; set; }

    public required IReadOnlyList<BugsnagError> Errors { get; init; }

    internal static string? GenerateHash(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA1.HashData(bytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
