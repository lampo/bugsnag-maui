#!/bin/bash

# Define the path to the Bugsnag.xcframework
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)

/usr/bin/xcodebuild \
  -project "$SCRIPT_DIR/../bugsnag-cocoa-build/bugsnag-ios.xcodeproj" archive \
  -scheme BugsnagBinding \
  -configuration Release \
  -archivePath "$SCRIPT_DIR/../bugsnag-cocoa-build/bin/Release/net8.0-ios/bugsnag-iosiOS.xcarchive" \
  -destination "generic/platform=iOS"  \
  ENABLE_BITCODE=NO \
  SKIP_INSTALL=NO \
  SWIFT_INSTALL_OBJC_HEADER=YES \
  BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
  OTHER_LDFLAGS="-ObjC" \
  OTHER_SWIFT_FLAGS="-no-verify-emitted-module-interface" \
  OBJC_CFLAGS="-fno-objc-msgsend-selector-stubs -ObjC" 