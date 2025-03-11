namespace Bugsnag.Maui;

internal static class MauiExceptions
{
    // We'll route all unhandled exceptions through this one event.
    public static event UnhandledExceptionEventHandler? UnhandledException;

    static MauiExceptions()
    {
#if IOS || MACCATALYST
        // For iOS and Mac Catalyst
        // Exceptions will flow through AppDomain.CurrentDomain.UnhandledException,
        // but we need to set UnwindNativeCode to get it to work correctly.
        //
        // See: https://github.com/xamarin/xamarin-macios/issues/15252

        // This is the normal event expected, and should still be used.
        // It will fire for exceptions from iOS and Mac Catalyst,
        // and for exceptions on background threads from WinUI 3.
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            UnhandledException?.Invoke(sender, args);
            Console.WriteLine("Exception Handled from Domain");
        };

        ObjCRuntime.Runtime.MarshalManagedException += (_, args) =>
        {
            args.ExceptionMode = ObjCRuntime.MarshalManagedExceptionMode.UnwindNativeCode;
        };

#elif ANDROID

        // For Android:
        // All exceptions will flow through Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser,
        // and NOT through AppDomain.CurrentDomain.UnhandledException

        global::Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            UnhandledException?.Invoke(sender!,
                new UnhandledExceptionEventArgs(args.Exception, true)
            );
            Console.WriteLine("Exception Handled from Android Environment");
        };

#endif
    }
}
