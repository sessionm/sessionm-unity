======================
SessionM for Unity 
======================

Unity Plugin for the SessionM SDKs (iOS & Android).
License: MIT

#####
Usage
#####

Please see http://www.sessionm.com/documentation/unity-integration.php 

###########
Build Steps 
###########

1. In order to build this extension, the following programs and files must be available:

* Unity (Tested with Unity 4.5)
* XCode (Tested with XCode 6.0.0)
* Android SDK (Tested with Android SDK version 19)
* SessionM iOS and Android SDKs. Available for download from: http://www.sessionm.com/documentation/downloads.php
* If you want to build Prime31 version of plugin, we assume that you have Prime31UnityActivity.jar ready. Otherwise, it can be downloaded from: https://app.box.com/s/xw6hq1ltjaniycc14j21

2. Create a Config file based on the provided Config.example. 
        
3. Once you have finished editing your Config file, run 
   ``build.sh`` 
   in the top-level directory.

    The following files are created during the build process and can be found in the build directory:

* Prime31 version of SessionM Unity Plugin: build/SessionM-Prime31.unityPackage
* BaseActivity version of SessionM Unity Plugin: build/SessionM-BaseActivity.unityPackage
* The iOS Unity Plugin Library: iOS-sdk/build/libSessionM-Unity.a
* The Android Unity Plugin Library: android-sdk/target/SessionMUnity.jar

4. You can use 
   ``clean.sh`` 
   to clean the build.

Note:
We only tested this build script on Mac OS.
