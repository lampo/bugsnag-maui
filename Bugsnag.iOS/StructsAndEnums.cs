using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace Bugsnag.iOS
{
    public enum BSGBreadcrumbType : uint
    {
        Manual,
        Error,
        Log,
        Navigation,
        Process,
        Request,
        State,
        User,
    }

    [Flags]
    public enum BSGEnabledBreadcrumbType : uint
    {
        None = 0,
        State = 1 << 1,
        User = 1 << 2,
        Log = 1 << 3,
        Navigation = 1 << 4,
        Request = 1 << 5,
        Process = 1 << 6,
        Error = 1 << 7,
        All = State | User | Log | Navigation | Request | Process | Error,
    }

    public enum BSGSeverity : uint
    {
        Error,
        Warning,
        Info,
    }

    public enum BSGErrorType : uint
    {
        Cocoa,
        C,
        ReactNativeJs,
        CSharp,
    }

    public enum BSGThreadType : uint
    {
        Cocoa = 0,
        ReactNativeJs = 1 << 1,
    }
}
