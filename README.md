# bugsnag-maui
Binding library for the MasterCard Connect SDK It consists of a android and ios binding and a cross platform sdk to consume the bindings.

## Installation

Install the package from nuget

```bash
dotnet package add Bugsnag.Maui
```

or add package reference to your project file

```xml

<ItemGroup>
    <PackageReference Include="Bugsnag.Maui" Version="3.0.3" />
</ItemGroup>
    
```

package versioning is based on the version of the MasterCard Connect SDK that is being bound.

## Binding Library
below are some helpful links to get started with binding libraries

* https://devblogs.microsoft.com/dotnet/native-library-interop-dotnet-maui/
* https://github.com/CommunityToolkit/Maui.NativeLibraryInterop/tree/main
* https://debruyn.dev/2016/creating-a-xamarin.ios-binding-project-for-dummies/
* https://jimbobbennett.dev/blogs/binding-ios-libraries-in-xamarin/

One of the big gotchas with the MasterConnect SDK is that on ios 