require "open3"
require "xcodeproj"
require "rbconfig"
require 'fileutils'
require 'tmpdir'
require "json"

HOST_OS = RbConfig::CONFIG['host_os']
def is_mac?; HOST_OS =~ /darwin/i; end
def is_windows?; HOST_OS =~ /mingw|mswin|windows/i; end

def current_directory
  File.dirname(__FILE__)
end

def maui_native_location
  File.join(current_directory, "native", "ios")
end

def assets_path
  File.join(current_directory, "Assets", "Bugsnag/Plugins")
end

def assemble_android filter_abis=true
  abi_filters = filter_abis ? "-PABI_FILTERS=armeabi-v7a,x86" : "-Pnoop_filters=true"
  android_dir = File.join(assets_path, "Android")

  Dir.chdir"bugsnag-android" do
    sh "./gradlew", "assembleRelease", abi_filters
  end

  Dir.chdir"bugsnag-android-unity" do
    sh "./gradlew", "assembleRelease", abi_filters
  end

  # copy each modularised bugsnag-android artefact
  android_core_lib = File.join("bugsnag-android", "bugsnag-android-core", "build", "outputs", "aar", "bugsnag-android-core-release.aar")
  anr_lib = File.join("bugsnag-android", "bugsnag-plugin-android-anr", "build", "outputs", "aar", "bugsnag-plugin-android-anr-release.aar")
  ndk_lib = File.join("bugsnag-android", "bugsnag-plugin-android-ndk", "build", "outputs", "aar", "bugsnag-plugin-android-ndk-release.aar")

  # copy kotlin dependencies required by bugsnag-android. the exact files required for each
  # version can be found here:
  # https://repo1.maven.org/maven2/org/jetbrains/kotlin/kotlin-stdlib/1.4.32/kotlin-stdlib-1.4.32.pom
  # The exact version number here should match the version in the EDM manifest in the BugsnagEditor.cs script and in the upm-tools/EDM/BugsnagAndroidDependencies.xml file.
  # All should be informed by what the android notifier is using
  kotlin_stdlib = File.join("android-libs", "org.jetbrains.kotlin.kotlin-stdlib-1.4.32.jar")
  kotlin_stdlib_common = File.join("android-libs", "org.jetbrains.kotlin.kotlin-stdlib-common-1.4.32.jar")
  kotlin_annotations = File.join("android-libs", "org.jetbrains.annotations-13.0.jar")

  # copy unity lib
  unity_lib = File.join("bugsnag-android-unity", "build", "outputs", "aar", "bugsnag-android-unity-release.aar")
  FileUtils.cp android_core_lib, File.join(android_dir, "bugsnag-android-release.aar")
  FileUtils.cp ndk_lib, File.join(android_dir, "bugsnag-plugin-android-ndk-release.aar")
  FileUtils.cp anr_lib, File.join(android_dir, "bugsnag-plugin-android-anr-release.aar")
  FileUtils.cp unity_lib, File.join(android_dir, "bugsnag-android-unity-release.aar")
  FileUtils.mkdir File.join(android_dir, "Kotlin")
  FileUtils.cp kotlin_stdlib, File.join(android_dir, "Kotlin/kotlin-stdlib.jar")
  FileUtils.cp kotlin_stdlib_common, File.join(android_dir, "Kotlin/kotlin-stdlib-common.jar")
  FileUtils.cp kotlin_annotations, File.join(android_dir, "Kotlin/kotlin-annotations.jar")

end

def get_current_version()
  version_line = File.open("build.sh").read.lines.find { |line| line.start_with?("VERSION=\"") }.strip
  return version_line.delete_prefix("VERSION=\"").delete_suffix("\"")
end

# Commit and tag the release
def update_package_git(package_dir)
  version = get_current_version()
  Dir.chdir(package_dir) do
    system("git add -A")
    system("git commit -m \"Release V#{version}\"")
    system("git push")
    if $?.exitstatus != 0
      throw "Cannot push."
    end
    system("git tag v#{version}")
    system("git push origin v#{version}")
    if $?.exitstatus != 0
      throw "Cannot push tag."
    end
  end
end

namespace :plugin do
  namespace :bind do
    cocoa_build_dir = "bugsnag-cocoa-build"
    if is_windows?
      task all: [:assets, :csharp]
      task all_android64: [:assets, :csharp]
    else
      task all: [:assets, :cocoa, :android, :csharp, ]
      task all_android64: [:assets, :cocoa, :android_64bit, :csharp ]
    end


    desc "Delete all build artifacts"
    task :clean do
      # remove any leftover artifacts from the package generation directory
      sh "git", "clean", "-dfx", "maui"
      # remove cocoa build area
      FileUtils.rm_rf cocoa_build_dir
      unless is_windows?
        # remove android build area
        Dir.chdir "./bugsnag-android" do
          sh "./gradlew", "clean"
        end

        Dir.chdir "bugsnag-android-unity" do
          sh "./gradlew", "clean"
        end
      end
    end

    task :cocoa do
      next unless is_mac?
      build_type = "Release" # "Debug" or "Release"
      FileUtils.mkdir_p cocoa_build_dir
      FileUtils.cp_r "bugsnag-cocoa/Bugsnag", cocoa_build_dir
      
      bugsnag_maui_header_file = File.realpath("BugsnagBinding.h", maui_native_location)
      bugsnag_maui_client_file = File.realpath("BugsnagBinding.m", maui_native_location)
      public_headers = Dir.entries(File.join(cocoa_build_dir, "Bugsnag", "include", "Bugsnag"))
      
      project_name = "bugsnag-ios"
      project_file = File.join("#{project_name}.xcodeproj")
    
      Dir.chdir cocoa_build_dir do
        project = Xcodeproj::Project.new(project_file)
    
        # Create platform-specific build targets, linking deps if needed.
        # Define TARGET_OS* macros since they aren't added for static targets.
        target = project.new_target(:framework, "BugsnagBinding", :ios, "9.0")
        target_macro = "-DTARGET_OS_IPHONE"
    
        # Link UIKit during compilation
        phase = target.build_phases.find { |p| p.is_a?(Xcodeproj::Project::PBXFrameworksBuildPhase) }
        target.add_system_frameworks("UIKit").each do |file_ref|
          phase.add_file_reference file_ref
        end
    
        group = project.new_group("BugsnagBinding")
    
        source_files = Dir.glob(File.join("Bugsnag", "**", "*.{c,h,mm,cpp,m}"))
                          .map(&File.method(:realpath))
                          .tap { |files| files << bugsnag_maui_header_file }
                          .tap { |files| files << bugsnag_maui_client_file }
                          .map { |f| group.new_file(f) }
    
        target.add_file_references(source_files) do |build_file|
          print "Adding #{build_file.file_ref.path} to #{project_name}\n"
          if public_headers.include? build_file.file_ref.name || File.basename(build_file.file_ref.path) == File.basename(bugsnag_maui_header_file)
            print "Setting #{build_file.file_ref.path} to public\n"
            build_file.settings = { "ATTRIBUTES" => ["Public"] }
          end
        end
    
        project.build_configurations.each do |build_configuration|
          build_configuration.build_settings["HEADER_SEARCH_PATHS"] = "$(SRCROOT)/Bugsnag/include/"
          build_configuration.build_settings["GENERATE_INFOPLIST_FILE"] = "YES"
          build_configuration.build_settings["ONLY_ACTIVE_ARCH"] = "NO"
          build_configuration.build_settings["VALID_ARCHS"] = ["x86_64", "armv7", "arm64"]
          build_configuration.build_settings["SWIFT_VERSION"] = "5.0" # Ensure Swift version is set
          build_configuration.build_settings["DEFINES_MODULE"] = "YES" # Ensure module is defined
          build_configuration.build_settings["CLANG_ENABLE_MODULES"] = "YES" # Enable modules
    
          case build_configuration.type
          when :debug
            build_configuration.build_settings["OTHER_CFLAGS"] = "-fembed-bitcode-marker #{target_macro}"
          when :release
            build_configuration.build_settings["OTHER_CFLAGS"] = "-fembed-bitcode #{target_macro}"
          end
        end
    
        project.save
    
        # Build the framework
        Open3.pipeline(["xcodebuild", "-project", "#{project_name}.xcodeproj", "-configuration", build_type, "-target", "BugsnagBinding", "build"], ["xcpretty"])
      end
    end
  end

  namespace :build do
    cocoa_build_dir = "bugsnag-cocoa-build"
    if is_windows?
      task all: [:assets, :csharp]
      task all_android64: [:assets, :csharp]
    else
      task all: [:assets, :cocoa, :android, :csharp, ]
      task all_android64: [:assets, :cocoa, :android_64bit, :csharp ]
    end


    desc "Delete all build artifacts"
    task :clean do
      # remove any leftover artifacts from the package generation directory
      sh "git", "clean", "-dfx", "maui"
      # remove cocoa build area
      FileUtils.rm_rf cocoa_build_dir
      unless is_windows?
        # remove android build area
        Dir.chdir "./bugsnag-android" do
          sh "./gradlew", "clean"
        end

        Dir.chdir "bugsnag-android-unity" do
          sh "./gradlew", "clean"
        end
      end
    end

    task :cocoa do
      next unless is_mac?
      build_type = "Release" # "Debug" or "Release"
      FileUtils.mkdir_p cocoa_build_dir
      FileUtils.cp_r "bugsnag-cocoa/Bugsnag", cocoa_build_dir
      bugsnag_maui_header_file = File.realpath("BugsnagBinding.h", maui_native_location)
      bugsnag_maui_client_file = File.realpath("BugsnagBinding.m", maui_native_location)
      public_headers = Dir.entries(File.join(cocoa_build_dir, "Bugsnag", "include", "Bugsnag"))

      Dir.chdir cocoa_build_dir do
        ["bugsnag-ios"].each do |project_name|
          project_file = File.join("#{project_name}.xcodeproj")          
          next if File.exist?(project_file)

          project = Xcodeproj::Project.new(project_file)

          # Create platform-specific build targets, linking deps if needed.
          # Define TARGET_OS* macros since they aren't added for static targets.
          case project_name
          when "bugsnag-tvos"
            target = project.new_target(:static_library, "bugsnag-tvos", :tvos, "9.2")
            target_macro = "-DTARGET_OS_TV"

            # Link UIKit during compilation
            phase = target.build_phases.find { |p| p.is_a?(Xcodeproj::Project::PBXFrameworksBuildPhase) }
            target.add_system_frameworks("UIKit").each do |file_ref|
              phase.add_file_reference file_ref
            end
          when "bugsnag-ios"
            target = project.new_target(:static_library, "bugsnag-ios", :ios, "9.0")
            target_macro = "-DTARGET_OS_IPHONE"

            # Link UIKit during compilation
            phase = target.build_phases.find { |p| p.is_a?(Xcodeproj::Project::PBXFrameworksBuildPhase) }
            target.add_system_frameworks("UIKit").each do |file_ref|
              phase.add_file_reference file_ref
            end
          when "bugsnag-osx"
            target = project.new_target(:bundle, "bugsnag-osx", :osx, "10.11")
            target_macro = "-DTARGET_OS_MAC"
          end

          group = project.new_group("Bugsnag")

          source_files = Dir.glob(File.join("Bugsnag", "**", "*.{c,h,mm,cpp,m}"))
                            .map(&File.method(:realpath))
                            .tap { |files| files << bugsnag_maui_header_file }
                            .tap { |files| files << bugsnag_maui_client_file }
                            .map { |f| group.new_file(f) }

          target.add_file_references(source_files) do |build_file|
            print "Adding #{build_file.file_ref.path} to #{project_name}\n"
            if File.basename(build_file.file_ref.path) == File.basename(bugsnag_maui_header_file)
              print "Setting #{build_file.file_ref.path} to public\n"
              build_file.settings = { "ATTRIBUTES" => ["Public"] }
            end
          end

          project.build_configurations.each do |build_configuration|
            build_configuration.build_settings["HEADER_SEARCH_PATHS"] = " $(SRCROOT)/Bugsnag/include/"

            build_configuration.build_settings["GENERATE_INFOPLIST_FILE"] = "YES"

            if ["bugsnag-ios", "bugsnag-tvos"].include? project_name
              build_configuration.build_settings["ONLY_ACTIVE_ARCH"] = "NO"
              build_configuration.build_settings["VALID_ARCHS"] = ["x86_64", "armv7", "arm64"]
            end
            case build_configuration.type
            when :debug
              build_configuration.build_settings["OTHER_CFLAGS"] = "-fembed-bitcode-marker #{target_macro}"
            when :release
              build_configuration.build_settings["OTHER_CFLAGS"] = "-fembed-bitcode #{target_macro}"
            end
          end

          project.save
          Open3.pipeline(["xcodebuild", "-project", "#{project_name}.xcodeproj", "-configuration", build_type, "build", "build"], ["xcpretty"])
          if project_name == "bugsnag-ios"
            Open3.pipeline(["xcodebuild", "-project", "#{project_name}.xcodeproj", "-configuration", build_type, "-sdk", "iphonesimulator", "build", "build"], ["xcpretty"])
          end
          if project_name == "bugsnag-tvos"
            Open3.pipeline(["xcodebuild", "-project", "#{project_name}.xcodeproj", "-configuration", build_type, "-sdk", "appletvsimulator", "build", "build"], ["xcpretty"])
          end
        end
      end

      osx_dir = File.join(assets_path, "OSX")

      ios_dir = File.join(assets_path, "iOS")

      tvos_dir = File.join(assets_path, "tvOS")

      #copy framework usage api file
      FileUtils.cp_r(File.join(current_directory,"bugsnag-cocoa", "Bugsnag", "resources", "PrivacyInfo.xcprivacy"), ios_dir)

      Dir.chdir cocoa_build_dir do
        Dir.chdir "build" do
          def is_fat library_path
            stdout, stderr, status = Open3.capture3("lipo", "-info", library_path)
            return !stdout.start_with?('Non-fat')
          end
          # we just need to copy the os x bundle into the correct directory

          FileUtils.cp_r(File.join(build_type, "bugsnag-osx.bundle"), osx_dir)

          # for ios and tvos we need to build a fat binary that includes architecture
          # slices for both the device and the simulator
          [["iphone", "ios", ios_dir], ["appletv", "tvos", tvos_dir]].each do |long_name, short_name, directory|
            library = "libbugsnag-#{short_name}.a"
            device_library = File.join("#{build_type}-#{long_name}os", library)
            simulator_dir = "#{build_type}-#{long_name}simulator"
            simulator_library = File.join(simulator_dir, library)
            simulator_x64 = File.join(simulator_dir, "libBugsnagStatic-x64.a")
            output_library = File.join(directory, library)

            if is_fat simulator_library
              sh "lipo", "-extract", "x86_64", simulator_library, "-output", simulator_x64
            else
              simulator_x64 = simulator_library
            end

            sh "lipo", "-create", device_library, simulator_x64, "-output", output_library

          end
        end
      end
    end

    task :android do
      assemble_android(true)
    end

    task :android_64bit do
      assemble_android(false)
    end
  end
end
