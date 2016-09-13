#!/bin/bash
source ../Config-SdkVersion
source ../Config

#curl -o libs/SessionM.zip https://content.sessionm.com/1/56/3/5249589/SessionM_android_v1.9.14.zip
echo "Building SessionM-Unity-Android-Library"
sed -i '' 's/$(UNITY_SDK_VERSION)/'${UNITY_SDK_VERSION}'/g' src/com/sessionm/unity/BaseNativeActivity.java 
sed -e 's/$(UNITY_SDK_VERSION)/'${UNITY_SDK_VERSION}'/g' src/com/sessionm/unity/SessionMPlugin.java.in > src/com/sessionm/unity/SessionMPlugin.java

export ANDROID_HOME=${ANDROID_SDK_PATH}
./gradlew build -x lint
cp target/libs/android-sdk.jar target/SessionMUnity.jar
