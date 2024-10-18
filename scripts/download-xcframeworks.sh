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

# Function to download and copy xcframework
download_and_copy_xcframework() {
  local module=$1
  local moduleDir=$2

  echo "→ Downloading $module prebuilt artifacts (version $SWIFT_SDK_VERSION)"
  curl -L -o "$module.xcframework.zip" "https://github.com/bugsnag/bugsnag-cocoa/releases/download/v$SWIFT_SDK_VERSION/$module.xcframework.zip"

  echo "→ Unzipping $module prebuilt artifacts"
  unzip -q "$module.xcframework.zip" -d artifacts
  rm "$module.xcframework.zip"

  echo "→ Copying $module dynamic xcframework"
  rm -rf "$moduleDir/$module.xcframework"
  mkdir -p "$moduleDir"
  cp -rf "artifacts/$module.xcframework" "$moduleDir"
}

# Main
cd "$SCRIPT_DIR"

# - Clean up previous artifacts
if [ -d "artifacts" ] || [ -f "artifacts.zip" ]; then
  echo "→ Cleaning up previous artifacts"
  rm -rf artifacts artifacts.zip
fi

# - Download and copy xcframeworks
download_and_copy_xcframework "Bugsnag" "$SCRIPT_DIR/../native/BugsnagBinding/"
download_and_copy_xcframework "BugsnagNetworkRequestPlugin" "$SCRIPT_DIR/../native/BugsnagBinding/"

echo "$SWIFT_SDK_VERSION" >"$SCRIPT_DIR/../native/BugsnagBinding/.swift-sdk-version"

# - Cleanup run
echo "→ Cleaning up"
rm -rf artifacts