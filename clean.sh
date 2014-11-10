#!/bin/bash

rm -rf build/
rm -rf android-sdk/target/
rm -rf iOS-sdk/build/
rm -rf iOS-sdk/Pods/
rm -f iOS-sdk/Podfile.lock
rm -f iOS-sdk/SessionM-Unity/SessionM_Unity.h
rm -rf Plugin_Shared/Assets/Plugins/Android/
rm -rf Plugin_Shared/Assets/Plugins/iOS/

echo "Done."
