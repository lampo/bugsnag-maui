using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

[StructLayout (LayoutKind.Sequential)]
public struct BSG_KSCrashReportWriter
{
	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, bool>* addBooleanElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, double>* addFloatingPointElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, long>* addIntegerElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, ulong>* addUIntegerElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, sbyte*>* addStringElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, sbyte*>* addTextFileElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, sbyte*>* addJSONFileElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, sbyte*, nuint>* addDataElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*>* beginDataElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, nuint>* appendDataElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*>* endDataElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, byte*>* addUUIDElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*, sbyte*>* addJSONElement;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*>* beginObject;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*, sbyte*>* beginArray;

	public unsafe Action<Bugsnag.BSG_KSCrashReportWriter*>* endContainer;

	public unsafe void* context;
}

[Native]
public enum BSGBreadcrumbType : ulong
{
	Manual,
	Error,
	Log,
	Navigation,
	Process,
	Request,
	State,
	User
}

[Flags]
[Native]
public enum BSGEnabledBreadcrumbType : ulong
{
	None = 0x0,
	State = 1uL << 1,
	User = 1uL << 2,
	Log = 1uL << 3,
	Navigation = 1uL << 4,
	Request = 1uL << 5,
	Process = 1uL << 6,
	Error = 1uL << 7,
	All = State | User | Log | Navigation | Request | Process | Error
}

[Native]
public enum BSGSeverity : ulong
{
	Error,
	Warning,
	Info
}

[Native]
public enum BSGThreadSendPolicy : long
{
	Always = 0,
	UnhandledOnly = 1,
	Never = 2
}

[Flags]
[Native]
public enum BSGTelemetryOptions : ulong
{
	InternalErrors = (1uL << 0),
	Usage = (1uL << 1),
	All = (InternalErrors | Usage)
}

[Flags]
[Native]
public enum BSGErrorType : ulong
{
	Cocoa,
	C,
	ReactNativeJs,
	CSharp
}

[Flags]
[Native]
public enum BSGThreadType : ulong
{
	Cocoa = 0x0,
	ReactNativeJs = 1uL << 1
}
