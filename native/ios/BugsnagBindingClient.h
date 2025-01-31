#import <Foundation/Foundation.h>
#import <Bugsnag/Bugsnag.h>

@interface BugsnagBindingClient : NSObject

/**
 * All Bugsnag access is class-level.  Prevent the creation of instances.
 */
- (instancetype _Nonnull )init NS_UNAVAILABLE NS_SWIFT_UNAVAILABLE("Use class methods to initialise Bugsnag.");

/**
 * Start listening for crashes.
 *
 * This method initializes Bugsnag with the configuration set in your Info.plist.
 *
 * If a Bugsnag apiKey string has not been added to your Info.plist or is empty, an
 * NSException will be thrown to indicate that the configuration is not valid.
 *
 * Once successfully initialized, NSExceptions, C-- exceptions, Mach exceptions and
 * signals will be logged to disk before your app crashes. The next time your app
 * launches, these reports will be sent to your Bugsnag dashboard.
 */
- (BugsnagClient *_Nonnull)start;

/**
 * Start listening for crashes.
 *
 * This method initializes Bugsnag with the default configuration and the provided
 * apiKey.
 *
 * If apiKey is nil or is empty, an NSException will be thrown to indicate that the
 * configuration is not valid.
 *
 * Once successfully initialized, NSExceptions, C-- exceptions, Mach exceptions and
 * signals will be logged to disk before your app crashes. The next time your app
 * launches, these reports will be sent to your Bugsnag dashboard.
 *
 * @param apiKey  The API key from your Bugsnag dashboard.
 */
- (BugsnagClient *_Nonnull)startWithApiKey:(NSString *_Nonnull)apiKey;

/**
 * Start listening for crashes.
 *
 * This method initializes Bugsnag with the provided configuration object.
 *
 * If the configuration's apiKey is nil or is empty, an NSException will be thrown
 * to indicate that the configuration is not valid.
 *
 * Once successfully initialized, NSExceptions, C-- exceptions, Mach exceptions and
 * signals will be logged to disk before your app crashes. The next time your app
 * launches, these reports will be sent to your Bugsnag dashboard.
 *
 * @param configuration  The configuration to use.
 */
- (BugsnagClient *_Nonnull)startWithConfiguration:(BugsnagConfiguration *_Nonnull)configuration;

/**
 * @return YES if and only if a Bugsnag.start() has been called
 * and Bugsnag has initialized such that any calls to the Bugsnag methods can succeed
 */
- (BOOL)isStarted;

/**
 * Information about the last run of the app, and whether it crashed.
 */
@property (class, readonly, nullable, nonatomic) BugsnagLastRunInfo *lastRunInfo;

/**
 * Tells Bugsnag that your app has finished launching.
 *
 * Errors reported after calling this method will have the `BugsnagAppWithState.isLaunching`
 * property set to false.
 */
- (void)markLaunchCompleted;

// =============================================================================
// MARK: - Notify
// =============================================================================

/**
 * Send a custom or caught exception to Bugsnag.
 *
 * The exception will be sent to Bugsnag in the background allowing your
 * app to continue running.
 *
 * @param exception  The exception.
 */
- (void)notify:(NSException *_Nonnull)exception;

/**
 *  Send a custom or caught exception to Bugsnag
 *
 *  @param exception The exception
 *  @param block     A block for optionally configuring the error report
 */
- (void)notify:(NSException *_Nonnull)exception
         block:(BugsnagOnErrorBlock _Nullable)block;

/**
 *  Send an error to Bugsnag
 *
 *  @param error The error
 */
- (void)notifyError:(NSError *_Nonnull)error;

/**
 *  Send an error to Bugsnag
 *
 *  @param error The error
 *  @param block A block for optionally configuring the error report
 */
- (void)notifyError:(NSError *_Nonnull)error
              block:(BugsnagOnErrorBlock _Nullable)block;

// =============================================================================
// MARK: - Breadcrumbs
// =============================================================================

/**
 * Leave a "breadcrumb" log message, representing an action that occurred
 * in your app, to aid with debugging.
 *
 * @param message  the log message to leave
 */
- (void)leaveBreadcrumbWithMessage:(NSString *_Nonnull)message;

/**
 *  Leave a "breadcrumb" log message each time a notification with a provided
 *  name is received by the application
 *
 *  @param notificationName name of the notification to capture
 */
- (void)leaveBreadcrumbForNotificationName:(NSString *_Nonnull)notificationName;

/**
 * Leave a "breadcrumb" log message, representing an action that occurred
 * in your app, to aid with debugging, along with additional metadata and
 * a type.
 *
 * @param message The log message to leave.
 * @param metadata Diagnostic data relating to the breadcrumb.
 *                 Values should be serializable to JSON with NSJSONSerialization.
 * @param type A BSGBreadcrumbTypeValue denoting the type of breadcrumb.
 */
- (void)leaveBreadcrumbWithMessage:(NSString *_Nonnull)message
                          metadata:(NSDictionary *_Nullable)metadata
                           andType:(BSGBreadcrumbType)type
    NS_SWIFT_NAME(leaveBreadcrumb(_:metadata:type:));

/**
 * Leave a "breadcrumb" log message representing a completed network request.
 */
- (void)leaveNetworkRequestBreadcrumbForTask:(nonnull NSURLSessionTask *)task
                                     metrics:(nonnull NSURLSessionTaskMetrics *)metrics
    API_AVAILABLE(macosx(10.12), ios(10.0), watchos(3.0), tvos(10.0))
    NS_SWIFT_NAME(leaveNetworkRequestBreadcrumb(task:metrics:));

/**
 * Returns the current buffer of breadcrumbs that will be sent with captured events. This
 * ordered list represents the most recent breadcrumbs to be captured up to the limit
 * set in `BugsnagConfiguration.maxBreadcrumbs`
 */
- (NSArray<BugsnagBreadcrumb *> *_Nonnull)breadcrumbs;

// =============================================================================
// MARK: - Session
// =============================================================================

/**
 * Starts tracking a new session.
 *
 * By default, sessions are automatically started when the application enters the foreground.
 * If you wish to manually call startSession at
 * the appropriate time in your application instead, the default behaviour can be disabled via
 * autoTrackSessions.
 *
 * Any errors which occur in an active session count towards your application's
 * stability score. You can prevent errors from counting towards your stability
 * score by calling pauseSession and resumeSession at the appropriate
 * time in your application.
 *
 * @see pauseSession:
 * @see resumeSession:
 */
- (void)startSession;

/**
 * Stops tracking a session.
 *
 * When a session is stopped, errors will not count towards your application's
 * stability score. This can be advantageous if you do not wish these calculations to
 * include a certain type of error, for example, a crash in a background service.
 * You should disable automatic session tracking via autoTrackSessions if you call this method.
 *
 * A stopped session can be resumed by calling resumeSession,
 * which will make any subsequent errors count towards your application's
 * stability score. Alternatively, an entirely new session can be created by calling startSession.
 *
 * @see startSession:
 * @see resumeSession:
 */
- (void)pauseSession;

/**
 * Resumes a session which has previously been stopped, or starts a new session if none exists.
 *
 * If a session has already been resumed or started and has not been stopped, calling this
 * method will have no effect. You should disable automatic session tracking via
 * autoTrackSessions if you call this method.
 *
 * It's important to note that sessions are stored in memory for the lifetime of the
 * application process and are not persisted on disk. Therefore calling this method on app
 * startup would start a new session, rather than continuing any previous session.
 *
 * You should call this at the appropriate time in your application when you wish to
 * resume a previously started session. Any subsequent errors which occur in your application
 * will be reported to Bugsnag and will count towards your application's stability score.
 *
 * @see startSession:
 * @see pauseSession:
 *
 * @return true if a previous session was resumed, false if a new session was started.
 */
- (BOOL)resumeSession;

// =============================================================================
// MARK: - Other methods
// =============================================================================

/**
 * Retrieves the context - a general summary of what was happening in the application
 */
- (void)setContext:(NSString *_Nullable)context;

/**
 * Retrieves the context - a general summary of what was happening in the application
 */
- (NSString *_Nullable)context;

// =============================================================================
// MARK: - User
// =============================================================================

/**
 * The current user
 */
- (BugsnagUser *_Nonnull)user;

/**
 *  Set user metadata
 *
 *  @param userId ID of the user
 *  @param name   Name of the user
 *  @param email  Email address of the user
 *
 *  If user ID is nil, a Bugsnag-generated Device ID is used for the `user.id` property of events and sessions.
 */
- (void)setUser:(NSString *_Nullable)userId
       withEmail:(NSString *_Nullable)email
       andName:(NSString *_Nullable)name;

// =============================================================================
// MARK: - Feature flags
// =============================================================================

- (void)addFeatureFlagWithName:(nonnull NSString *)name variant:(nullable NSString *)variant
    NS_SWIFT_NAME(addFeatureFlag(name:variant:));

- (void)addFeatureFlagWithName:(nonnull NSString *)name
    NS_SWIFT_NAME(addFeatureFlag(name:));

- (void)addFeatureFlags:(nonnull NSArray<BugsnagFeatureFlag *> *)featureFlags
    NS_SWIFT_NAME(addFeatureFlags(_:));

- (void)clearFeatureFlagWithName:(nonnull NSString *)name
    NS_SWIFT_NAME(clearFeatureFlag(name:));

- (void)clearFeatureFlags;

// =============================================================================
// MARK: - onSession
// =============================================================================

/**
 *  Add a callback to be invoked before a session is sent to Bugsnag.
 *
 *  @param block A block which can modify the session
 *
 *  @returns An opaque reference to the callback which can be passed to `removeOnSession:`
 */
- (nonnull BugsnagOnSessionRef)addOnSessionBlock:(nonnull BugsnagOnSessionBlock)block
    NS_SWIFT_NAME(addOnSession(block:));



// =============================================================================
// MARK: - onBreadcrumb
// =============================================================================

/**
 *  Add a callback to be invoked when a breadcrumb is captured by Bugsnag, to
 *  change the breadcrumb contents as needed
 *
 *  @param block A block which returns YES if the breadcrumb should be captured
 *
 *  @returns An opaque reference to the callback which can be passed to `removeOnBreadcrumb:`
 */
- (nonnull BugsnagOnBreadcrumbRef)addOnBreadcrumbBlock:(nonnull BugsnagOnBreadcrumbBlock)block
    NS_SWIFT_NAME(addOnBreadcrumb(block:));

/**
 * Remove the callback that would be invoked when a breadcrumb is captured.
 *
 * @param callback The opaque reference of the callback, returned by `addOnBreadcrumbBlock:`
 */
- (void)removeOnBreadcrumb:(nonnull BugsnagOnBreadcrumbRef)callback
    NS_SWIFT_NAME(removeOnBreadcrumb(_:));

- (NSDictionary *)createEvent:(NSDictionary *)jsonError unhandled:(Boolean)unhandled deliver:(Boolean)deliver;

- (void)deliverEvent:(NSDictionary *)json;

@end
