#!/bin/bash

SDK_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
pushd $SDK_DIR

echo "Installing latest SessionM podspec"
pod install

CONFIGURATION=$1
[ $CONFIGURATION ] || CONFIGURATION=Release
INSTALL_PATH=build
[ -z $INSTALL_PATH ] || INSTALL_PATH=build
rm -rf "$INSTALL_PATH"
mkdir -p "$INSTALL_PATH"
WORKSPACE=SessionM-Unity.xcworkspace
SCHEME=SessionM-Unity
LIBRARY_FILE_NAME=lib${SCHEME}.a
echo "Building ${SCHEME}"

xcodebuild -workspace $WORKSPACE -scheme $SCHEME -sdk iphoneos -configuration ${CONFIGURATION} -derivedDataPath build > build/device.log
xcodebuild -workspace $WORKSPACE -scheme $SCHEME -sdk iphonesimulator -configuration ${CONFIGURATION} -derivedDataPath build > build/sim.log
lipo -create -output build/${LIBRARY_FILE_NAME} build/Products/${CONFIGURATION}-iphoneos/${LIBRARY_FILE_NAME} build/Products/${CONFIGURATION}-iphonesimulator/${LIBRARY_FILE_NAME} > build/lipo.log

echo "Build is under: build/$LIBRARY_FILE_NAME"

popd
