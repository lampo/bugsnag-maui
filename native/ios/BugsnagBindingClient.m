#import "BugsnagBindingClient.h"
#import "BugsnagInternals.h"

@implementation BugsnagBindingClient

- (BugsnagClient *)start {
    return [Bugsnag start];
}

- (BugsnagClient *)startWithApiKey:(NSString *)apiKey {
    return [Bugsnag startWithApiKey:apiKey];
}

- (BugsnagClient *)startWithConfiguration:(BugsnagConfiguration *)configuration {
    return [Bugsnag startWithConfiguration:configuration];
}

- (BOOL)isStarted {
    return [Bugsnag isStarted];
}

- (BugsnagLastRunInfo *)lastRunInfo {
    return [Bugsnag lastRunInfo];
}

- (void)markLaunchCompleted {
    [Bugsnag markLaunchCompleted];
}

- (void)notify:(NSException *)exception {
    [Bugsnag notify:exception];
}

- (void)notify:(NSException *)exception block:(BugsnagOnErrorBlock)block {
    [Bugsnag notify:exception block:block];
}

- (void)notifyError:(NSError *)error {
    [Bugsnag notifyError:error];
}

- (void)notifyError:(NSError *)error block:(BugsnagOnErrorBlock)block {
    [Bugsnag notifyError:error block:block];
}

- (void)leaveBreadcrumbWithMessage:(NSString *)message {
    [Bugsnag leaveBreadcrumbWithMessage:message];
}

- (void)leaveBreadcrumbForNotificationName:(NSString *)notificationName {
    [Bugsnag leaveBreadcrumbForNotificationName:notificationName];
}

- (void)leaveBreadcrumbWithMessage:(NSString *)message metadata:(NSDictionary *)metadata andType:(BSGBreadcrumbType)type {
    [Bugsnag leaveBreadcrumbWithMessage:message metadata:metadata andType:type];
}

- (void)leaveNetworkRequestBreadcrumbForTask:(NSURLSessionTask *)task metrics:(NSURLSessionTaskMetrics *)metrics {
    [Bugsnag leaveNetworkRequestBreadcrumbForTask:task metrics:metrics];
}

- (NSArray<BugsnagBreadcrumb *> *)breadcrumbs {
    return [Bugsnag breadcrumbs];
}

- (void)startSession {
    [Bugsnag startSession];
}

- (void)pauseSession {
    [Bugsnag pauseSession];
}

- (BOOL)resumeSession {
    return [Bugsnag resumeSession];
}

- (void)setContext:(NSString *)context {
    [Bugsnag setContext:context];
}

- (NSString *)context {
    return [Bugsnag context];
}

- (BugsnagUser *)user {
    return [Bugsnag user];
}

- (void)setUser:(NSString *)userId withEmail:(NSString *)email andName:(NSString *)name {
    [Bugsnag setUser:userId withEmail:email andName:name];
}

- (void)addFeatureFlagWithName:(NSString *)name variant:(NSString *)variant {
    [Bugsnag addFeatureFlagWithName:name variant:variant];
}

- (void)addFeatureFlagWithName:(NSString *)name {
    [Bugsnag addFeatureFlagWithName:name];
}

- (void)addFeatureFlags:(NSArray<BugsnagFeatureFlag *> *)featureFlags {
    [Bugsnag addFeatureFlags:featureFlags];
}

- (void)clearFeatureFlagWithName:(NSString *)name {
    [Bugsnag clearFeatureFlagWithName:name];
}

- (void)clearFeatureFlags {
    [Bugsnag clearFeatureFlags];
}

- (BugsnagOnSessionRef)addOnSessionBlock:(BugsnagOnSessionBlock)block {
    return [Bugsnag addOnSessionBlock:block];
}

- (BugsnagOnBreadcrumbRef)addOnBreadcrumbBlock:(BugsnagOnBreadcrumbBlock)block {
    return [Bugsnag addOnBreadcrumbBlock:block];
}

- (void)removeOnBreadcrumb:(BugsnagOnBreadcrumbRef)callback {
    [Bugsnag removeOnBreadcrumb:callback];
}

- (NSDictionary *)createEvent:(NSDictionary *)jsonError unhandled:(Boolean)unhandled deliver:(Boolean)deliver {
    NSDictionary *systemInfo = BSGGetSystemInfo();
    BugsnagClient *client = Bugsnag.client;
    BugsnagError *error = [BugsnagError errorFromJson:jsonError];
    BugsnagEvent *event = [[BugsnagEvent alloc] initWithApp:[client generateAppWithState:systemInfo]
                                                     device:[client generateDeviceWithState:systemInfo]
                                               handledState:[BugsnagHandledState handledStateWithSeverityReason:
                                                             unhandled ? UnhandledException : HandledException]
                                                       user:client.user
                                                   metadata:[client.metadata copy]
                                                breadcrumbs:[client breadcrumbs]
                                                     errors:@[error]
                                                    threads:@[]
                                                    session:nil /* set by -[BugsnagClient notifyInternal:block:] */];
    event.apiKey = client.configuration.apiKey;
    event.context = client.context;
    
    // TODO: Expose BugsnagClient's featureFlagStore or provide a better way to create an event
    id featureFlagStore = [client valueForKey:@"featureFlagStore"];
    @synchronized (featureFlagStore) {
        event.featureFlagStore = [featureFlagStore copy];
    }

//    for (BugsnagStackframe *frame in error.stacktrace) {
//        if ([frame.type isEqualToString:@"dart"] && !frame.codeIdentifier) {
//            frame.codeIdentifier = DartCodeBuildId;
//        }
//    }
//    
    if (client.configuration.sendThreads == BSGThreadSendPolicyAlways) {
        event.threads = [BugsnagThread allThreads:YES callStackReturnAddresses:NSThread.callStackReturnAddresses];
    }

//    NSDictionary *metadata = json[@"flutterMetadata"];
//    if (metadata != nil) {
//        [event addMetadata:metadata toSection:@"flutter"];
//        if (!metadata[@"buildID"]) {
//            [event addMetadata:DartCodeBuildId withKey:@"buildID" toSection:@"flutter"];
//        }
//    }

//    NSDictionary *correlation = json[@"correlation"];
//    if (correlation != nil) {
//        NSString *traceId = correlation[@"traceId"];
//        NSString *spanId = correlation[@"spanId"];
//        if (traceId != nil && spanId != nil) {
//            [event setCorrelationTraceId:traceId spanId:spanId];
//        }
//    }
    
    if (deliver) {
        [client notifyInternal:event block:nil];
        return nil;
    } else {
        // A BugsnagStackframe initialized from JSON won't symbolicate, so do it now.
        [event symbolicateIfNeeded];
        return [event toJsonWithRedactedKeys:nil];
    }
}

- (void)deliverEvent:(NSDictionary *)json {
    BugsnagEvent *event = [[BugsnagEvent alloc] initWithJson:json];
    [Bugsnag.client notifyInternal:event block:nil];
}

@end
