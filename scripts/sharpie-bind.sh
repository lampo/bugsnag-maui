#!/bin/bash

# Define the path to the Bugsnag.xcframework
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)
BUGSNAG_DIR="$SCRIPT_DIR/../Bugsnag.iOS"
XCFRAMEWORK_PATH="$BUGSNAG_DIR/Bugsnag.xcframework/ios-arm64/Bugsnag.framework"

# Use sharpie to generate bindings
sharpie bind -o $BUGSNAG_DIR -n Bugsnag.iOS -f $XCFRAMEWORK_PATH -scope $XCFRAMEWORK_PATH/Headers -sdk iphoneos17.5