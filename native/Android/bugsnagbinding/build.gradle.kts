plugins {
    id("com.android.library")
}

android {
    namespace = "com.bugsnag.maui"
    compileSdk = 34

    defaultConfig {
        minSdk = 21
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
}

// Read version from file
val sdkVersion = File(rootDir.parentFile, "../.maven-sdk-version").readText().trim()

// Create configuration for copyDependencies
configurations {
    create("copyDependencies")
}

dependencies {
    implementation("com.bugsnag:bugsnag-android:$sdkVersion")
    implementation("com.bugsnag:bugsnag-plugin-android-exitinfo:$sdkVersion")
}

// Copy dependencies for binding library
project.afterEvaluate {
    tasks.register<Copy>("copyDeps") {
        from(configurations["copyDependencies"])
        into("${buildDir}/outputs/deps")
    }
    tasks.named("preBuild") { finalizedBy("copyDeps") }
}
