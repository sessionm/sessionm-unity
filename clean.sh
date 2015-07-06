#!/bin/bash

rm -rf build/
rm -rf android-sdk/.gradle/
rm -rf android-sdk/target/
rm -f android-sdk/libs/SessionM.jar
rm -f android-sdk/libs/Unity.jar
rm -f android-sdk/src/com/sessionm/unity/SessionMPlugin.java
rm -rf iOS-sdk/build/
rm -rf iOS-sdk/Pods/
rm -f iOS-sdk/Podfile.lock
rm -f iOS-sdk/SessionM-Unity/SessionM_Unity.h
rm -f Plugin_Shared/Assets/Plugins/Android/SessionM.jar
rm -f Plugin_Shared/Assets/Plugins/Android/SessionMUnity.jar
rm -f Plugin_Shared/Assets/Plugins/iOS/libSessionM-Unity.a

echo "Done."
