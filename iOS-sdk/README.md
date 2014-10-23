SessionM Unity iOS Plugin

The SessionM-Unity Xcode project is provided for developers who want to make changes to the SessionM Unity iOS plugin source code, or those who just want to build the Unity plugin with an updated version of the SessionM iOS SDK.

Follow the steps below to build the iOS plugin:

- Run 'pod install' to update the SessionM iOS SDK to the latest version (refer to the CocoaPods Troubleshooting page at http://guides.cocoapods.org/using/troubleshooting.html for more information on using and install CocoaPods)
- Open SessionM-Unity/SessionM-Unity.xcworkspace and build the libSessionM-Unity.a static library
- Copy the libSessionM-Unity.a library to the following path in your Unity project directory: Assets/Plugins/iOS/libSessionM-Unity.a
