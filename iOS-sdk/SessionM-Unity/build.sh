#!/bin/bash

echo "Installing latest SessionM podspec"
pod install

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
xcodebuild -workspace ${WORKSPACE} -scheme ${SCHEME} -sdk iphonesimulator -configuration ${CONFIGURATION} -derivedDataPath ${INSTALL_PATH} > ${INSTALL_PATH}/sim.log
lipo -create -output ${LIBRARY_FILE_PATH} ${PRODUCTS_PATH}-iphoneos/${LIBRARY_FILE_NAME} ${PRODUCTS_PATH}-iphonesimulator/${LIBRARY_FILE_NAME} > ${INSTALL_PATH}/lipo.log

echo "Build is under: ${LIBRARY_FILE_PATH}"
