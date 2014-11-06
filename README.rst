======================
SessionM for Unity 
======================

Unity Plugin for the SessionM SDKs (iOS & Android).
License: MIT

#####
Usage
#####

Please see  http://www.sessionm.com/documentation/adobeair-integration.php

Full online API documentation is also available at  http://devdoc.sessionm.com/adobeair/index.html

##################
Build dependencies
##################

In order to build this extension, the following programs and files must be available:

* Unity (Tested with Unity 4.5)
* XCode (Tested on XCode 6.0.0)
* Android SDK (can be specified with ``android.sdk`` Java options)
* SessionM iOS and Android SDKs. Available for download from: http://www.sessionm.com/documentation/downloads.php
* If you want to build Prime31 version of plugin, we assume that you have Prime31UnityActivity.jar ready.

Create a Config file based on the provided Config.example. 
        
Once you have finished editing your Config file, running ``build.sh`` in the top-level directory will reflect the changes made to settings.

The following files are created during the build process and can be found in the build directory:

* Prime31 version of SessionM Unity Plugin: build/SessionM-Prime31.unityPackage
* BaseActivity version of SessionM Unity Plugin: build/SessionM-BaseActivity.unityPackage
* The iOS Unity Plugin Library: iOS-sdk/build/libSessionM-Unity.a
* The Android Unity Plugin Library: android-sdk/target/SessionMUnity.jar

Note:
We only tested this build script on Mac OS.
