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

# Check the framework structure
if [ ! -f "$XCFRAMEWORK_PATH/BugsnagBinding" ] || [ ! -d "$XCFRAMEWORK_PATH/Headers" ] || [ ! -f "$XCFRAMEWORK_PATH/Modules/module.modulemap" ]; then
  echo "Error: Framework structure is incorrect"
  exit 1
fi

# Check the architectures in the binary
lipo -info "$XCFRAMEWORK_PATH/BugsnagBinding"
if [ $? -ne 0 ]; then
  echo "Error: Failed to check architectures"
  exit 1
fi

# Check the symbols in the binary
nm "$XCFRAMEWORK_PATH/BugsnagBinding" | grep Bugsnag
if [ $? -ne 0 ]; then
  echo "Error: Failed to find expected symbols"
  exit 1
fi

echo "Framework built correctly and is usable"