###################################################################################################
# This script verifies that the desired Maven SDK version is installed and downloads it if not.
# It is executed automatically by the iOS `.csproj` files.
# It uses:
# - The content of ../.maven-sdk-version as the desired version of the Maven SDK.
# - The content of .maven-sdk-version as the current installed version of the Maven SDK (this file
#   exist side by side with the modules `.csproj`).
###################################################################################################

# Constants
$SCRIPT_DIR = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$DESIRED_MAVEN_SDK_VERSION = Get-Content "$SCRIPT_DIR\..\.maven-sdk-version"

if (Test-Path .\.maven-sdk-version) {
  $CURRENT_MAVEN_SDK_VERSION = Get-Content ".\.maven-sdk-version"
} else {
  $CURRENT_MAVEN_SDK_VERSION = "unknown"
}

Write-Host "→ Checking Maven SDK version"
Write-Host "  · Desired version: $DESIRED_MAVEN_SDK_VERSION"
Write-Host "  · Current version: $CURRENT_MAVEN_SDK_VERSION"

# Main
# - Early exit if the desired Maven SDK version is already installed
if ($CURRENT_MAVEN_SDK_VERSION -eq $DESIRED_MAVEN_SDK_VERSION) {
  Write-Host "The desired Maven SDK version ($DESIRED_MAVEN_SDK_VERSION) is already installed"
  exit
}

# - Download the desired Maven SDK version
& "$SCRIPT_DIR\download-maven.ps1" $DESIRED_MAVEN_SDK_VERSION
