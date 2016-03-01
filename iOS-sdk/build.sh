#!/bin/bash
source ../Config-SdkVersion

echo "Installing latest SessionM podspec"
pod install
perl -pi -e 's/ -ObjC//g' 'Pods/Target Support Files/Pods/Pods.debug.xcconfig'
printf "\nVALID_ARCHS = armv7 armv7s arm64 i386 x86_64" >> "Pods/Target Support Files/Pods/Pods.debug.xcconfig"
perl -pi -e 's/ -ObjC//g' 'Pods/Target Support Files/Pods/Pods.release.xcconfig'
printf "\nVALID_ARCHS = armv7 armv7s arm64 i386 x86_64" >> "Pods/Target Support Files/Pods/Pods.release.xcconfig"
sed -e 's/$(UNITY_SDK_VERSION)/'${UNITY_SDK_VERSION}'/g' SessionM-Unity/SessionM_Unity.h.in > SessionM-Unity/SessionM_Unity.h

CONFIGURATION=Release
WORKSPACE=SessionM-Unity.xcworkspace
SCHEME=SessionM-Unity
INSTALL_PATH=build

rm -rf "${INSTALL_PATH}"
mkdir -p "${INSTALL_PATH}"

PRODUCTS_PATH=${INSTALL_PATH}/Build/Products/${CONFIGURATION}
LIBRARY_FILE_NAME=lib${SCHEME}.a
LIBRARY_FILE_PATH=${INSTALL_PATH}/${LIBRARY_FILE_NAME}

echo "Building ${SCHEME}"
xcodebuild -workspace ${WORKSPACE} -scheme ${SCHEME} -sdk iphoneos -configuration ${CONFIGURATION} -derivedDataPath ${INSTALL_PATH} > ${INSTALL_PATH}/device.log
xcodebuild -workspace ${WORKSPACE} -scheme ${SCHEME} -sdk iphonesimulator -configuration ${CONFIGURATION} ARCHS="i386 x86_64" PLATFORM_NAME="iphonesimulator" -derivedDataPath ${INSTALL_PATH} > ${INSTALL_PATH}/sim.log
lipo -create -output ${LIBRARY_FILE_PATH} ${PRODUCTS_PATH}-iphoneos/${LIBRARY_FILE_NAME} ${PRODUCTS_PATH}-iphonesimulator/${LIBRARY_FILE_NAME} > ${INSTALL_PATH}/lipo.log

echo "Build is under: ${LIBRARY_FILE_PATH}"
