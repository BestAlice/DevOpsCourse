FROM ubuntu:22.04

# Установка JDK, Android SDK и Gradle
RUN apt-get update && \
    apt-get install -y openjdk-11-jdk curl unzip && \
    curl -s "https://get.sdkman.io" | bash && \
    bash -c "source $HOME/.sdkman/bin/sdkman-init.sh && sdk install gradle 8.0"

# Копируем исходный код
WORKDIR /app
COPY . .

# Собираем APK
RUN gradle assembleDebug

# Можно скопировать APK из /app/app/build/outputs/apk/debug/