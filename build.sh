#!/bin/bash
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

##Build iOS Plugin SDK##
cd iOS-sdk
sh build.sh
cp build/libSessionM-Unity.a ../Plugin_Shared/Assets/Plugins/iOS/
cd ..

##Build Unity Plugin##
cp -r Plugin_Shared/ build/BaseActivity/
cp -r Plugin_Shared/ build/Prime31/

cp Plugin_BaseActivity/AndroidManifest.xml build/BaseActivity/Assets/Plugins/Android
cp Plugin_BaseActivity/AndroidManifest.xml.meta build/BaseActivity/Assets/Plugins/Android
cp Plugin_BaseActivity/ISessionM_Android.cs build/BaseActivity/Assets/Plugins/SessionM
cp Plugin_BaseActivity/ISessionM_Android.cs.meta build/BaseActivity/Assets/Plugins/SessionM

cp Plugin_Prime31/AndroidManifest.xml ${PRIME31_JAR_PATH} build/Prime31/Assets/Plugins/Android
cp Plugin_Prime31/AndroidManifest.xml.meta build/Prime31/Assets/Plugins/Android
cp Plugin_Prime31/Prime31UnityActivity.jar.meta build/Prime31/Assets/Plugins/Android
cp Plugin_Prime31/ISessionM_Android.cs build/Prime31/Assets/Plugins/SessionM
cp Plugin_Prime31/ISessionM_Android.cs.meta build/Prime31/Assets/Plugins/SessionM

cd build/BaseActivity
echo "Building SessionM Unity BaseActivity Plugin..."
echo
$UNITY_PATH -batchmode -exportPackage Assets/Plugins Assets/Editor SessionM-BaseActivity.unityPackage -projectPath "$DEST_PATH/build/BaseActivity" -quit
cp SessionM-BaseActivity.unityPackage ../

cd ../Prime31
echo "Building SessionM Unity Prime31 Plugin..."
echo
$UNITY_PATH -batchmode -exportPackage Assets/Plugins Assets/Editor SessionM-Prime31.unityPackage -projectPath "$DEST_PATH/build/Prime31" -quit
cp SessionM-Prime31.unityPackage ../

cd ..

rm -rf BaseActivity/ Prime31/
zip -9 -r "SessionM_Unity_v${UNITY_SDK_VERSION}.zip" * ../ReleaseNotes.txt
echo "Done."
