Bugsnag.Maui: [![Bugsnag.Maui package in RamseySolutions feed in Azure Artifacts](https://feeds.dev.azure.com/RamseySolutions/_apis/public/Packaging/Feeds/RamseySolutions/Packages/869fe73e-575e-418a-af32-e59bf4b8df69/Badge)](https://dev.azure.com/RamseySolutions/Ramsey%20Plus/_artifacts/feed/RamseySolutions/NuGet/Bugsnag.Maui?preferRelease=true) \
Bugsnag.Android: [![Bugsnag.Android package in RamseySolutions feed in Azure Artifacts](https://feeds.dev.azure.com/RamseySolutions/_apis/public/Packaging/Feeds/RamseySolutions/Packages/de2b3b39-8806-4352-a224-f641ba3101d4/Badge)](https://dev.azure.com/RamseySolutions/Ramsey%20Plus/_artifacts/feed/RamseySolutions/NuGet/Bugsnag.Android?preferRelease=true) \
Bugsnag.iOS: [![Bugsnag.iOS package in RamseySolutions feed in Azure Artifacts](https://feeds.dev.azure.com/RamseySolutions/_apis/public/Packaging/Feeds/RamseySolutions/Packages/25bb06e4-dc77-4c6b-b70e-5e14375cf3a3/Badge)](https://dev.azure.com/RamseySolutions/Ramsey%20Plus/_artifacts/feed/RamseySolutions/NuGet/Bugsnag.iOS?preferRelease=true)

# bugsnag-maui
Binding library for the Bugsnag SDK It consists of a android and ios binding and a cross platform sdk to consume the bindings.

## Installation

Install the package from nuget

```bash
dotnet package add Bugsnag.Maui
dotnet package add Bugsnag.Android
dotnet package add Bugsnag.iOS
```

or add package reference to your project file

```xml

<ItemGroup>
    <PackageReference Include="Bugsnag.Maui" Version="<insert version>" />    
    <PackageReference Include="Bugsnag.Android" Version="<insert version>" />
    <PackageReference Include="Bugsnag.iOS" Version="<insert version>" />
</ItemGroup>
    
```

the native bindings follow the sdk version that they are bound to

## Building

### Android

*prereques*
your android envirnemnt must be configured. see dotnet maui [getting started](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?view=net-maui-8.0&tabs=visual-studio-code)
you also must have `ANDROID_HOME` configured

* ensure that the [.maven-sdk-version](.maven-sdk-version) is at the correct version
* open the solution
* build

### iOS

* open project
* build

note: the rake file does not correctly handle creating the .xcodeproj, in fact, all of the files that are supposed to be automatically copied are committed to the repo. There is going to be a little bit of manual work to bump the ios sdk to a new version. The other option is to finish building the scripts.
