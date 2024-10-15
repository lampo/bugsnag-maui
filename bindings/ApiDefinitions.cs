using System;
using Bugsnag;
using Foundation;
using ObjCRuntime;

// @interface BugsnagApp : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagApp
{
	// @property (copy, nonatomic) NSString * _Nullable binaryArch;
	[NullAllowed, Export ("binaryArch")]
	string BinaryArch { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable bundleVersion;
	[NullAllowed, Export ("bundleVersion")]
	string BundleVersion { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable codeBundleId;
	[NullAllowed, Export ("codeBundleId")]
	string CodeBundleId { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable dsymUuid;
	[NullAllowed, Export ("dsymUuid")]
	string DsymUuid { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable id;
	[NullAllowed, Export ("id")]
	string Id { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable releaseStage;
	[NullAllowed, Export ("releaseStage")]
	string ReleaseStage { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable type;
	[NullAllowed, Export ("type")]
	string Type { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable version;
	[NullAllowed, Export ("version")]
	string Version { get; set; }
}

// @interface BugsnagAppWithState : BugsnagApp
[BaseType (typeof(BugsnagApp))]
interface BugsnagAppWithState
{
	// @property (nonatomic, strong) NSNumber * _Nullable duration;
	[NullAllowed, Export ("duration", ArgumentSemantic.Strong)]
	NSNumber Duration { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable durationInForeground;
	[NullAllowed, Export ("durationInForeground", ArgumentSemantic.Strong)]
	NSNumber DurationInForeground { get; set; }

	// @property (nonatomic) BOOL inForeground;
	[Export ("inForeground")]
	bool InForeground { get; set; }

	// @property (nonatomic) BOOL isLaunching;
	[Export ("isLaunching")]
	bool IsLaunching { get; set; }
}

// @interface BugsnagBreadcrumb : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagBreadcrumb
{
	// @property (readonly, nonatomic) NSDate * _Nullable timestamp;
	[NullAllowed, Export ("timestamp")]
	NSDate Timestamp { get; }

	// @property (readwrite, nonatomic) BSGBreadcrumbType type;
	[Export ("type", ArgumentSemantic.Assign)]
	BSGBreadcrumbType Type { get; set; }

	// @property (readwrite, copy, nonatomic) NSString * _Nonnull message;
	[Export ("message")]
	string Message { get; set; }

	// @property (readwrite, copy, nonatomic) NSDictionary * _Nonnull metadata;
	[Export ("metadata", ArgumentSemantic.Copy)]
	NSDictionary Metadata { get; set; }
}

// @protocol BSGBreadcrumbSink <NSObject>
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
[BaseType (typeof(NSObject))]
interface BSGBreadcrumbSink
{
	// @required -(void)leaveBreadcrumbWithMessage:(NSString * _Nonnull)message metadata:(NSDictionary * _Nullable)metadata andType:(BSGBreadcrumbType)type __attribute__((swift_name("leaveBreadcrumb(_:metadata:type:)")));
	[Abstract]
	[Export ("leaveBreadcrumbWithMessage:metadata:andType:")]
	void Metadata (string message, [NullAllowed] NSDictionary metadata, BSGBreadcrumbType type);
}

// @interface BugsnagFeatureFlag : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagFeatureFlag
{
	// +(instancetype _Nonnull)flagWithName:(NSString * _Nonnull)name;
	[Static]
	[Export ("flagWithName:")]
	BugsnagFeatureFlag FlagWithName (string name);

	// +(instancetype _Nonnull)flagWithName:(NSString * _Nonnull)name variant:(NSString * _Nullable)variant;
	[Static]
	[Export ("flagWithName:variant:")]
	BugsnagFeatureFlag FlagWithName (string name, [NullAllowed] string variant);

	// @property (readonly, nonatomic) NSString * _Nonnull name;
	[Export ("name")]
	string Name { get; }

	// @property (readonly, nonatomic) NSString * _Nullable variant;
	[NullAllowed, Export ("variant")]
	string Variant { get; }
}

// @protocol BugsnagFeatureFlagStore
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
interface BugsnagFeatureFlagStore
{
	// @required -(void)addFeatureFlagWithName:(NSString * _Nonnull)name variant:(NSString * _Nullable)variant __attribute__((swift_name("addFeatureFlag(name:variant:)")));
	[Abstract]
	[Export ("addFeatureFlagWithName:variant:")]
	void AddFeatureFlagWithName (string name, [NullAllowed] string variant);

	// @required -(void)addFeatureFlagWithName:(NSString * _Nonnull)name __attribute__((swift_name("addFeatureFlag(name:)")));
	[Abstract]
	[Export ("addFeatureFlagWithName:")]
	void AddFeatureFlagWithName (string name);

	// @required -(void)addFeatureFlags:(NSArray<BugsnagFeatureFlag *> * _Nonnull)featureFlags __attribute__((swift_name("addFeatureFlags(_:)")));
	[Abstract]
	[Export ("addFeatureFlags:")]
	void AddFeatureFlags (BugsnagFeatureFlag[] featureFlags);

	// @required -(void)clearFeatureFlagWithName:(NSString * _Nonnull)name __attribute__((swift_name("clearFeatureFlag(name:)")));
	[Abstract]
	[Export ("clearFeatureFlagWithName:")]
	void ClearFeatureFlagWithName (string name);

	// @required -(void)clearFeatureFlags;
	[Abstract]
	[Export ("clearFeatureFlags")]
	void ClearFeatureFlags ();
}

// @protocol BugsnagMetadataStore <NSObject>
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
[BaseType (typeof(NSObject))]
interface BugsnagMetadataStore
{
	// @required -(void)addMetadata:(NSDictionary * _Nonnull)metadata toSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("addMetadata(_:section:)")));
	[Abstract]
	[Export ("addMetadata:toSection:")]
	void AddMetadata (NSDictionary metadata, string sectionName);

	// @required -(void)addMetadata:(id _Nullable)metadata withKey:(NSString * _Nonnull)key toSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("addMetadata(_:key:section:)")));
	[Abstract]
	[Export ("addMetadata:withKey:toSection:")]
	void AddMetadata ([NullAllowed] NSObject metadata, string key, string sectionName);

	// @required -(NSMutableDictionary * _Nullable)getMetadataFromSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("getMetadata(section:)")));
	[Abstract]
	[Export ("getMetadataFromSection:")]
	[return: NullAllowed]
	NSMutableDictionary GetMetadataFromSection (string sectionName);

	// @required -(id _Nullable)getMetadataFromSection:(NSString * _Nonnull)sectionName withKey:(NSString * _Nonnull)key __attribute__((swift_name("getMetadata(section:key:)")));
	[Abstract]
	[Export ("getMetadataFromSection:withKey:")]
	[return: NullAllowed]
	NSObject GetMetadataFromSection (string sectionName, string key);

	// @required -(void)clearMetadataFromSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("clearMetadata(section:)")));
	[Abstract]
	[Export ("clearMetadataFromSection:")]
	void ClearMetadataFromSection (string sectionName);

	// @required -(void)clearMetadataFromSection:(NSString * _Nonnull)sectionName withKey:(NSString * _Nonnull)key __attribute__((swift_name("clearMetadata(section:key:)")));
	[Abstract]
	[Export ("clearMetadataFromSection:withKey:")]
	void ClearMetadataFromSection (string sectionName, string key);
}

// @protocol BugsnagClassLevelMetadataStore <NSObject>
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
[BaseType (typeof(NSObject))]
interface BugsnagClassLevelMetadataStore
{
	// @required +(void)addMetadata:(NSDictionary * _Nonnull)metadata toSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("addMetadata(_:section:)")));
	[Static, Abstract]
	[Export ("addMetadata:toSection:")]
	void AddMetadata (NSDictionary metadata, string sectionName);

	// @required +(void)addMetadata:(id _Nullable)metadata withKey:(NSString * _Nonnull)key toSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("addMetadata(_:key:section:)")));
	[Static, Abstract]
	[Export ("addMetadata:withKey:toSection:")]
	void AddMetadata ([NullAllowed] NSObject metadata, string key, string sectionName);

	// @required +(NSMutableDictionary * _Nullable)getMetadataFromSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("getMetadata(section:)")));
	[Static, Abstract]
	[Export ("getMetadataFromSection:")]
	[return: NullAllowed]
	NSMutableDictionary GetMetadataFromSection (string sectionName);

	// @required +(id _Nullable)getMetadataFromSection:(NSString * _Nonnull)sectionName withKey:(NSString * _Nonnull)key __attribute__((swift_name("getMetadata(section:key:)")));
	[Static, Abstract]
	[Export ("getMetadataFromSection:withKey:")]
	[return: NullAllowed]
	NSObject GetMetadataFromSection (string sectionName, string key);

	// @required +(void)clearMetadataFromSection:(NSString * _Nonnull)sectionName __attribute__((swift_name("clearMetadata(section:)")));
	[Static, Abstract]
	[Export ("clearMetadataFromSection:")]
	void ClearMetadataFromSection (string sectionName);

	// @required +(void)clearMetadataFromSection:(NSString * _Nonnull)sectionName withKey:(NSString * _Nonnull)key __attribute__((swift_name("clearMetadata(section:key:)")));
	[Static, Abstract]
	[Export ("clearMetadataFromSection:withKey:")]
	void ClearMetadataFromSection (string sectionName, string key);
}

// @interface BugsnagEvent : NSObject <BugsnagFeatureFlagStore, BugsnagMetadataStore>
[BaseType (typeof(NSObject))]
interface BugsnagEvent : IBugsnagFeatureFlagStore, IBugsnagMetadataStore
{
	// @property (readwrite, copy, nonatomic) NSString * _Nullable context;
	[NullAllowed, Export ("context")]
	string Context { get; set; }

	// @property (readwrite, nonatomic) BSGSeverity severity;
	[Export ("severity", ArgumentSemantic.Assign)]
	BSGSeverity Severity { get; set; }

	// @property (readwrite, copy, nonatomic) NSArray<BugsnagError *> * _Nonnull errors;
	[Export ("errors", ArgumentSemantic.Copy)]
	BugsnagError[] Errors { get; set; }

	// @property (readwrite, copy, nonatomic) NSString * _Nullable groupingHash;
	[NullAllowed, Export ("groupingHash")]
	string GroupingHash { get; set; }

	// @property (readwrite, copy, nonatomic) NSArray<BugsnagBreadcrumb *> * _Nonnull breadcrumbs;
	[Export ("breadcrumbs", ArgumentSemantic.Copy)]
	BugsnagBreadcrumb[] Breadcrumbs { get; set; }

	// @property (readonly, nonatomic, strong) NSArray<BugsnagFeatureFlag *> * _Nonnull featureFlags;
	[Export ("featureFlags", ArgumentSemantic.Strong)]
	BugsnagFeatureFlag[] FeatureFlags { get; }

	// @property (readwrite, copy, nonatomic) NSString * _Nullable apiKey;
	[NullAllowed, Export ("apiKey")]
	string ApiKey { get; set; }

	// @property (readonly, nonatomic) BugsnagDeviceWithState * _Nonnull device;
	[Export ("device")]
	BugsnagDeviceWithState Device { get; }

	// @property (readonly, nonatomic) BugsnagAppWithState * _Nonnull app;
	[Export ("app")]
	BugsnagAppWithState App { get; }

	// @property (readwrite, nonatomic) BOOL unhandled;
	[Export ("unhandled")]
	bool Unhandled { get; set; }

	// @property (readwrite, copy, nonatomic) NSArray<BugsnagThread *> * _Nonnull threads;
	[Export ("threads", ArgumentSemantic.Copy)]
	BugsnagThread[] Threads { get; set; }

	// @property (nonatomic, strong) id _Nullable originalError;
	[NullAllowed, Export ("originalError", ArgumentSemantic.Strong)]
	NSObject OriginalError { get; set; }

	// @property (readonly, nonatomic) BugsnagUser * _Nonnull user;
	[Export ("user")]
	BugsnagUser User { get; }

	// -(void)setUser:(NSString * _Nullable)userId withEmail:(NSString * _Nullable)email andName:(NSString * _Nullable)name;
	[Export ("setUser:withEmail:andName:")]
	void SetUser ([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);

	// -(void)setCorrelationTraceId:(NSString * _Nonnull)traceId spanId:(NSString * _Nonnull)spanId;
	[Export ("setCorrelationTraceId:spanId:")]
	void SetCorrelationTraceId (string traceId, string spanId);
}

// @interface BugsnagMetadata : NSObject <BugsnagMetadataStore>
[BaseType (typeof(NSObject))]
interface BugsnagMetadata : IBugsnagMetadataStore
{
	// -(instancetype _Nonnull)initWithDictionary:(NSDictionary * _Nonnull)dict;
	[Export ("initWithDictionary:")]
	NativeHandle Constructor (NSDictionary dict);

	// -(void)setStorageBuffer:(char * _Nullable * _Nullable)buffer file:(NSString * _Nullable)file;
	[Export ("setStorageBuffer:file:")]
	unsafe void SetStorageBuffer ([NullAllowed] sbyte** buffer, [NullAllowed] string file);

	// -(void)writeData:(NSData * _Nonnull)data toBuffer:(char * _Nullable * _Nonnull)buffer;
	[Export ("writeData:toBuffer:")]
	unsafe void WriteData (NSData data, [NullAllowed] sbyte** buffer);
}

// @protocol BugsnagPlugin <NSObject>
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
[BaseType (typeof(NSObject))]
interface BugsnagPlugin
{
	// @required -(void)load:(BugsnagClient * _Nonnull)client;
	[Abstract]
	[Export ("load:")]
	void Load (BugsnagClient client);

	// @required -(void)unload;
	[Abstract]
	[Export ("unload")]
	void Unload ();
}

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants
{
	// extern const NSUInteger BugsnagAppHangThresholdFatalOnly __attribute__((visibility("default"))) __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Field ("BugsnagAppHangThresholdFatalOnly", "__Internal")]
	nuint BugsnagAppHangThresholdFatalOnly { get; }
}

// typedef BOOL (^BugsnagOnErrorBlock)(BugsnagEvent * _Nonnull);
delegate bool BugsnagOnErrorBlock (BugsnagEvent arg0);

// typedef BOOL (^BugsnagOnSendErrorBlock)(BugsnagEvent * _Nonnull);
delegate bool BugsnagOnSendErrorBlock (BugsnagEvent arg0);

// typedef BOOL (^BugsnagOnBreadcrumbBlock)(BugsnagBreadcrumb * _Nonnull);
delegate bool BugsnagOnBreadcrumbBlock (BugsnagBreadcrumb arg0);

// typedef BOOL (^BugsnagOnSessionBlock)(BugsnagSession * _Nonnull);
delegate bool BugsnagOnSessionBlock (BugsnagSession arg0);

// @interface BugsnagConfiguration : NSObject <BugsnagFeatureFlagStore, BugsnagMetadataStore>
[BaseType (typeof(NSObject))]
[DisableDefaultCtor]
interface BugsnagConfiguration : IBugsnagFeatureFlagStore, IBugsnagMetadataStore
{
	// +(instancetype _Nonnull)loadConfig;
	[Static]
	[Export ("loadConfig")]
	BugsnagConfiguration LoadConfig ();

	// -(instancetype _Nonnull)initWithApiKey:(NSString * _Nullable)apiKey __attribute__((objc_designated_initializer)) __attribute__((swift_name("init(_:)")));
	[Export ("initWithApiKey:")]
	[DesignatedInitializer]
	NativeHandle Constructor ([NullAllowed] string apiKey);

	// @property (copy, nonatomic) NSString * _Nonnull apiKey;
	[Export ("apiKey")]
	string ApiKey { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable releaseStage;
	[NullAllowed, Export ("releaseStage")]
	string ReleaseStage { get; set; }

	// @property (copy, nonatomic) NSSet<NSString *> * _Nullable enabledReleaseStages;
	[NullAllowed, Export ("enabledReleaseStages", ArgumentSemantic.Copy)]
	NSSet<NSString> EnabledReleaseStages { get; set; }

	// @property (copy, nonatomic) NSSet<id> * _Nullable redactedKeys;
	[NullAllowed, Export ("redactedKeys", ArgumentSemantic.Copy)]
	NSSet<NSObject> RedactedKeys { get; set; }

	// @property (copy, nonatomic) NSSet<id> * _Nullable discardClasses;
	[NullAllowed, Export ("discardClasses", ArgumentSemantic.Copy)]
	NSSet<NSObject> DiscardClasses { get; set; }

	// @property (copy, atomic) NSString * _Nullable context;
	[NullAllowed, Export ("context")]
	string Context { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable appVersion;
	[NullAllowed, Export ("appVersion")]
	string AppVersion { get; set; }

	// @property (readwrite, nonatomic, strong) NSURLSession * _Nonnull session;
	[Export ("session", ArgumentSemantic.Strong)]
	NSUrlSession Session { get; set; }

	// @property (nonatomic) BSGThreadSendPolicy sendThreads __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("sendThreads", ArgumentSemantic.Assign)]
	BSGThreadSendPolicy SendThreads { get; set; }

	// @property (nonatomic) void (* _Nullable)(const BSG_KSCrashReportWriter * _Nonnull) onCrashHandler;
	[NullAllowed, Export ("onCrashHandler", ArgumentSemantic.Assign)]
	unsafe Action<Bugsnag.BSG_KSCrashReportWriter*>* OnCrashHandler { get; set; }

	// @property (nonatomic) BOOL autoDetectErrors;
	[Export ("autoDetectErrors")]
	bool AutoDetectErrors { get; set; }

	// @property (nonatomic) NSUInteger appHangThresholdMillis __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("appHangThresholdMillis")]
	nuint AppHangThresholdMillis { get; set; }

	// @property (nonatomic) BOOL reportBackgroundAppHangs __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("reportBackgroundAppHangs")]
	bool ReportBackgroundAppHangs { get; set; }

	// @property (nonatomic) BOOL autoTrackSessions;
	[Export ("autoTrackSessions")]
	bool AutoTrackSessions { get; set; }

	// @property (nonatomic) NSUInteger launchDurationMillis;
	[Export ("launchDurationMillis")]
	nuint LaunchDurationMillis { get; set; }

	// @property (nonatomic) BOOL sendLaunchCrashesSynchronously;
	[Export ("sendLaunchCrashesSynchronously")]
	bool SendLaunchCrashesSynchronously { get; set; }

	// @property (nonatomic) BOOL attemptDeliveryOnCrash;
	[Export ("attemptDeliveryOnCrash")]
	bool AttemptDeliveryOnCrash { get; set; }

	// @property (nonatomic) BSGEnabledBreadcrumbType enabledBreadcrumbTypes;
	[Export ("enabledBreadcrumbTypes", ArgumentSemantic.Assign)]
	BSGEnabledBreadcrumbType EnabledBreadcrumbTypes { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable bundleVersion;
	[NullAllowed, Export ("bundleVersion")]
	string BundleVersion { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable appType;
	[NullAllowed, Export ("appType")]
	string AppType { get; set; }

	// @property (nonatomic) NSUInteger maxPersistedEvents;
	[Export ("maxPersistedEvents")]
	nuint MaxPersistedEvents { get; set; }

	// @property (nonatomic) NSUInteger maxPersistedSessions;
	[Export ("maxPersistedSessions")]
	nuint MaxPersistedSessions { get; set; }

	// @property (nonatomic) NSUInteger maxBreadcrumbs;
	[Export ("maxBreadcrumbs")]
	nuint MaxBreadcrumbs { get; set; }

	// @property (nonatomic) NSUInteger maxStringValueLength;
	[Export ("maxStringValueLength")]
	nuint MaxStringValueLength { get; set; }

	// @property (nonatomic) BOOL persistUser;
	[Export ("persistUser")]
	bool PersistUser { get; set; }

	// @property (nonatomic, strong) BugsnagErrorTypes * _Nonnull enabledErrorTypes;
	[Export ("enabledErrorTypes", ArgumentSemantic.Strong)]
	BugsnagErrorTypes EnabledErrorTypes { get; set; }

	// @property (copy, nonatomic) BugsnagEndpointConfiguration * _Nonnull endpoints;
	[Export ("endpoints", ArgumentSemantic.Copy)]
	BugsnagEndpointConfiguration Endpoints { get; set; }

	// @property (readonly, retain, nonatomic) BugsnagUser * _Nonnull user;
	[Export ("user", ArgumentSemantic.Retain)]
	BugsnagUser User { get; }

	// -(void)setUser:(NSString * _Nullable)userId withEmail:(NSString * _Nullable)email andName:(NSString * _Nullable)name;
	[Export ("setUser:withEmail:andName:")]
	void SetUser ([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);

	// -(BugsnagOnSessionRef _Nonnull)addOnSessionBlock:(BugsnagOnSessionBlock _Nonnull)block __attribute__((swift_name("addOnSession(block:)")));
	[Export ("addOnSessionBlock:")]
	NSObject AddOnSessionBlock (BugsnagOnSessionBlock block);

	// -(void)removeOnSession:(BugsnagOnSessionRef _Nonnull)callback __attribute__((swift_name("removeOnSession(_:)")));
	[Export ("removeOnSession:")]
	void RemoveOnSession (NSObject callback);

	// -(void)removeOnSessionBlock:(BugsnagOnSessionBlock _Nonnull)block __attribute__((deprecated("", "removeOnSession:"))) __attribute__((swift_name("removeOnSession(block:)")));
	[Export ("removeOnSessionBlock:")]
	void RemoveOnSessionBlock (BugsnagOnSessionBlock block);

	// -(BugsnagOnSendErrorRef _Nonnull)addOnSendErrorBlock:(BugsnagOnSendErrorBlock _Nonnull)block __attribute__((swift_name("addOnSendError(block:)")));
	[Export ("addOnSendErrorBlock:")]
	NSObject AddOnSendErrorBlock (BugsnagOnSendErrorBlock block);

	// -(void)removeOnSendError:(BugsnagOnSendErrorRef _Nonnull)callback __attribute__((swift_name("removeOnSendError(_:)")));
	[Export ("removeOnSendError:")]
	void RemoveOnSendError (NSObject callback);

	// -(void)removeOnSendErrorBlock:(BugsnagOnSendErrorBlock _Nonnull)block __attribute__((deprecated("", "removeOnSendError:"))) __attribute__((swift_name("removeOnSendError(block:)")));
	[Export ("removeOnSendErrorBlock:")]
	void RemoveOnSendErrorBlock (BugsnagOnSendErrorBlock block);

	// -(BugsnagOnBreadcrumbRef _Nonnull)addOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock _Nonnull)block __attribute__((swift_name("addOnBreadcrumb(block:)")));
	[Export ("addOnBreadcrumbBlock:")]
	NSObject AddOnBreadcrumbBlock (BugsnagOnBreadcrumbBlock block);

	// -(void)removeOnBreadcrumb:(BugsnagOnBreadcrumbRef _Nonnull)callback __attribute__((swift_name("removeOnBreadcrumb(_:)")));
	[Export ("removeOnBreadcrumb:")]
	void RemoveOnBreadcrumb (NSObject callback);

	// -(void)removeOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock _Nonnull)block __attribute__((deprecated("", "removeOnBreadcrumb:"))) __attribute__((swift_name("removeOnBreadcrumb(block:)")));
	[Export ("removeOnBreadcrumbBlock:")]
	void RemoveOnBreadcrumbBlock (BugsnagOnBreadcrumbBlock block);

	// @property (nonatomic) BSGTelemetryOptions telemetry;
	[Export ("telemetry", ArgumentSemantic.Assign)]
	BSGTelemetryOptions Telemetry { get; set; }

	// -(void)addPlugin:(id<BugsnagPlugin> _Nonnull)plugin;
	[Export ("addPlugin:")]
	void AddPlugin (BugsnagPlugin plugin);
}

// @interface BugsnagLastRunInfo : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagLastRunInfo
{
	// @property (readonly, nonatomic) NSUInteger consecutiveLaunchCrashes;
	[Export ("consecutiveLaunchCrashes")]
	nuint ConsecutiveLaunchCrashes { get; }

	// @property (readonly, nonatomic) BOOL crashed;
	[Export ("crashed")]
	bool Crashed { get; }

	// @property (readonly, nonatomic) BOOL crashedDuringLaunch;
	[Export ("crashedDuringLaunch")]
	bool CrashedDuringLaunch { get; }
}

// @interface BugsnagClient : NSObject <BugsnagFeatureFlagStore, BugsnagMetadataStore>
[BaseType (typeof(NSObject))]
interface BugsnagClient : IBugsnagFeatureFlagStore, IBugsnagMetadataStore
{
	// -(instancetype _Nonnull)initWithConfiguration:(BugsnagConfiguration * _Nonnull)configuration;
	[Export ("initWithConfiguration:")]
	NativeHandle Constructor (BugsnagConfiguration configuration);

	// -(void)notify:(NSException * _Nonnull)exception;
	[Export ("notify:")]
	void Notify (NSException exception);

	// -(void)notify:(NSException * _Nonnull)exception block:(BugsnagOnErrorBlock _Nullable)block;
	[Export ("notify:block:")]
	void Notify (NSException exception, [NullAllowed] BugsnagOnErrorBlock block);

	// -(void)notifyError:(NSError * _Nonnull)error;
	[Export ("notifyError:")]
	void NotifyError (NSError error);

	// -(void)notifyError:(NSError * _Nonnull)error block:(BugsnagOnErrorBlock _Nullable)block;
	[Export ("notifyError:block:")]
	void NotifyError (NSError error, [NullAllowed] BugsnagOnErrorBlock block);

	// -(void)leaveBreadcrumbWithMessage:(NSString * _Nonnull)message;
	[Export ("leaveBreadcrumbWithMessage:")]
	void LeaveBreadcrumbWithMessage (string message);

	// -(void)leaveBreadcrumbForNotificationName:(NSString * _Nonnull)notificationName;
	[Export ("leaveBreadcrumbForNotificationName:")]
	void LeaveBreadcrumbForNotificationName (string notificationName);

	// -(void)leaveBreadcrumbWithMessage:(NSString * _Nonnull)message metadata:(NSDictionary * _Nullable)metadata andType:(BSGBreadcrumbType)type __attribute__((swift_name("leaveBreadcrumb(_:metadata:type:)")));
	[Export ("leaveBreadcrumbWithMessage:metadata:andType:")]
	void LeaveBreadcrumbWithMessage (string message, [NullAllowed] NSDictionary metadata, BSGBreadcrumbType type);

	// -(void)leaveNetworkRequestBreadcrumbForTask:(NSURLSessionTask * _Nonnull)task metrics:(NSURLSessionTaskMetrics * _Nonnull)metrics __attribute__((availability(macos, introduced=10.12))) __attribute__((availability(ios, introduced=10.0))) __attribute__((availability(watchos, introduced=3.0))) __attribute__((availability(tvos, introduced=10.0))) __attribute__((swift_name("leaveNetworkRequestBreadcrumb(task:metrics:)")));
	[Watch (3,0), TV (10,0), Mac (10,12), iOS (10,0)]
	[Export ("leaveNetworkRequestBreadcrumbForTask:metrics:")]
	void LeaveNetworkRequestBreadcrumbForTask (NSUrlSessionTask task, NSUrlSessionTaskMetrics metrics);

	// -(NSArray<BugsnagBreadcrumb *> * _Nonnull)breadcrumbs;
	[Export ("breadcrumbs")]
	[Verify (MethodToProperty)]
	BugsnagBreadcrumb[] Breadcrumbs { get; }

	// -(void)startSession;
	[Export ("startSession")]
	void StartSession ();

	// -(void)pauseSession;
	[Export ("pauseSession")]
	void PauseSession ();

	// -(BOOL)resumeSession;
	[Export ("resumeSession")]
	[Verify (MethodToProperty)]
	bool ResumeSession { get; }

	// -(BugsnagOnSessionRef _Nonnull)addOnSessionBlock:(BugsnagOnSessionBlock _Nonnull)block __attribute__((swift_name("addOnSession(block:)")));
	[Export ("addOnSessionBlock:")]
	NSObject AddOnSessionBlock (BugsnagOnSessionBlock block);

	// -(void)removeOnSession:(BugsnagOnSessionRef _Nonnull)callback __attribute__((swift_name("removeOnSession(_:)")));
	[Export ("removeOnSession:")]
	void RemoveOnSession (NSObject callback);

	// -(void)removeOnSessionBlock:(BugsnagOnSessionBlock _Nonnull)block __attribute__((deprecated("", "removeOnSession:"))) __attribute__((swift_name("removeOnSession(block:)")));
	[Export ("removeOnSessionBlock:")]
	void RemoveOnSessionBlock (BugsnagOnSessionBlock block);

	// @property (copy, atomic) NSString * _Nullable context;
	[NullAllowed, Export ("context")]
	string Context { get; set; }

	// -(BOOL)appDidCrashLastLaunch __attribute__((deprecated("", "lastRunInfo.crashed")));
	[Export ("appDidCrashLastLaunch")]
	[Verify (MethodToProperty)]
	bool AppDidCrashLastLaunch { get; }

	// @property (readonly, nonatomic) BugsnagLastRunInfo * _Nullable lastRunInfo;
	[NullAllowed, Export ("lastRunInfo")]
	BugsnagLastRunInfo LastRunInfo { get; }

	// -(void)markLaunchCompleted;
	[Export ("markLaunchCompleted")]
	void MarkLaunchCompleted ();

	// -(BugsnagUser * _Nonnull)user;
	[Export ("user")]
	[Verify (MethodToProperty)]
	BugsnagUser User { get; }

	// -(void)setUser:(NSString * _Nullable)userId withEmail:(NSString * _Nullable)email andName:(NSString * _Nullable)name;
	[Export ("setUser:withEmail:andName:")]
	void SetUser ([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);

	// -(BugsnagOnBreadcrumbRef _Nonnull)addOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock _Nonnull)block __attribute__((swift_name("addOnBreadcrumb(block:)")));
	[Export ("addOnBreadcrumbBlock:")]
	NSObject AddOnBreadcrumbBlock (BugsnagOnBreadcrumbBlock block);

	// -(void)removeOnBreadcrumb:(BugsnagOnBreadcrumbRef _Nonnull)callback __attribute__((swift_name("removeOnBreadcrumb(_:)")));
	[Export ("removeOnBreadcrumb:")]
	void RemoveOnBreadcrumb (NSObject callback);

	// -(void)removeOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock _Nonnull)block __attribute__((deprecated("", "removeOnBreadcrumb:"))) __attribute__((swift_name("removeOnBreadcrumb(block:)")));
	[Export ("removeOnBreadcrumbBlock:")]
	void RemoveOnBreadcrumbBlock (BugsnagOnBreadcrumbBlock block);
}

// @interface BugsnagDevice : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagDevice
{
	// @property (nonatomic) BOOL jailbroken;
	[Export ("jailbroken")]
	bool Jailbroken { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable id;
	[NullAllowed, Export ("id")]
	string Id { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable locale;
	[NullAllowed, Export ("locale")]
	string Locale { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable manufacturer;
	[NullAllowed, Export ("manufacturer")]
	string Manufacturer { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable model;
	[NullAllowed, Export ("model")]
	string Model { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable modelNumber;
	[NullAllowed, Export ("modelNumber")]
	string ModelNumber { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable osName;
	[NullAllowed, Export ("osName")]
	string OsName { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable osVersion;
	[NullAllowed, Export ("osVersion")]
	string OsVersion { get; set; }

	// @property (copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable runtimeVersions;
	[NullAllowed, Export ("runtimeVersions", ArgumentSemantic.Copy)]
	NSDictionary<NSString, NSString> RuntimeVersions { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable totalMemory;
	[NullAllowed, Export ("totalMemory", ArgumentSemantic.Strong)]
	NSNumber TotalMemory { get; set; }
}

// @interface BugsnagDeviceWithState : BugsnagDevice
[BaseType (typeof(BugsnagDevice))]
interface BugsnagDeviceWithState
{
	// @property (nonatomic, strong) NSNumber * _Nullable freeDisk;
	[NullAllowed, Export ("freeDisk", ArgumentSemantic.Strong)]
	NSNumber FreeDisk { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable freeMemory;
	[NullAllowed, Export ("freeMemory", ArgumentSemantic.Strong)]
	NSNumber FreeMemory { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable orientation;
	[NullAllowed, Export ("orientation")]
	string Orientation { get; set; }

	// @property (nonatomic, strong) NSDate * _Nullable time;
	[NullAllowed, Export ("time", ArgumentSemantic.Strong)]
	NSDate Time { get; set; }
}

// @interface BugsnagEndpointConfiguration : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagEndpointConfiguration
{
	// @property (copy, nonatomic) NSString * _Nonnull notify;
	[Export ("notify")]
	string Notify { get; set; }

	// @property (copy, nonatomic) NSString * _Nonnull sessions;
	[Export ("sessions")]
	string Sessions { get; set; }

	// -(instancetype _Nonnull)initWithNotify:(NSString * _Nonnull)notify sessions:(NSString * _Nonnull)sessions;
	[Export ("initWithNotify:sessions:")]
	NativeHandle Constructor (string notify, string sessions);
}

// @interface BugsnagError : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagError
{
	// @property (copy, nonatomic) NSString * _Nullable errorClass;
	[NullAllowed, Export ("errorClass")]
	string ErrorClass { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable errorMessage;
	[NullAllowed, Export ("errorMessage")]
	string ErrorMessage { get; set; }

	// @property (copy, nonatomic) NSArray<BugsnagStackframe *> * _Nonnull stacktrace;
	[Export ("stacktrace", ArgumentSemantic.Copy)]
	BugsnagStackframe[] Stacktrace { get; set; }

	// @property (nonatomic) BSGErrorType type;
	[Export ("type", ArgumentSemantic.Assign)]
	BSGErrorType Type { get; set; }
}

// @interface BugsnagErrorTypes : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagErrorTypes
{
	// @property (nonatomic) BOOL appHangs __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("appHangs")]
	bool AppHangs { get; set; }

	// @property (nonatomic) BOOL ooms __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("ooms")]
	bool Ooms { get; set; }

	// @property (nonatomic) BOOL thermalKills __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("thermalKills")]
	bool ThermalKills { get; set; }

	// @property (nonatomic) BOOL unhandledExceptions;
	[Export ("unhandledExceptions")]
	bool UnhandledExceptions { get; set; }

	// @property (nonatomic) BOOL signals __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("signals")]
	bool Signals { get; set; }

	// @property (nonatomic) BOOL cppExceptions;
	[Export ("cppExceptions")]
	bool CppExceptions { get; set; }

	// @property (nonatomic) BOOL machExceptions __attribute__((availability(watchos, unavailable)));
	[NoWatch]
	[Export ("machExceptions")]
	bool MachExceptions { get; set; }

	// @property (nonatomic) BOOL unhandledRejections;
	[Export ("unhandledRejections")]
	bool UnhandledRejections { get; set; }
}

// @interface BugsnagUser : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagUser
{
	// @property (readonly, nonatomic) NSString * _Nullable id;
	[NullAllowed, Export ("id")]
	string Id { get; }

	// @property (readonly, nonatomic) NSString * _Nullable name;
	[NullAllowed, Export ("name")]
	string Name { get; }

	// @property (readonly, nonatomic) NSString * _Nullable email;
	[NullAllowed, Export ("email")]
	string Email { get; }
}

// @interface BugsnagSession : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagSession
{
	// @property (copy, nonatomic) NSString * _Nonnull id;
	[Export ("id")]
	string Id { get; set; }

	// @property (nonatomic, strong) NSDate * _Nonnull startedAt;
	[Export ("startedAt", ArgumentSemantic.Strong)]
	NSDate StartedAt { get; set; }

	// @property (readonly, nonatomic) BugsnagApp * _Nonnull app;
	[Export ("app")]
	BugsnagApp App { get; }

	// @property (readonly, nonatomic) BugsnagDevice * _Nonnull device;
	[Export ("device")]
	BugsnagDevice Device { get; }

	// @property (readonly, nonatomic) BugsnagUser * _Nonnull user;
	[Export ("user")]
	BugsnagUser User { get; }

	// -(void)setUser:(NSString * _Nullable)userId withEmail:(NSString * _Nullable)email andName:(NSString * _Nullable)name;
	[Export ("setUser:withEmail:andName:")]
	void SetUser ([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);
}

[Static]
[Verify (ConstantsInterfaceAssociation)]
partial interface Constants
{
	// extern const BugsnagStackframeType _Nonnull BugsnagStackframeTypeCocoa __attribute__((visibility("default")));
	[Field ("BugsnagStackframeTypeCocoa", "__Internal")]
	NSString BugsnagStackframeTypeCocoa { get; }
}

// @interface BugsnagStackframe : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagStackframe
{
	// @property (copy, nonatomic) NSString * _Nullable method;
	[NullAllowed, Export ("method")]
	string Method { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable machoFile;
	[NullAllowed, Export ("machoFile")]
	string MachoFile { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable machoUuid;
	[NullAllowed, Export ("machoUuid")]
	string MachoUuid { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable frameAddress;
	[NullAllowed, Export ("frameAddress", ArgumentSemantic.Strong)]
	NSNumber FrameAddress { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable machoVmAddress;
	[NullAllowed, Export ("machoVmAddress", ArgumentSemantic.Strong)]
	NSNumber MachoVmAddress { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable symbolAddress;
	[NullAllowed, Export ("symbolAddress", ArgumentSemantic.Strong)]
	NSNumber SymbolAddress { get; set; }

	// @property (nonatomic, strong) NSNumber * _Nullable machoLoadAddress;
	[NullAllowed, Export ("machoLoadAddress", ArgumentSemantic.Strong)]
	NSNumber MachoLoadAddress { get; set; }

	// @property (nonatomic) BOOL isPc;
	[Export ("isPc")]
	bool IsPc { get; set; }

	// @property (nonatomic) BOOL isLr;
	[Export ("isLr")]
	bool IsLr { get; set; }

	// @property (copy, nonatomic) BugsnagStackframeType _Nullable type;
	[NullAllowed, Export ("type")]
	string Type { get; set; }

	// +(NSArray<BugsnagStackframe *> * _Nonnull)stackframesWithCallStackReturnAddresses:(NSArray<NSNumber *> * _Nonnull)callStackReturnAddresses;
	[Static]
	[Export ("stackframesWithCallStackReturnAddresses:")]
	BugsnagStackframe[] StackframesWithCallStackReturnAddresses (NSNumber[] callStackReturnAddresses);

	// +(NSArray<BugsnagStackframe *> * _Nullable)stackframesWithCallStackSymbols:(NSArray<NSString *> * _Nonnull)callStackSymbols;
	[Static]
	[Export ("stackframesWithCallStackSymbols:")]
	[return: NullAllowed]
	BugsnagStackframe[] StackframesWithCallStackSymbols (string[] callStackSymbols);
}

// @interface BugsnagThread : NSObject
[BaseType (typeof(NSObject))]
interface BugsnagThread
{
	// @property (copy, nonatomic) NSString * _Nullable id;
	[NullAllowed, Export ("id")]
	string Id { get; set; }

	// @property (copy, nonatomic) NSString * _Nullable name;
	[NullAllowed, Export ("name")]
	string Name { get; set; }

	// @property (readonly, nonatomic) BOOL errorReportingThread;
	[Export ("errorReportingThread")]
	bool ErrorReportingThread { get; }

	// @property (copy, nonatomic) NSString * _Nullable state;
	[NullAllowed, Export ("state")]
	string State { get; set; }

	// @property (copy, nonatomic) NSArray<BugsnagStackframe *> * _Nonnull stacktrace;
	[Export ("stacktrace", ArgumentSemantic.Copy)]
	BugsnagStackframe[] Stacktrace { get; set; }

	// @property (nonatomic) BSGThreadType type;
	[Export ("type", ArgumentSemantic.Assign)]
	BSGThreadType Type { get; set; }
}

// @interface Bugsnag : NSObject <BugsnagClassLevelMetadataStore>
[BaseType (typeof(NSObject))]
[DisableDefaultCtor]
interface Bugsnag : IBugsnagClassLevelMetadataStore
{
	// +(BugsnagClient * _Nonnull)start;
	[Static]
	[Export ("start")]
	[Verify (MethodToProperty)]
	BugsnagClient Start { get; }

	// +(BugsnagClient * _Nonnull)startWithApiKey:(NSString * _Nonnull)apiKey;
	[Static]
	[Export ("startWithApiKey:")]
	BugsnagClient StartWithApiKey (string apiKey);

	// +(BugsnagClient * _Nonnull)startWithConfiguration:(BugsnagConfiguration * _Nonnull)configuration;
	[Static]
	[Export ("startWithConfiguration:")]
	BugsnagClient StartWithConfiguration (BugsnagConfiguration configuration);

	// +(BOOL)appDidCrashLastLaunch __attribute__((deprecated("", "lastRunInfo.crashed")));
	[Static]
	[Export ("appDidCrashLastLaunch")]
	[Verify (MethodToProperty)]
	bool AppDidCrashLastLaunch { get; }

	// +(BOOL)isStarted;
	[Static]
	[Export ("isStarted")]
	[Verify (MethodToProperty)]
	bool IsStarted { get; }

	// @property (readonly, nonatomic, class) BugsnagLastRunInfo * _Nullable lastRunInfo;
	[Static]
	[NullAllowed, Export ("lastRunInfo")]
	BugsnagLastRunInfo LastRunInfo { get; }

	// +(void)markLaunchCompleted;
	[Static]
	[Export ("markLaunchCompleted")]
	void MarkLaunchCompleted ();

	// +(void)notify:(NSException * _Nonnull)exception;
	[Static]
	[Export ("notify:")]
	void Notify (NSException exception);

	// +(void)notify:(NSException * _Nonnull)exception block:(BugsnagOnErrorBlock _Nullable)block;
	[Static]
	[Export ("notify:block:")]
	void Notify (NSException exception, [NullAllowed] BugsnagOnErrorBlock block);

	// +(void)notifyError:(NSError * _Nonnull)error;
	[Static]
	[Export ("notifyError:")]
	void NotifyError (NSError error);

	// +(void)notifyError:(NSError * _Nonnull)error block:(BugsnagOnErrorBlock _Nullable)block;
	[Static]
	[Export ("notifyError:block:")]
	void NotifyError (NSError error, [NullAllowed] BugsnagOnErrorBlock block);

	// +(void)leaveBreadcrumbWithMessage:(NSString * _Nonnull)message;
	[Static]
	[Export ("leaveBreadcrumbWithMessage:")]
	void LeaveBreadcrumbWithMessage (string message);

	// +(void)leaveBreadcrumbForNotificationName:(NSString * _Nonnull)notificationName;
	[Static]
	[Export ("leaveBreadcrumbForNotificationName:")]
	void LeaveBreadcrumbForNotificationName (string notificationName);

	// +(void)leaveBreadcrumbWithMessage:(NSString * _Nonnull)message metadata:(NSDictionary * _Nullable)metadata andType:(BSGBreadcrumbType)type __attribute__((swift_name("leaveBreadcrumb(_:metadata:type:)")));
	[Static]
	[Export ("leaveBreadcrumbWithMessage:metadata:andType:")]
	void LeaveBreadcrumbWithMessage (string message, [NullAllowed] NSDictionary metadata, BSGBreadcrumbType type);

	// +(void)leaveNetworkRequestBreadcrumbForTask:(NSURLSessionTask * _Nonnull)task metrics:(NSURLSessionTaskMetrics * _Nonnull)metrics __attribute__((availability(macos, introduced=10.12))) __attribute__((availability(ios, introduced=10.0))) __attribute__((availability(watchos, introduced=3.0))) __attribute__((availability(tvos, introduced=10.0))) __attribute__((swift_name("leaveNetworkRequestBreadcrumb(task:metrics:)")));
	[Watch (3,0), TV (10,0), Mac (10,12), iOS (10,0)]
	[Static]
	[Export ("leaveNetworkRequestBreadcrumbForTask:metrics:")]
	void LeaveNetworkRequestBreadcrumbForTask (NSUrlSessionTask task, NSUrlSessionTaskMetrics metrics);

	// +(NSArray<BugsnagBreadcrumb *> * _Nonnull)breadcrumbs;
	[Static]
	[Export ("breadcrumbs")]
	[Verify (MethodToProperty)]
	BugsnagBreadcrumb[] Breadcrumbs { get; }

	// +(void)startSession;
	[Static]
	[Export ("startSession")]
	void StartSession ();

	// +(void)pauseSession;
	[Static]
	[Export ("pauseSession")]
	void PauseSession ();

	// +(BOOL)resumeSession;
	[Static]
	[Export ("resumeSession")]
	[Verify (MethodToProperty)]
	bool ResumeSession { get; }

	// +(NSString * _Nullable)context;
	// +(void)setContext:(NSString * _Nullable)context;
	[Static]
	[NullAllowed, Export ("context")]
	[Verify (MethodToProperty)]
	string Context { get; set; }

	// +(BugsnagUser * _Nonnull)user;
	[Static]
	[Export ("user")]
	[Verify (MethodToProperty)]
	BugsnagUser User { get; }

	// +(void)setUser:(NSString * _Nullable)userId withEmail:(NSString * _Nullable)email andName:(NSString * _Nullable)name;
	[Static]
	[Export ("setUser:withEmail:andName:")]
	void SetUser ([NullAllowed] string userId, [NullAllowed] string email, [NullAllowed] string name);

	// +(void)addFeatureFlagWithName:(NSString * _Nonnull)name variant:(NSString * _Nullable)variant __attribute__((swift_name("addFeatureFlag(name:variant:)")));
	[Static]
	[Export ("addFeatureFlagWithName:variant:")]
	void AddFeatureFlagWithName (string name, [NullAllowed] string variant);

	// +(void)addFeatureFlagWithName:(NSString * _Nonnull)name __attribute__((swift_name("addFeatureFlag(name:)")));
	[Static]
	[Export ("addFeatureFlagWithName:")]
	void AddFeatureFlagWithName (string name);

	// +(void)addFeatureFlags:(NSArray<BugsnagFeatureFlag *> * _Nonnull)featureFlags __attribute__((swift_name("addFeatureFlags(_:)")));
	[Static]
	[Export ("addFeatureFlags:")]
	void AddFeatureFlags (BugsnagFeatureFlag[] featureFlags);

	// +(void)clearFeatureFlagWithName:(NSString * _Nonnull)name __attribute__((swift_name("clearFeatureFlag(name:)")));
	[Static]
	[Export ("clearFeatureFlagWithName:")]
	void ClearFeatureFlagWithName (string name);

	// +(void)clearFeatureFlags;
	[Static]
	[Export ("clearFeatureFlags")]
	void ClearFeatureFlags ();

	// +(BugsnagOnSessionRef _Nonnull)addOnSessionBlock:(BugsnagOnSessionBlock _Nonnull)block __attribute__((swift_name("addOnSession(block:)")));
	[Static]
	[Export ("addOnSessionBlock:")]
	NSObject AddOnSessionBlock (BugsnagOnSessionBlock block);

	// +(void)removeOnSession:(BugsnagOnSessionRef _Nonnull)callback __attribute__((swift_name("removeOnSession(_:)")));
	[Static]
	[Export ("removeOnSession:")]
	void RemoveOnSession (NSObject callback);

	// +(void)removeOnSessionBlock:(BugsnagOnSessionBlock _Nonnull)block __attribute__((deprecated("", "removeOnSession:"))) __attribute__((swift_name("removeOnSession(block:)")));
	[Static]
	[Export ("removeOnSessionBlock:")]
	void RemoveOnSessionBlock (BugsnagOnSessionBlock block);

	// +(BugsnagOnBreadcrumbRef _Nonnull)addOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock _Nonnull)block __attribute__((swift_name("addOnBreadcrumb(block:)")));
	[Static]
	[Export ("addOnBreadcrumbBlock:")]
	NSObject AddOnBreadcrumbBlock (BugsnagOnBreadcrumbBlock block);

	// +(void)removeOnBreadcrumb:(BugsnagOnBreadcrumbRef _Nonnull)callback __attribute__((swift_name("removeOnBreadcrumb(_:)")));
	[Static]
	[Export ("removeOnBreadcrumb:")]
	void RemoveOnBreadcrumb (NSObject callback);

	// +(void)removeOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock _Nonnull)block __attribute__((deprecated("", "removeOnBreadcrumb:"))) __attribute__((swift_name("removeOnBreadcrumb(block:)")));
	[Static]
	[Export ("removeOnBreadcrumbBlock:")]
	void RemoveOnBreadcrumbBlock (BugsnagOnBreadcrumbBlock block);
}
