using System;
using Foundation;

namespace Bugsnag.iOS
{
	// @interface DotnetBugsnagBinding : NSObject
	[BaseType (typeof(NSObject), Name = "_TtC14BugsnagBinding20DotnetBugsnagBinding")]
	interface DotnetBugsnagBinding
	{
		// +(void)start;
		[Static]
		[Export ("start")]
		void Start ();

		// +(void)markLaunchCompleted;
		[Static]
		[Export ("markLaunchCompleted")]
		void MarkLaunchCompleted ();

		// +(void)addFeatureFlagWith:(NSString * _Nonnull)name variant:(NSString * _Nonnull)variant;
		[Static]
		[Export ("addFeatureFlagWith:variant:")]
		void AddFeatureFlagWith (string name, string variant);

		// +(void)clearFeatureFlagWith:(NSString * _Nonnull)name;
		[Static]
		[Export ("clearFeatureFlagWith:")]
		void ClearFeatureFlagWith (string name);

		// +(void)clearFeatureFlags;
		[Static]
		[Export ("clearFeatureFlags")]
		void ClearFeatureFlags ();

		// +(void)notifyErrorWith:(NSError * _Nonnull)error;
		[Static]
		[Export ("notifyErrorWith:")]
		void NotifyErrorWith (NSError error);

		// +(void)notifyWith:(NSException * _Nonnull)exception;
		[Static]
		[Export ("notifyWith:")]
		void NotifyWith (NSException exception);

		// +(void)setUserWith:(NSString * _Nonnull)userId;
		[Static]
		[Export ("setUserWith:")]
		void SetUserWith (string userId);

		// +(void)leaveBreadcrumbWith:(NSString * _Nonnull)message;
		[Static]
		[Export ("leaveBreadcrumbWith:")]
		void LeaveBreadcrumbWith (string message);

		// +(void)leaveBreadcrumbWith:(NSString * _Nonnull)message with:(NSDictionary * _Nonnull)metadata;
		[Static]
		[Export ("leaveBreadcrumbWith:with:")]
		void LeaveBreadcrumbWith (string message, NSDictionary metadata);
	}
}
