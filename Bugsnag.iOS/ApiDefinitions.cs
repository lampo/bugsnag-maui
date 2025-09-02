using System;
using Foundation;
using ObjCRuntime;

namespace Bugsnag.iOS
{
    // Delegate for BugsnagOnErrorBlock
    delegate void BugsnagOnErrorBlock(BugsnagEvent @event);

    // Delegate for BugsnagOnSessionBlock
    delegate bool BugsnagOnSessionBlock(BugsnagSession session);

    // Delegate for BugsnagOnBreadcrumbBlock
    delegate bool BugsnagOnBreadcrumbBlock(BugsnagBreadcrumb breadcrumb);

    [BaseType(typeof(NSObject))]
    interface BugsnagBindingClient
    {
        [Export("start")]
        BugsnagClient Start();

        [Export("startWithApiKey:")]
        BugsnagClient Start(string apiKey);

        [Export("startWithConfiguration:")]
        BugsnagClient Start(BugsnagConfiguration configuration);

        [Export("isStarted")]
        bool IsStarted { get; }

        [Static]
        [Export("lastRunInfo")]
        BugsnagLastRunInfo LastRunInfo { get; }

        [Export("markLaunchCompleted")]
        void MarkLaunchCompleted();

        [Export("notify:")]
        void Notify(NSException exception);

        [Export("notify:block:")]
        void Notify(NSException exception, [NullAllowed] Action<BugsnagEvent> block);

        [Export("notifyError:")]
        void NotifyError(NSError error);

        [Export("notifyError:block:")]
        void NotifyError(NSError error, [NullAllowed] Action<BugsnagEvent> block);

        [Export("leaveBreadcrumbWithMessage:")]
        void LeaveBreadcrumb(string message);

        [Export("leaveBreadcrumbForNotificationName:")]
        void LeaveBreadcrumbForNotificationName(string notificationName);

        [Export("leaveBreadcrumbWithMessage:metadata:andType:")]
        void LeaveBreadcrumb(
            string message,
            [NullAllowed] NSDictionary metadata,
            BSGBreadcrumbType type
        );

        [Export("leaveNetworkRequestBreadcrumbForTask:metrics:")]
        void LeaveNetworkRequestBreadcrumb(NSUrlSessionTask task, NSUrlSessionTaskMetrics metrics);

        [Export("breadcrumbs")]
        BugsnagBreadcrumb[] Breadcrumbs { get; }

        [Export("startSession")]
        void StartSession();

        [Export("pauseSession")]
        void PauseSession();

        [Export("resumeSession")]
        bool ResumeSession();

        [Export("setContext:")]
        void SetContext([NullAllowed] string context);

        [Export("context")]
        [NullAllowed]
        string Context { get; }

        [Export("user")]
        BugsnagUser User { get; }

        [Export("setUser:withEmail:andName:")]
        void SetUser(
            [NullAllowed] string userId,
            [NullAllowed] string email,
            [NullAllowed] string name
        );

        [Export("addFeatureFlagWithName:variant:")]
        void AddFeatureFlag(string name, [NullAllowed] string variant);

        [Export("addFeatureFlagWithName:")]
        void AddFeatureFlag(string name);

        [Export("addFeatureFlags:")]
        void AddFeatureFlags(BugsnagFeatureFlag[] featureFlags);

        [Export("clearFeatureFlagWithName:")]
        void ClearFeatureFlag(string name);

        [Export("clearFeatureFlags")]
        void ClearFeatureFlags();

        [Export("addOnSessionBlock:")]
        NSObject AddOnSessionBlock(Func<BugsnagSession, bool> block);

        [Export("removeOnSession:")]
        void RemoveOnSession(NSObject callback);

        [Export("addOnBreadcrumbBlock:")]
        NSObject AddOnBreadcrumbBlock(Func<BugsnagBreadcrumb, bool> block);

        [Export("removeOnBreadcrumb:")]
        void RemoveOnBreadcrumb(NSObject callback);

        [Export("createEvent:unhandled:deliver:")]
        NSDictionary CreateEvent(NSDictionary jsonError, bool unhandled, bool deliver);

        [Export("deliverEvent:")]
        void DeliverEvent(NSDictionary json);
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagEvent
    {
        [Export("context", ArgumentSemantic.Copy)]
        string Context { get; set; }

        [Export("severity", ArgumentSemantic.Assign)]
        BSGSeverity Severity { get; set; }

        [Export("errors", ArgumentSemantic.Copy)]
        BugsnagError[] Errors { get; set; }

        [Export("groupingHash", ArgumentSemantic.Copy)]
        string GroupingHash { get; set; }

        [Export("breadcrumbs", ArgumentSemantic.Copy)]
        BugsnagBreadcrumb[] Breadcrumbs { get; set; }

        [Export("featureFlags", ArgumentSemantic.Strong)]
        BugsnagFeatureFlag[] FeatureFlags { get; }

        [Export("apiKey", ArgumentSemantic.Copy)]
        string ApiKey { get; set; }

        [Export("device", ArgumentSemantic.Strong)]
        BugsnagDeviceWithState Device { get; }

        [Export("app", ArgumentSemantic.Strong)]
        BugsnagAppWithState App { get; }

        [Export("unhandled")]
        bool Unhandled { get; set; }

        [Export("threads", ArgumentSemantic.Copy)]
        BugsnagThread[] Threads { get; set; }

        [Export("originalError", ArgumentSemantic.Strong)]
        NSObject OriginalError { get; set; }

        [Export("user", ArgumentSemantic.Strong)]
        BugsnagUser User { get; }

        [Export("setUser:withEmail:andName:")]
        void SetUser(
            [NullAllowed] string userId,
            [NullAllowed] string email,
            [NullAllowed] string name
        );

        [Export("setCorrelationTraceId:spanId:")]
        void SetCorrelationTraceId(string traceId, string spanId);
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagBreadcrumb
    {
        [Export("timestamp", ArgumentSemantic.Copy)]
        NSDate Timestamp { get; }

        [Export("type", ArgumentSemantic.Assign)]
        BSGBreadcrumbType Type { get; set; }

        [Export("message", ArgumentSemantic.Copy)]
        string Message { get; set; }

        [Export("metadata", ArgumentSemantic.Copy)]
        NSDictionary Metadata { get; set; }
    }

    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface BSGBreadcrumbSink
    {
        [Abstract]
        [Export("leaveBreadcrumbWithMessage:metadata:andType:")]
        void LeaveBreadcrumb(
            string message,
            [NullAllowed] NSDictionary metadata,
            BSGBreadcrumbType type
        );
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagFeatureFlag
    {
        [Static]
        [Export("flagWithName:")]
        BugsnagFeatureFlag FlagWithName(string name);

        [Static]
        [Export("flagWithName:variant:")]
        BugsnagFeatureFlag FlagWithName(string name, [NullAllowed] string variant);

        [Export("name")]
        string Name { get; }

        [NullAllowed, Export("variant")]
        string Variant { get; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagDevice
    {
        [Export("jailbroken")]
        bool Jailbroken { get; set; }

        [NullAllowed, Export("id", ArgumentSemantic.Copy)]
        string Id { get; set; }

        [NullAllowed, Export("locale", ArgumentSemantic.Copy)]
        string Locale { get; set; }

        [NullAllowed, Export("manufacturer", ArgumentSemantic.Copy)]
        string Manufacturer { get; set; }

        [NullAllowed, Export("model", ArgumentSemantic.Copy)]
        string Model { get; set; }

        [NullAllowed, Export("modelNumber", ArgumentSemantic.Copy)]
        string ModelNumber { get; set; }

        [NullAllowed, Export("osName", ArgumentSemantic.Copy)]
        string OsName { get; set; }

        [NullAllowed, Export("osVersion", ArgumentSemantic.Copy)]
        string OsVersion { get; set; }

        [NullAllowed, Export("runtimeVersions", ArgumentSemantic.Copy)]
        NSDictionary<NSString, NSString> RuntimeVersions { get; set; }

        [NullAllowed, Export("totalMemory", ArgumentSemantic.Strong)]
        NSNumber TotalMemory { get; set; }
    }

    [BaseType(typeof(BugsnagDevice))]
    interface BugsnagDeviceWithState
    {
        [Export("freeDisk", ArgumentSemantic.Strong)]
        NSNumber FreeDisk { get; set; }

        [Export("freeMemory", ArgumentSemantic.Strong)]
        NSNumber FreeMemory { get; set; }

        [Export("orientation", ArgumentSemantic.Copy)]
        string Orientation { get; set; }

        [Export("time", ArgumentSemantic.Strong)]
        NSDate Time { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagUser
    {
        [NullAllowed, Export("id")]
        string Id { get; }

        [NullAllowed, Export("name")]
        string Name { get; }

        [NullAllowed, Export("email")]
        string Email { get; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagError
    {
        [NullAllowed, Export("errorClass", ArgumentSemantic.Copy)]
        string ErrorClass { get; set; }

        [NullAllowed, Export("errorMessage", ArgumentSemantic.Copy)]
        string ErrorMessage { get; set; }

        [Export("stacktrace", ArgumentSemantic.Copy)]
        BugsnagStackframe[] Stacktrace { get; set; }

        [Export("type", ArgumentSemantic.Assign)]
        BSGErrorType Type { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagApp
    {
        [NullAllowed, Export("binaryArch", ArgumentSemantic.Copy)]
        string BinaryArch { get; set; }

        [NullAllowed, Export("bundleVersion", ArgumentSemantic.Copy)]
        string BundleVersion { get; set; }

        [NullAllowed, Export("codeBundleId", ArgumentSemantic.Copy)]
        string CodeBundleId { get; set; }

        [NullAllowed, Export("dsymUuid", ArgumentSemantic.Copy)]
        string DsymUuid { get; set; }

        [NullAllowed, Export("id", ArgumentSemantic.Copy)]
        string Id { get; set; }

        [NullAllowed, Export("releaseStage", ArgumentSemantic.Copy)]
        string ReleaseStage { get; set; }

        [NullAllowed, Export("type", ArgumentSemantic.Copy)]
        string Type { get; set; }

        [NullAllowed, Export("version", ArgumentSemantic.Copy)]
        string Version { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagStackframe
    {
        [NullAllowed, Export("method", ArgumentSemantic.Copy)]
        string Method { get; set; }

        [NullAllowed, Export("machoFile", ArgumentSemantic.Copy)]
        string MachoFile { get; set; }

        [NullAllowed, Export("machoUuid", ArgumentSemantic.Copy)]
        string MachoUuid { get; set; }

        [NullAllowed, Export("frameAddress", ArgumentSemantic.Strong)]
        NSNumber FrameAddress { get; set; }

        [NullAllowed, Export("machoVmAddress", ArgumentSemantic.Strong)]
        NSNumber MachoVmAddress { get; set; }

        [NullAllowed, Export("symbolAddress", ArgumentSemantic.Strong)]
        NSNumber SymbolAddress { get; set; }

        [NullAllowed, Export("machoLoadAddress", ArgumentSemantic.Strong)]
        NSNumber MachoLoadAddress { get; set; }

        [Export("isPc")]
        bool IsPc { get; set; }

        [Export("isLr")]
        bool IsLr { get; set; }

        [NullAllowed, Export("type", ArgumentSemantic.Copy)]
        string Type { get; set; }

        [Static]
        [Export("stackframesWithCallStackReturnAddresses:")]
        BugsnagStackframe[] StackframesWithCallStackReturnAddresses(
            NSNumber[] callStackReturnAddresses
        );

        [Static]
        [Export("stackframesWithCallStackSymbols:")]
        BugsnagStackframe[] StackframesWithCallStackSymbols(string[] callStackSymbols);
    }

    [BaseType(typeof(BugsnagApp))]
    interface BugsnagAppWithState
    {
        [NullAllowed, Export("duration", ArgumentSemantic.Strong)]
        NSNumber Duration { get; set; }

        [NullAllowed, Export("durationInForeground", ArgumentSemantic.Strong)]
        NSNumber DurationInForeground { get; set; }

        [Export("inForeground")]
        bool InForeground { get; set; }

        [Export("isLaunching")]
        bool IsLaunching { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagThread
    {
        [NullAllowed, Export("id", ArgumentSemantic.Copy)]
        string Id { get; set; }

        [NullAllowed, Export("name", ArgumentSemantic.Copy)]
        string Name { get; set; }

        [Export("errorReportingThread")]
        bool ErrorReportingThread { get; }

        [NullAllowed, Export("state", ArgumentSemantic.Copy)]
        string State { get; set; }

        [Export("stacktrace", ArgumentSemantic.Copy)]
        BugsnagStackframe[] Stacktrace { get; set; }

        [Export("type", ArgumentSemantic.Assign)]
        BSGThreadType Type { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagConfiguration
    {
        [Static]
        [Export("loadConfig")]
        BugsnagConfiguration LoadConfig();

        [Export("initWithApiKey:")]
        IntPtr Constructor([NullAllowed] string apiKey);

        [Export("apiKey", ArgumentSemantic.Copy)]
        string ApiKey { get; set; }

        [NullAllowed, Export("releaseStage", ArgumentSemantic.Copy)]
        string ReleaseStage { get; set; }

        [NullAllowed, Export("enabledReleaseStages", ArgumentSemantic.Copy)]
        NSSet<NSString> EnabledReleaseStages { get; set; }

        [NullAllowed, Export("redactedKeys", ArgumentSemantic.Copy)]
        NSSet<NSObject> RedactedKeys { get; set; }

        [NullAllowed, Export("discardClasses", ArgumentSemantic.Copy)]
        NSSet<NSObject> DiscardClasses { get; set; }

        [NullAllowed, Export("context", ArgumentSemantic.Copy)]
        string Context { get; set; }

        [NullAllowed, Export("appVersion", ArgumentSemantic.Copy)]
        string AppVersion { get; set; }

        [Export("autoDetectErrors")]
        bool AutoDetectErrors { get; set; }

        [Export("appHangThresholdMillis")]
        nuint AppHangThresholdMillis { get; set; }

        [Export("reportBackgroundAppHangs")]
        bool ReportBackgroundAppHangs { get; set; }

        [Export("autoTrackSessions")]
        bool AutoTrackSessions { get; set; }

        [Export("launchDurationMillis")]
        nuint LaunchDurationMillis { get; set; }

        [Export("sendLaunchCrashesSynchronously")]
        bool SendLaunchCrashesSynchronously { get; set; }

        [Export("attemptDeliveryOnCrash")]
        bool AttemptDeliveryOnCrash { get; set; }

        [NullAllowed, Export("bundleVersion", ArgumentSemantic.Copy)]
        string BundleVersion { get; set; }

        [NullAllowed, Export("appType", ArgumentSemantic.Copy)]
        string AppType { get; set; }

        [Export("maxPersistedEvents")]
        nuint MaxPersistedEvents { get; set; }

        [Export("maxPersistedSessions")]
        nuint MaxPersistedSessions { get; set; }

        [Export("maxBreadcrumbs")]
        nuint MaxBreadcrumbs { get; set; }

        [Export("maxStringValueLength")]
        nuint MaxStringValueLength { get; set; }

        [Export("persistUser")]
        bool PersistUser { get; set; }

        [Export("user", ArgumentSemantic.Strong)]
        BugsnagUser User { get; }

        [Export("setUser:withEmail:andName:")]
        void SetUser(
            [NullAllowed] string userId,
            [NullAllowed] string email,
            [NullAllowed] string name
        );

        [Export("addOnSessionBlock:")]
        NSObject AddOnSessionBlock(Func<BugsnagSession, bool> block);

        [Export("removeOnSession:")]
        void RemoveOnSession(NSObject callback);

        [Export("addOnSendErrorBlock:")]
        NSObject AddOnSendErrorBlock(Func<BugsnagEvent, bool> block);

        [Export("removeOnSendError:")]
        void RemoveOnSendError(NSObject callback);

        [Export("addOnBreadcrumbBlock:")]
        NSObject AddOnBreadcrumbBlock(Func<BugsnagBreadcrumb, bool> block);

        [Export("removeOnBreadcrumb:")]
        void RemoveOnBreadcrumb(NSObject callback);
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagSession
    {
        [Export("id", ArgumentSemantic.Copy)]
        string Id { get; set; }

        [Export("startedAt", ArgumentSemantic.Strong)]
        NSDate StartedAt { get; set; }

        [Export("app", ArgumentSemantic.Strong)]
        BugsnagApp App { get; }

        [Export("device", ArgumentSemantic.Strong)]
        BugsnagDevice Device { get; }

        [Export("user", ArgumentSemantic.Strong)]
        BugsnagUser User { get; }

        [Export("setUser:withEmail:andName:")]
        void SetUser(
            [NullAllowed] string userId,
            [NullAllowed] string email,
            [NullAllowed] string name
        );
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagClient
    {
        [Export("notify:")]
        void Notify(NSException exception);

        [Export("notify:block:")]
        void Notify(NSException exception, [NullAllowed] BugsnagOnErrorBlock block);

        [Export("notifyError:")]
        void NotifyError(NSError error);

        [Export("notifyError:block:")]
        void NotifyError(NSError error, [NullAllowed] BugsnagOnErrorBlock block);

        [Export("leaveBreadcrumbWithMessage:")]
        void LeaveBreadcrumb(string message);

        [Export("leaveBreadcrumbForNotificationName:")]
        void LeaveBreadcrumbForNotificationName(string notificationName);

        [Export("leaveBreadcrumbWithMessage:metadata:andType:")]
        void LeaveBreadcrumb(
            string message,
            [NullAllowed] NSDictionary metadata,
            BSGBreadcrumbType type
        );

        [Export("leaveNetworkRequestBreadcrumbForTask:metrics:")]
        void LeaveNetworkRequestBreadcrumb(NSUrlSessionTask task, NSUrlSessionTaskMetrics metrics);

        [Export("breadcrumbs")]
        BugsnagBreadcrumb[] Breadcrumbs { get; }

        [Export("startSession")]
        void StartSession();

        [Export("pauseSession")]
        void PauseSession();

        [Export("resumeSession")]
        bool ResumeSession();

        [Export("addOnSessionBlock:")]
        NSObject AddOnSessionBlock(Func<BugsnagSession, bool> block);

        [Export("removeOnSession:")]
        void RemoveOnSession(NSObject callback);

        [Export("context")]
        string Context { get; set; }

        [Export("lastRunInfo")]
        BugsnagLastRunInfo LastRunInfo { get; }

        [Export("markLaunchCompleted")]
        void MarkLaunchCompleted();

        [Export("user")]
        BugsnagUser User { get; }

        [Export("setUser:withEmail:andName:")]
        void SetUser(
            [NullAllowed] string userId,
            [NullAllowed] string email,
            [NullAllowed] string name
        );

        [Export("addOnBreadcrumbBlock:")]
        NSObject AddOnBreadcrumbBlock(Func<BugsnagBreadcrumb, bool> block);

        [Export("removeOnBreadcrumb:")]
        void RemoveOnBreadcrumb(NSObject callback);
    }

    [BaseType(typeof(NSObject))]
    interface BugsnagLastRunInfo
    {
        [Export("consecutiveLaunchCrashes")]
        nuint ConsecutiveLaunchCrashes { get; }

        [Export("crashed")]
        bool Crashed { get; }

        [Export("crashedDuringLaunch")]
        bool CrashedDuringLaunch { get; }
    }
}
