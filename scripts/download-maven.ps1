# Get the script location
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Write-Output "Script Dir: $scriptDir"

# Get the version from the .sdk-version file
$version = Get-Content $scriptDir/../.maven-sdk-version

# Define the Maven group, artifact, and type
$group = "com.bugsnag"
$outputDirectory = "$scriptDir\..\Bugsnag.Android\Native"

Write-Output "Output Dir: $outputDirectory"

# Replace . in group with / for the URL
$groupUrl = $group -replace "\.", "/"

# Create the output directory if it doesn't exist
if (Test-Path $outputDirectory) {
    Remove-Item "$outputDirectory/*" -Recurse -Force
} else {
    New-Item -ItemType Directory -Path $outputDirectory -Force
}

function Get-AAR {
    param (
        [string]$groupUrl,
        [string]$artifact,
        [string]$version,
        [string]$type,
        [string]$outputDir
    )

    # Define the URL to download the AAR from
    $url = "https://repo1.maven.org/maven2/$groupUrl/$artifact/$version/$artifact-$version.$type"

    # Define the output file
    $outputFile = "$outputDir\$artifact-$version.$type"
    
    Write-Output "Downloading $artifact-$version.$type from $url"  

    # Download the AAR file
    Invoke-WebRequest -Uri $url -OutFile $outputFile

    Write-Output "Downloaded $artifact-$version.$type to $outputFile"
}

# Call the function
Get-AAR -groupUrl $groupUrl -artifact "bugsnag-android-core" -version $version -type "aar" -outputDir $outputDirectory
Get-AAR -groupUrl $groupUrl -artifact "bugsnag-plugin-android-anr" -version $version -type "aar" -outputDir $outputDirectory
Get-AAR -groupUrl $groupUrl -artifact "bugsnag-plugin-android-ndk" -version $version -type "aar" -outputDir $outputDirectory

Set-Content -Path "$outputDirectory\.maven-sdk-version" -Value $version
