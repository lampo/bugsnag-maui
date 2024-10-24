#!/bin/bash

# Get the script location
scriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Get the version from the .sdk-version file
version=$(cat "$scriptDir/../.maven-sdk-version")

# Define the Maven group, artifact, and type
group="com.bugsnag"
outputDirectory="$scriptDir/../Bugsnag.Android/Native"

# Replace . in group with / for the URL
groupUrl="${group//.//}"

# Create the output directory if it doesn't exist
if [ -d "$outputDirectory" ]; then
    rm -rf "$outputDirectory/*"
else
    mkdir -p "$outputDirectory"
fi

function GetAAR {
    local groupUrl=$1
    local artifact=$2
    local version=$3
    local type=$4
    local outputDir=$5

    # Define the URL to download the AAR from
    local url="https://repo1.maven.org/maven2/$groupUrl/$artifact/$version/$artifact-$version.$type"

    # Define the output file
    local outputFile="$outputDir/$artifact-$version.$type"
    
    echo "Downloading $artifact-$version.$type from $url"  

    # Download the AAR file
    curl -o "$outputFile" "$url"

    echo "Downloaded $artifact-$version.$type to $outputFile"
}

# Call the function
GetAAR "$groupUrl" "bugsnag-android-core" "$version" "aar" "$outputDirectory"
GetAAR "$groupUrl" "bugsnag-plugin-android-anr" "$version" "aar" "$outputDirectory"
GetAAR "$groupUrl" "bugsnag-plugin-android-ndk" "$version" "aar" "$outputDirectory"

echo "$version" > "$scriptDir/../Bugsnag.Android/.maven-sdk-version"