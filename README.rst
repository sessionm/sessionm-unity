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

* Unity (Tested with Unity 5.1.0p1)
* XCode (Tested with XCode 6.3.1)
* CocoaPods (Tested with CocoaPods 0.37.2 - install the Ruby gem by running ``gem install cocoapods``)
* Android SDK (Tested with Android SDK version 21. Please add your Android SDK path in Config file and make sure you have Android SDK build tool version 21 downloaded. If you are familiar with Android development, feel free to change version number in ``android-sdk/build.gradle`` and ``android-sdk/AndroidManifest.xml``)
* SessionM iOS and Android SDKs. Available for downloading from: http://www.sessionm.com/documentation/downloads.php
* If you want to build Prime31 version of plugin, we assume that you have Prime31UnityActivity.jar ready. For more information on the Prime31 plugin, check their web site https://prime31.com, and instructions where to download the plugin can be found on their github account: https://gist.github.com/prime31/0908e6100d7e228f1add

2. Create a Config file based on the provided Config.example. 
        
3. Once you have finished editing your Config file, run 
   ``build.sh`` 
   in the top-level directory.

   The following files are created during the build process and can be found in the build directory:

* Prime31 version of SessionM Unity Plugin: ``build/SessionM-Prime31.unityPackage``
* BaseActivity version of SessionM Unity Plugin: ``build/SessionM-BaseActivity.unityPackage``
* The iOS Unity Plugin Library: ``iOS-sdk/build/libSessionM-Unity.a``
* The Android Unity Plugin Library: ``android-sdk/target/SessionMUnity.jar``

4. You can use 
   ``clean.sh`` 
   to clean the build.

Note:
We only tested this build script on Mac OS.
