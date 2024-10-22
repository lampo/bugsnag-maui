using System;
using Foundation;
using ObjCRuntime;

namespace Bugsnag.iOS
{
    [BaseType(typeof(NSObject))]
interface BugsnagBinding
{
    [Static]
    [Export("startBugsnagWithConfiguration:")]
    void StartBugsnagWithConfiguration(BugsnagConfiguration configuration, string notifierVersion);

    [Static]
    [Export("clearMetadata:")]
    void ClearMetadata(string section);

    [Static]
    [Export("clearMetadataWithKey:section:")]
    void ClearMetadataWithKey(string section, string key);

    [Static]
    [Export("getDictionaryFromMetadataJson:")]
    NSDictionary GetDictionaryFromMetadataJson(string jsonString);

    [Static]
    [Export("getEventMetaData:tab:")]
    string GetEventMetaData(IntPtr eventPtr, string tab);

    [Static]
    [Export("clearEventMetadataWithKey:section:key:")]
    void ClearEventMetadataWithKey(IntPtr eventPtr, string section, string key);

    [Static]
    [Export("clearEventMetadataSection:section:")]
    void ClearEventMetadataSection(IntPtr eventPtr, string section);

    [Static]
    [Export("setEventMetadata:tab:metadataJson:")]
    void SetEventMetadata(IntPtr eventPtr, string tab, string metadataJson);

    [Static]
    [Export("getUserFromSession:")]
    BugsnagUser GetUserFromSession(IntPtr session);

    [Static]
    [Export("setUserFromSession:userId:userEmail:userName:")]
    void SetUserFromSession(IntPtr session, string userId, string userEmail, string userName);

    [Static]
    [Export("getUserFromEvent:")]
    BugsnagUser GetUserFromEvent(IntPtr eventPtr);

    [Static]
    [Export("setUserFromEvent:userId:userEmail:userName:")]
    void SetUserFromEvent(IntPtr eventPtr, string userId, string userEmail, string userName);
    
    [Static]
    [Export("setEventSeverity:event:severity:")]
    void SetEventSeverity(IntPtr eventPtr, string severity);

    [Static]
    [Export("getSeverityFromEvent:")]
    string GetSeverityFromEvent(IntPtr eventPtr);

    [Static]
    [Export("getFeatureFlagsFromEvent:")]
    string GetFeatureFlagsFromEvent(BugsnagEvent eventPtr);

    [Static]
    [Export("getBreadcrumbMetadata:")]
    string GetBreadcrumbMetadata(IntPtr breadcrumb);

    [Static]
    [Export("setBreadcrumbMetadata:metadataJson:")]
    void SetBreadcrumbMetadata(IntPtr breadcrumb, string metadataJson);

    [Static]
    [Export("getBreadcrumbType:")]
    string GetBreadcrumbType(IntPtr breadcrumb);

    [Static]
    [Export("setBreadcrumbType:type:")]
    void SetBreadcrumbType(IntPtr breadcrumb, string type);

    [Static]
    [Export("getValueAsString:object:key:")]
    string GetValueAsString(IntPtr obj, string key);

    [Static]
    [Export("setNumberValue:object:key:value:")]
    void SetNumberValue(IntPtr obj, string key, string value);

    [Static]
    [Export("getTimestampFromDateInObject:object:key:")]
    double GetTimestampFromDateInObject(IntPtr obj, string key);

    [Static]
    [Export("setTimestampFromDateInObject:object:key:timeStamp:")]
    void SetTimestampFromDateInObject(IntPtr obj, string key, double timeStamp);

    [Static]
    [Export("setRuntimeVersionsFromDevice:device:versions:count:")]
    void SetRuntimeVersionsFromDevice(IntPtr device, string[] versions, int count);

    [Static]
    [Export("getRuntimeVersionsFromDevice:")]
    string GetRuntimeVersionsFromDevice(IntPtr device);

    [Static]
    [Export("setBoolValue:object:key:value:")]
    void SetBoolValue(IntPtr obj, string key, string value);

    [Static]
    [Export("setStringValue:object:key:value:")]
    void SetStringValue(IntPtr obj, string key, string value);

    [Static]
    [Export("getErrorTypeFromError:error:")]
    string GetErrorTypeFromError(IntPtr error);

    [Static]
    [Export("getThreadTypeFromThread:thread:")]
    string GetThreadTypeFromThread(IntPtr thread);

    [Static]
    [Export("getAppFromSession:session:")]
    BugsnagApp GetAppFromSession(IntPtr session);

    [Static]
    [Export("getAppFromEvent:event:")]
    BugsnagAppWithState GetAppFromEvent(IntPtr eventPtr);

    [Static]
    [Export("getDeviceFromSession:session:")]
    BugsnagDevice GetDeviceFromSession(IntPtr session);

    [Static]
    [Export("getDeviceFromEvent:event:")]
    BugsnagDeviceWithState GetDeviceFromEvent(IntPtr eventPtr);

    [Static]
    [Export("registerForSessionCallbacks:configuration:callback:")]
    void RegisterForSessionCallbacks(IntPtr configuration, Func<IntPtr, bool> callback);

    [Static]
    [Export("registerForOnSendCallbacks:configuration:callback:")]
    void RegisterForOnSendCallbacks(IntPtr configuration, Func<IntPtr, bool> callback);

    [Static]
    [Export("registerSession:sessionId:startedAt:unhandledCount:handledCount:")]
    void RegisterSession(string sessionId, long startedAt, int unhandledCount, int handledCount);

    [Static]
    [Export("retrieveCurrentSession:ptr:callback:")]
    void RetrieveCurrentSession(IntPtr ptr, Action<IntPtr, string, string, int, int> callback);

    [Static]
    [Export("markLaunchCompleted")]
    void MarkLaunchCompleted();

    [Static]
    [Export("registerForSessionCallbacksAfterStart:callback:")]
    void RegisterForSessionCallbacksAfterStart(Func<IntPtr, bool> callback);

    [Static]
    [Export("createConfiguration:apiKey:")]
    IntPtr CreateConfiguration(string apiKey);

    [Static]
    [Export("setReleaseStage:configuration:releaseStage:")]
    void SetReleaseStage(IntPtr configuration, string releaseStage);

    [Static]
    [Export("addFeatureFlagOnConfig:configuration:name:variant:")]
    void AddFeatureFlagOnConfig(IntPtr configuration, string name, string variant);

    [Static]
    [Export("addFeatureFlag:name:variant:")]
    void AddFeatureFlag(string name, string variant);

    [Static]
    [Export("clearFeatureFlag:name:")]
    void ClearFeatureFlag(string name);

    [Static]
    [Export("clearFeatureFlags")]
    void ClearFeatureFlags();

    [Static]
    [Export("addFeatureFlagOnEvent:event:name:variant:")]
    void AddFeatureFlagOnEvent(IntPtr eventPtr, string name, string variant);

    [Static]
    [Export("clearFeatureFlagOnEvent:event:name:")]
    void ClearFeatureFlagOnEvent(IntPtr eventPtr, string name);

    [Static]
    [Export("clearFeatureFlagsOnEvent:event:")]
    void ClearFeatureFlagsOnEvent(IntPtr eventPtr);

    [Static]
    [Export("setNotifyReleaseStages:configuration:releaseStages:releaseStagesCount:")]
    void SetNotifyReleaseStages(IntPtr configuration, string[] releaseStages, int releaseStagesCount);

    [Static]
    [Export("setAppVersion:configuration:appVersion:")]
    void SetAppVersion(IntPtr configuration, string appVersion);

    [Static]
    [Export("setAppHangThresholdMillis:configuration:appHangThresholdMillis:")]
    void SetAppHangThresholdMillis(IntPtr configuration, nuint appHangThresholdMillis);

    [Static]
    [Export("setLaunchDurationMillis:configuration:launchDurationMillis:")]
    void SetLaunchDurationMillis(IntPtr configuration, nuint launchDurationMillis);

    [Static]
    [Export("setBundleVersion:configuration:bundleVersion:")]
    void SetBundleVersion(IntPtr configuration, string bundleVersion);

    [Static]
    [Export("setAppType:configuration:appType:")]
    void SetAppType(IntPtr configuration, string appType);

    [Static]
    [Export("setContext:configuration:context:")]
    void SetContext(IntPtr configuration, string context);

    [Static]
    [Export("setContextConfig:configuration:context:")]
    void SetContextConfig(IntPtr configuration, string context);

    [Static]
    [Export("setMaxBreadcrumbs:configuration:maxBreadcrumbs:")]
    void SetMaxBreadcrumbs(IntPtr configuration, int maxBreadcrumbs);

    [Static]
    [Export("setMaxStringValueLength:configuration:maxStringValueLength:")]
    void SetMaxStringValueLength(IntPtr configuration, int maxStringValueLength);

    [Static]
    [Export("setMaxPersistedEvents:configuration:maxPersistedEvents:")]
    void SetMaxPersistedEvents(IntPtr configuration, int maxPersistedEvents);

    [Static]
    [Export("setMaxPersistedSessions:configuration:maxPersistedSessions:")]
    void SetMaxPersistedSessions(IntPtr configuration, int maxPersistedSessions);

    [Static]
    [Export("setEnabledBreadcrumbTypes:configuration:types:count:")]
    void SetEnabledBreadcrumbTypes(IntPtr configuration, string[] types, int count);

    [Static]
    [Export("setEnabledTelemetryTypes:configuration:types:count:")]
    void SetEnabledTelemetryTypes(IntPtr configuration, string[] types, int count);

    [Static]
    [Export("setThreadSendPolicy:configuration:threadSendPolicy:")]
    void SetThreadSendPolicy(IntPtr configuration, string threadSendPolicy);

    [Static]
    [Export("setEnabledErrorTypes:configuration:types:count:")]
    void SetEnabledErrorTypes(IntPtr configuration, string[] types, int count);

    [Static]
    [Export("setDiscardClasses:configuration:classNames:count:")]
    void SetDiscardClasses(IntPtr configuration, string[] classNames, int count);

    [Static]
    [Export("setUserInConfig:configuration:userId:userEmail:userName:")]
    void SetUserInConfig(IntPtr configuration, string userId, string userEmail, string userName);

    [Static]
    [Export("setRedactedKeys:configuration:redactedKeys:count:")]
    void SetRedactedKeys(IntPtr configuration, string[] redactedKeys, int count);

    [Static]
    [Export("setAutoNotifyConfig:configuration:autoNotify:")]
    void SetAutoNotifyConfig(IntPtr configuration, bool autoNotify);

    [Static]
    [Export("setAutoTrackSessions:configuration:autoTrackSessions:")]
    void SetAutoTrackSessions(IntPtr configuration, bool autoTrackSessions);

    [Static]
    [Export("setPersistUser:configuration:persistUser:")]
    void SetPersistUser(IntPtr configuration, bool persistUser);

    [Static]
    [Export("setSendLaunchCrashesSynchronously:configuration:sendLaunchCrashesSynchronously:")]
    void SetSendLaunchCrashesSynchronously(IntPtr configuration, bool sendLaunchCrashesSynchronously);

    [Static]
    [Export("setEndpoints:configuration:notifyURL:sessionsURL:")]
    void SetEndpoints(IntPtr configuration, string notifyURL, string sessionsURL);

    [Static]
    [Export("setMetadata:section:jsonString:")]
    void SetMetadata(string section, string jsonString);

    [Static]
    [Export("retrieveMetaData")]
    string RetrieveMetaData();

    [Static]
    [Export("removeMetadata:configuration:tab:")]
    void RemoveMetadata(IntPtr configuration, string tab);

    [Static]
    [Export("addBreadcrumb:message:type:metadataJson:")]
    void AddBreadcrumb(string message, string type, string metadataJson);

    [Static]
    [Export("retrieveBreadcrumbs:managedBreadcrumbs:breadcrumb:")]
    void RetrieveBreadcrumbs(IntPtr managedBreadcrumbs, Action<IntPtr, string, string, string, string> breadcrumb);

    [Static]
    [Export("retrieveAppData")]
    string RetrieveAppData();

    [Static]
    [Export("retrieveLastRunInfo:lastRuninfo:callback:")]
    void RetrieveLastRunInfo(IntPtr lastRuninfo, Action<IntPtr, bool, bool, int> callback);

    [Static]
    [Export("retrieveDeviceData:deviceData:callback:")]
    void RetrieveDeviceData(IntPtr deviceData, Action<IntPtr, string, string> callback);

    [Static]
    [Export("populateUser:user:")]
    void PopulateUser(ref BugsnagUser user);

    [Static]
    [Export("setUser:userId:userEmail:userName:")]
    void SetUser(string userId, string userEmail, string userName);

    [Static]
    [Export("startSession")]
    void StartSession();

    [Static]
    [Export("pauseSession")]
    void PauseSession();

    [Static]
    [Export("resumeSession")]
    bool ResumeSession();
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
        void SetUser([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);

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
    void LeaveBreadcrumb(string message, [NullAllowed] NSDictionary metadata, BSGBreadcrumbType type);
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
    BugsnagStackframe[] StackframesWithCallStackReturnAddresses(NSNumber[] callStackReturnAddresses);

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
    void SetUser([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);

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
    void SetUser([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);
}
}