#!/bin/bash
source Config
rm -rf build/
mkdir build/

cd android-sdk
sh build.sh
cd ..

cp -r Plugin_Shared/ build/BaseActivity/
cp -r Plugin_Shared/ build/Prime31/
cp Plugin_BaseActivity/AndroidManifest.xml android-sdk/libs/SessionM.${ANDROID_SDK_VERSION}.jar android-sdk/target/SessionMUnity.jar build/BaseActivity/Assets/Plugins/Android
cp Plugin_BaseActivity/ISessionM_Android.cs build/BaseActivity/Assets/Plugins/SessionM/ISessionM_Android.cs
cp Plugin_Prime31/AndroidManifest.xml android-sdk/libs/SessionM.${ANDROID_SDK_VERSION}.jar android-sdk/target/SessionMUnity.jar ${PRIME31_JAR_PATH} build/Prime31/Assets/Plugins/Android
cp Plugin_Prime31/ISessionM_Android.cs build/Prime31/Assets/Plugins/SessionM/ISessionM_Android.cs

cd build/BaseActivity
$UNITY_PATH -batchmode -exportPackage Assets/Plugins Assets/Editor SessionM-BaseActivity.unityPackage -projectPath "$DEST_PATH/build/BaseActivity" -quit
cp SessionM-BaseActivity.unityPackage ../

cd ../Prime31
$UNITY_PATH -batchmode -exportPackage Assets/Plugins Assets/Editor SessionM-Prime31.unityPackage -projectPath "$DEST_PATH/build/Prime31" -quit
cp SessionM-Prime31.unityPackage ../

cd ..

rm -rf BaseActivity/ Prime31/
