#!/bin/bash

# Define the path to the Bugsnag.xcframework
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)
BUGSNAG_DIR="$SCRIPT_DIR/../bugsnag-cocoa-build/bin/Release/net8.0-ios"
XCFRAMEWORK_PATH="$BUGSNAG_DIR/BugsnagBindingiOS.xcframework/ios-arm64/BugsnagBinding.framework"

# Check if the framework path exists
if [ ! -d "$XCFRAMEWORK_PATH" ]; then
  echo "Error: Framework path does not exist: $XCFRAMEWORK_PATH"
  exit 1
fi

# Use sharpie to generate bindings with verbose logging
sharpie bind \
   -o $BUGSNAG_DIR \
   -n Bugsnag.iOS \
   -f $XCFRAMEWORK_PATH \
   -scope \
   $XCFRAMEWORK_PATH/Headers/BugsnagEvent.h \
   -sdk iphoneos17.5 \
   --verbose

# Check the exit status of sharpie
if [ $? -ne 0 ]; then
  echo "Error: sharpie bind command failed"
  exit 1
fi

echo "Bindings generated successfully"