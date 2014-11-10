#!/bin/bash
source ../Config-SdkVersion

#curl -o libs/SessionM.zip https://content.sessionm.com/1/56/3/5249589/SessionM_android_v1.9.14.zip
echo "Building SessionM-Unity-Android-Library"
sed -e 's/$(UNITY_SDK_VERSION)/'${UNITY_SDK_VERSION}'/g' src/com/sessionm/unity/SessionMPlugin.java.in > src/com/sessionm/unity/SessionMPlugin.java
./gradlew build
cp target/libs/android-sdk.jar target/SessionMUnity.jar
