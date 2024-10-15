# Get the version from the .sdk-version file
$version = Get-Content ../.maven-sdk-version

# Define the Maven group, artifact, and type
$group = "com.mastercard.openbanking.connect"
$artifact = "connect-sdk"
$type = "aar"

# Replace . in group with / for the URL
$groupUrl = $group -replace "\.", "/"

# Define the URL to download the AAR from
$url = "https://repo1.maven.org/maven2/$groupUrl/$artifact/$version/$artifact-$version.$type"

Write-Output "Downloading $artifact-$version.$type from $url"

# Define the output directory and file
$outputDir = "../Bugsnag.Android/Native"
$outputFile = "$outputDir/$artifact-$version.$type"

# Create the output directory if it doesn't exist
if (Test-Path $outputDir) {
    Remove-Item "$outputDir/*" -Recurse -Force
} else {
    New-Item -ItemType Directory -Path $outputDir -Force
}

# Download the AAR file
Invoke-WebRequest -Uri $url -OutFile $outputFile

Write-Output "Downloaded $artifact-$version.$type to $outputFile"

Set-Content -Path "../Bugsnag.Android/.maven-sdk-version" -Value $version
