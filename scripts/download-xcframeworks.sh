#! /bin/bash
# Unofficial bash strict mode
set -euo pipefail
IFS=$'\n\t'

###################################################################################################
# This script downloads and copies the prebuilt xcframeworks to the corresponding module
# directories.
# When called without argument, it uses the content of ../.swift-sdk-version as the version of the
# Swift SDK to download. Otherwise, the first argument is used as the version.
###################################################################################################

# Constants
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)

# Parameters
SWIFT_SDK_VERSION=${1:-$(cat "$SCRIPT_DIR/../.swift-sdk-version")}

# Main
cd "$SCRIPT_DIR"

# - Clean up previous artifacts
if [ -d "artifacts" ] || [ -f "artifacts.zip" ]; then
  echo "→ Cleaning up previous artifacts"
  rm -rf artifacts artifacts.zip
fi

# - Download prebuilt artifacts
echo "→ Downloading Swift SDK prebuilt artifacts (version $SWIFT_SDK_VERSION)"
curl -L -o Bugsnag.xcframework.zip https://github.com/bugsnag/bugsnag-cocoa/releases/download/v$SWIFT_SDK_VERSION/Bugsnag.xcframework.zip
curl -L -o BugsnagNetworkRequestPlugin.xcframework.zip https://github.com/bugsnag/bugsnag-cocoa/releases/download/v$SWIFT_SDK_VERSION/BugsnagNetworkRequestPlugin.xcframework.zip


# - Unzip prebuilt artifacts
echo "→ Unzipping prebuilt artifacts"
unzip -q Bugsnag.xcframework.zip -d artifacts
rm Bugsnag.xcframework.zip

unzip -q BugsnagNetworkRequestPlugin.xcframework.zip -d artifacts
rm BugsnagNetworkRequestPlugin.xcframework.zip

# - Copy dynamic xcframeworks to corresponding module directories
echo "→ Copying dynamic xcframeworks"

MODULE="Bugsnag"
moduleDir="$SCRIPT_DIR/../native/BugsnagBinding/"


echo "  · $MODULE"
rm -rf "$moduleDir/$MODULE.xcframework"
mkdir -p "$moduleDir"
cp -rf "artifacts/$MODULE.xcframework" "$moduleDir"

MODULE="BugsnagNetworkRequestPlugin"
echo "  · $MODULE"
rm -rf "$moduleDir/$MODULE.xcframework"
mkdir -p "$moduleDir"
cp -rf "artifacts/$MODULE.xcframework" "$moduleDir"

echo "$SWIFT_SDK_VERSION" >"$moduleDir/.swift-sdk-version"

# - Cleanup run
echo "→ Cleaning up"
rm -rf artifacts
