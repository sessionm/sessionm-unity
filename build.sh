#!/bin/bash

set -x 

source Config
source Config-SdkVersion
DEST_PATH=${PWD}

rm -rf build/
mkdir build/

##Build Android Plugin SDK##
cp ${SESSIONM_ANDROID_JAR_PATH} android-sdk/libs/SessionM.jar
cp ${UNITY_JAR_PATH} android-sdk/libs/Unity.jar
cd android-sdk
sh build.sh
cp libs/SessionM.jar target/SessionMUnity.jar ../Plugin_Shared/Assets/Plugins/Android
cd ..

##Build Unity Plugin##
cp -r Plugin_Shared/ build/BaseActivity/

cp Plugin_BaseActivity/AndroidManifest.xml build/BaseActivity/Assets/Plugins/Android
cp Plugin_BaseActivity/AndroidManifest.xml.meta build/BaseActivity/Assets/Plugins/Android
cp Plugin_BaseActivity/ISessionM_Android.cs build/BaseActivity/Assets/Plugins/SessionM
cp Plugin_BaseActivity/ISessionM_Android.cs.meta build/BaseActivity/Assets/Plugins/SessionM

cd build/BaseActivity
echo "Building SessionM Unity BaseActivity Plugin..."
echo
rm /tmp/x
$UNITY_PATH -batchmode -exportPackage Assets/Plugins Assets/Editor SessionM-BaseActivity.unityPackage -projectPath "$DEST_PATH/build/BaseActivity" -logFile /tmp/x -quit
cp SessionM-BaseActivity.unityPackage ../
cd ..

pwd
cd ..
pwd

cp Plugin_Shared/Assets/Plugins/SessionM/SessionMEventListener.cs SampleApp/Assets/Plugins/SessionM/SessionMEventListener.cs
cp Plugin_Shared/Assets/Plugins/SessionM/Example/SessionMListenerExample.cs SampleApp/Assets/Plugins/SessionM/Example/SessionMListenerExample.cs

rm -rf BaseActivity/ Prime31/

# ~/Library/Android/sdk/platform-tools/adb logcat
