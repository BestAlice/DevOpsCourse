pluginManagement {
    repositories {
        google()             // обязательно для com.android.application
        mavenCentral()
        gradlePluginPortal()
    }
}

dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.PREFER_SETTINGS) // PREFER_SETTINGS чтобы избежать FAIL_ON_PROJECT_REPOS
    repositories {
        google()
        mavenCentral()
    }
}

rootProject.name = "DevOps"
include(":app")
