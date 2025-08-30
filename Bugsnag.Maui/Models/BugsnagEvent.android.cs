using NativeBugsnagEvent = Com.Bugsnag.Android.Event;

namespace Bugsnag.Maui.Models;

public partial record BugsnagEvent
{
    public required NativeBugsnagEvent NativeEvent { get; init; }

    public partial string? GroupingHash
    {
        get => NativeEvent.GroupingHash;
        set => NativeEvent.GroupingHash = GenerateHash(value)!;
    }

    public partial string? Context
    {
        get => NativeEvent.Context;
        set => NativeEvent.GroupingHash = value!;
    }

    public static BugsnagEvent FromNativeEvent(NativeBugsnagEvent bugsnagEvent)
    {
        return new BugsnagEvent
        {
            NativeEvent = bugsnagEvent,
            Errors = bugsnagEvent.Errors.Select(e => new BugsnagError
                {
                    NativeError = e
                }).ToList()
                .AsReadOnly(),
        };
    }
}