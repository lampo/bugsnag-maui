#!/bin/bash

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)

# Get the version from the .sdk-version file
version=$(cat "$SCRIPT_DIR/../.maven-sdk-version")

# Define the Maven group, artifact, and type
group=com.bugsnag
artifact=bugsnag-android
type=jar

# Replace . in group with / for the URL
groupUrl=${group//./\/}

# Define the URL to download the AAR from
url=https://repo1.maven.org/maven2/$groupUrl/$artifact/$version/$artifact-$version.$type

echo "Downloading $artifact-$version.$type from $url"

# Define the output directory and file
outputDir=$SCRIPT_DIR/../Bugsnag.Android/Native
outputFile=$outputDir/$artifact-$version.$type

# Create the output directory if it doesn't exist
if [ -d "$outputDir" ]; then
    rm -r "$outputDir/*"
else
  mkdir -p $outputDir  
fi


# Download the AAR file
curl -L $url -o $outputFile

echo "Downloaded $artifact-$version.$type to $outputFile"

echo -n "$version" > "$SCRIPT_DIR/../Bugsnag.Android/.maven-sdk-version"