#!/bin/bash

# Define the path to the Bugsnag.xcframework
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)
RESOURCE_DIR="$SCRIPT_DIR/../Bugsnag.iOS/bin/Debug/net8.0-ios/Bugsnag.iOS.resources"
XCFRAMEWORK_PATH="$RESOURCE_DIR/BugsnagBindingiOS.xcframework/ios-arm64/BugsnagBinding.framework/BugsnagBinding"


otool -L $XCFRAMEWORK_PATH

nm $XCFRAMEWORK_PATH | grep "startBugsnagWithConfiguration"
