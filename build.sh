#!/bin/bash
source Config
DEST_PATH=${PWD}

rm -rf build/
mkdir build/

cd android-sdk
sh build.sh
cp libs/SessionM* target/SessionMUnity.jar ../Plugin_Shared/Assets/Plugins/Android
cd ..

cd iOS-sdk/SessionM-Unity
sh build.sh
cp build/libSessionM-Unity.a ../../Plugin_Shared/Assets/Plugins/iOS/
cd ../..

cp -r Plugin_Shared/ build/BaseActivity/
cp -r Plugin_Shared/ build/Prime31/

cp Plugin_BaseActivity/AndroidManifest.xml  build/BaseActivity/Assets/Plugins/Android
cp Plugin_BaseActivity/ISessionM_Android.cs build/BaseActivity/Assets/Plugins/SessionM

cp Plugin_Prime31/AndroidManifest.xml ${PRIME31_JAR_PATH} build/Prime31/Assets/Plugins/Android
cp Plugin_Prime31/ISessionM_Android.cs build/Prime31/Assets/Plugins/SessionM

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
echo "Done."
