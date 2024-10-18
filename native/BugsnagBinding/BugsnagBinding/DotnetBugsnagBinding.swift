import Foundation
import Bugsnag
import BugsnagNetworkRequestPlugin
import UIKit

@objc public class DotnetBugsnagBinding : NSObject {
        
    @objc public static func start(with releaseStage: String) {
        let config = BugsnagConfiguration.loadConfig()
        config.launchDurationMillis = 0
        config.releaseStage = releaseStage
        config.add(BugsnagNetworkRequestPlugin())
        Bugsnag.start(with: config)
    }
    
    @objc public static func markLaunchCompleted() {
        Bugsnag.markLaunchCompleted()
    }
    
    @objc public static func addFeatureFlag(with name: String, variant: String) {
        Bugsnag.addFeatureFlag(name:name, variant: variant)
    }
    
    @objc public static func clearFeatureFlag(with name: String){
        Bugsnag.clearFeatureFlag(name:name)
    }
    
    @objc public static func clearFeatureFlags() {
        Bugsnag.clearFeatureFlags()
    }
    
    @objc public static func notifyError(with error: NSError) {
        Bugsnag.notifyError(error)
    }
    
    @objc public static func notify(with exception: NSException) {
        Bugsnag.notify(exception)
    }
    
    @objc public static func setUser(with userId: String) {
        Bugsnag.setUser(userId, withEmail: nil, andName: nil)
    }
    
    @objc public static func leaveBreadcrumb(with message: String) {
        Bugsnag.leaveBreadcrumb(withMessage: message)
    }
    
    @objc public static func leaveBreadcrumb(with message: String, with metadata: [AnyHashable : Any] ) {
        Bugsnag.leaveBreadcrumb(message, metadata: metadata, type: .state)
    }
}
