#import <Foundation/Foundation.h>
#import "Bugsnag.h"
#import "BugsnagUser.h"
#import "BugsnagEvent.h"
#import "BugsnagApp.h"
#import "BugsnagAppWithState.h"
#import "BugsnagDevice.h"
#import "BugsnagDeviceWithState.h"
#import "BugsnagConfiguration.h"

#ifdef __cplusplus
extern "C" {
#endif

struct bugsnag_user {
    const char *user_id;
    const char *user_name;
    const char *user_email;
};

void bugsnag_startBugsnagWithConfiguration(BugsnagConfiguration *configuration, char *notifierVersion);

void bugsnag_clearMetadata(const char *section);
void bugsnag_clearMetadataWithKey(const char *section, const char *key);

NSDictionary *getDictionaryFromMetadataJson(const char *jsonString);

const char *bugsnag_getEventMetaData(const void *event, const char *tab);
void bugsnag_clearEventMetadataWithKey(const void *event, const char *section, const char *key);
void bugsnag_clearEventMetadataSection(const void *event, const char *section);
void bugsnag_setEventMetadata(const void *event, const char *tab, const char *metadataJson);

BugsnagUser *bugsnag_getUserFromSession(const void *session);
void bugsnag_setUserFromSession(const void *session, char *userId, char *userEmail, char *userName);
BugsnagUser *bugsnag_getUserFromEvent(const void *event);
void bugsnag_setUserFromEvent(const void *event, char *userId, char *userEmail, char *userName);

void bugsnag_getThreadsFromEvent(const void *event, const void *instance, void (*callback)(const void *instance, void *threads[], int threads_size));

void bugsnag_setEventSeverity(const void *event, const char *severity);
const char *bugsnag_getSeverityFromEvent(const void *event);

void bugsnag_getStackframesFromError(const void *error, const void *instance, void (*callback)(const void *instance, void *stackframes[], int stackframes_size));
void bugsnag_getStackframesFromThread(const void *thread, const void *instance, void (*callback)(const void *instance, void *stackframes[], int stackframes_size));

void bugsnag_getErrorsFromEvent(const void *event, const void *instance, void (*callback)(const void *instance, void *errors[], int errors_size));
void bugsnag_getBreadcrumbsFromEvent(const void *event, const void *instance, void (*callback)(const void *instance, void *breadcrumbs[], int breadcrumbs_size));

const char *bugsnag_getFeatureFlagsFromEvent(BugsnagEvent *event);

const char *bugsnag_getBreadcrumbMetadata(const void *breadcrumb);
void bugsnag_setBreadcrumbMetadata(const void *breadcrumb, const char *jsonString);

const char *bugsnag_getBreadcrumbType(const void *breadcrumb);
void bugsnag_setBreadcrumbType(const void *breadcrumb, char *type);

const char *bugsnag_getValueAsString(const void *object, char *key);
void bugsnag_setNumberValue(const void *object, char *key, const char *value);

double bugsnag_getTimestampFromDateInObject(const void *object, char *key);
void bugsnag_setTimestampFromDateInObject(const void *object, char *key, double timeStamp);

void bugsnag_setRuntimeVersionsFromDevice(const void *device, const char *versions[], int count);
const char *bugsnag_getRuntimeVersionsFromDevice(const void *device);

void bugsnag_setBoolValue(const void *object, char *key, char *value);
void bugsnag_setStringValue(const void *object, char *key, char *value);

const char *bugsnag_getErrorTypeFromError(const void *error);
const char *bugsnag_getThreadTypeFromThread(const void *thread);

BugsnagApp *bugsnag_getAppFromSession(const void *session);
BugsnagAppWithState *bugsnag_getAppFromEvent(const void *event);

BugsnagDevice *bugsnag_getDeviceFromSession(const void *session);
BugsnagDeviceWithState *bugsnag_getDeviceFromEvent(const void *event);

void bugsnag_registerForSessionCallbacks(const void *configuration, bool (*callback)(void *session));
void bugsnag_registerForOnSendCallbacks(const void *configuration, bool (*callback)(void *event));

void bugsnag_registerSession(char *sessionId, long startedAt, int unhandledCount, int handledCount);
void bugsnag_retrieveCurrentSession(const void *ptr, void (*callback)(const void *instance, const char *sessionId, const char *startedAt, int handled, int unhandled));

void bugsnag_markLaunchCompleted();
void bugsnag_registerForSessionCallbacksAfterStart(bool (*callback)(void *session));

void *bugsnag_createConfiguration(char *apiKey);
void bugsnag_setReleaseStage(const void *configuration, char *releaseStage);

void bugsnag_addFeatureFlagOnConfig(const void *configuration, char *name, char *variant);
void bugsnag_addFeatureFlag(char *name, char *variant);
void bugsnag_clearFeatureFlag(char *name);
void bugsnag_clearFeatureFlags();

void bugsnag_addFeatureFlagOnEvent(const void *event, char *name, char *variant);
void bugsnag_clearFeatureFlagOnEvent(const void *event, char *name);
void bugsnag_clearFeatureFlagsOnEvent(const void *event);

void bugsnag_setNotifyReleaseStages(const void *configuration, const char *releaseStages[], int releaseStagesCount);
void bugsnag_setAppVersion(const void *configuration, char *appVersion);
void bugsnag_setAppHangThresholdMillis(const void *configuration, NSUInteger appHangThresholdMillis);
void bugsnag_setLaunchDurationMillis(const void *configuration, NSUInteger launchDurationMillis);
void bugsnag_setBundleVersion(const void *configuration, char *bundleVersion);
void bugsnag_setAppType(const void *configuration, char *appType);
void bugsnag_setContext(const void *configuration, char *context);
void bugsnag_setContextConfig(const void *configuration, char *context);
void bugsnag_setMaxBreadcrumbs(const void *configuration, int maxBreadcrumbs);
void bugsnag_setMaxStringValueLength(const void *configuration, int maxStringValueLength);
void bugsnag_setMaxPersistedEvents(const void *configuration, int maxPersistedEvents);
void bugsnag_setMaxPersistedSessions(const void *configuration, int maxPersistedSessions);
void bugsnag_setEnabledBreadcrumbTypes(const void *configuration, const char *types[], int count);
void bugsnag_setEnabledTelemetryTypes(const void *configuration, const char *types[], int count);
void bugsnag_setThreadSendPolicy(const void *configuration, char *threadSendPolicy);
void bugsnag_setEnabledErrorTypes(const void *configuration, const char *types[], int count);
void bugsnag_setDiscardClasses(const void *configuration, const char *classNames[], int count);
void bugsnag_setUserInConfig(const void *configuration, char *userId, char *userEmail, char *userName);
void bugsnag_setRedactedKeys(const void *configuration, const char *redactedKeys[], int count);
void bugsnag_setAutoNotifyConfig(const void *configuration, bool autoNotify);
void bugsnag_setAutoTrackSessions(const void *configuration, bool autoTrackSessions);
void bugsnag_setPersistUser(const void *configuration, bool persistUser);
void bugsnag_setSendLaunchCrashesSynchronously(const void *configuration, bool sendLaunchCrashesSynchronously);
void bugsnag_setEndpoints(const void *configuration, char *notifyURL, char *sessionsURL);

void bugsnag_setMetadata(const char *section, const char *jsonString);
const char *bugsnag_retrieveMetaData();
void bugsnag_removeMetadata(const void *configuration, const char *tab);

void bugsnag_addBreadcrumb(char *message, char *type, char *metadataJson);
void bugsnag_retrieveBreadcrumbs(const void *managedBreadcrumbs, void (*breadcrumb)(const void *instance, const char *name, const char *timestamp, const char *type, const char *metadataJson));

char *bugsnag_retrieveAppData();
void bugsnag_retrieveLastRunInfo(const void *lastRuninfo, void (*callback)(const void *instance, bool crashed, bool crashedDuringLaunch, int consecutiveLaunchCrashes));
char *bugsnag_retrieveDeviceData(const void *deviceData, void (*callback)(const void *instance, const char *key, const char *value));

void bugsnag_populateUser(struct bugsnag_user *user);
void bugsnag_setUser(char *userId, char *userEmail, char *userName);
void bugsnag_startSession();
void bugsnag_pauseSession();
bool bugsnag_resumeSession();

#ifdef __cplusplus
}
#endif

#endif /* BugsnagBindingClient_h */