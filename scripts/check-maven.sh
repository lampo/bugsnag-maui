#! /bin/bash
# Unofficial bash strict mode
set -euo pipefail
IFS=$'\n\t'

###################################################################################################
# This script verifies that the desired maven SDK version is installed and downloads it if not.
# It is executed automatically by the iOS `.csproj` files.
# It uses:
# - The content of ../.maven-sdk-version as the desired version of the maven SDK.
# - The content of .maven-sdk-version as the current installed version of the maven SDK (this file
#   exist side by side with the modules `.csproj`).
###################################################################################################

# Constants
SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &> /dev/null && pwd)
DESIRED_MAVEN_SDK_VERSION=$(cat "$SCRIPT_DIR/../.maven-sdk-version")

if [[ -f .maven-sdk-version ]]; then
  CURRENT_MAVEN_SDK_VERSION=$(cat ".maven-sdk-version")
else
  CURRENT_MAVEN_SDK_VERSION="unknown"
fi

echo "→ Checking maven SDK version"
echo "  · Desired version: $DESIRED_MAVEN_SDK_VERSION"
echo "  · Current version: $CURRENT_MAVEN_SDK_VERSION"

# Main
# - Early exit if the desired maven SDK version is already installed
if [ "$CURRENT_MAVEN_SDK_VERSION" = "$DESIRED_MAVEN_SDK_VERSION" ]; then
  echo "The desired maven SDK version ($DESIRED_MAVEN_SDK_VERSION) is already installed"
  exit 0
fi

# - Download the desired maven SDK version
"$SCRIPT_DIR/download-maven.sh" "$DESIRED_MAVEN_SDK_VERSION"
