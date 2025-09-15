plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("jacoco") // ← добавляем Jacoco
    id("org.jetbrains.kotlinx.kover") version "0.9.1"
    id("org.sonarqube") version "5.1.0.4882"
}

android {
    namespace = "com.example.devops"
    compileSdk = 34

    defaultConfig {
        applicationId = "com.example.devops"
        minSdk = 24
        targetSdk = 34
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
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

    testOptions {
        unitTests.isIncludeAndroidResources = true
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }

    kotlinOptions {
        jvmTarget = "17"
    }
}

kotlin {
    jvmToolchain(17)
}

dependencies {
    implementation("androidx.core:core-ktx:1.12.0")
    implementation("androidx.appcompat:appcompat:1.6.1")
    implementation("com.google.android.material:material:1.11.0")
    implementation("androidx.constraintlayout:constraintlayout:2.1.4")

    testImplementation("org.junit.jupiter:junit-jupiter:5.11.0")
    // Если нужно запускать старые JUnit4-тесты:
    testRuntimeOnly("org.junit.vintage:junit-vintage-engine:5.11.0")
    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")

    // --- тесты ---
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")

    // --- ★ Retrofit2 + конвертер JSON (Moshi или Gson) ---
    implementation("com.squareup.retrofit2:retrofit:2.9.0")
    implementation("com.squareup.retrofit2:converter-gson:2.9.0")
    // или, если предпочитаешь Moshi:
    // implementation("com.squareup.retrofit2:converter-moshi:2.9.0")

    // --- (необязательно) OkHttp для логирования ---
    implementation("com.squareup.okhttp3:logging-interceptor:4.12.0")
    testImplementation("com.squareup.okhttp3:mockwebserver:4.12.0")
    testImplementation("org.robolectric:robolectric:4.12.2")
}

jacoco {
    toolVersion = "0.8.10"
}

tasks.withType<Test> {
    useJUnitPlatform()
    finalizedBy("jacocoTestReport")
}

tasks.register<JacocoReport>("jacocoTestReport") {
    dependsOn("testDebugUnitTest")

    reports {
        xml.required.set(true)
        html.required.set(true)
    }

    val debugTree = fileTree("${buildDir}/tmp/kotlin-classes/debug")
    classDirectories.setFrom(debugTree)
    sourceDirectories.setFrom(files("src/main/java"))
    executionData.setFrom(fileTree(buildDir).include("jacoco/testDebugUnitTest.exec"))
}

val jacocoTestReportOutput by tasks.registering(JacocoReport::class) {
    dependsOn(tasks.withType<Test>())
    reports {
        xml.required.set(true)
        html.required.set(true)
    }
    // при необходимости укажите sourceDirectories, classDirectories, executionData
}

sonarqube {
    properties {
        property("sonar.coverage.jacoco.xmlReportPaths",
            "${project.buildDir}/reports/jacoco/test/jacocoTestReport.xml")
    }
}

sonar {
    properties {
        property("sonar.projectKey", "DevOpsClient")
        property("sonar.qualitygate.wait", true)
    }
}

tasks.named("sonarqube") { dependsOn(jacocoTestReportOutput) }


tasks.withType<Test>().configureEach {
    testLogging {
        events("failed", "skipped", "passed", "standardOut", "standardError")
        showExceptions = true
        exceptionFormat = org.gradle.api.tasks.testing.logging.TestExceptionFormat.FULL
        showCauses = true
        showStackTraces = true
    }
}