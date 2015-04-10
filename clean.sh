#!/bin/bash

rm -rf build/
rm -f android-sdk/src/com/sessionm/unity/SessionMPlugin.java
rm -rf android-sdk/target/
rm -rf iOS-sdk/build/
rm -rf iOS-sdk/Pods/
rm -f iOS-sdk/Podfile.lock
rm -f iOS-sdk/SessionM-Unity/SessionM_Unity.h

echo "Done."
