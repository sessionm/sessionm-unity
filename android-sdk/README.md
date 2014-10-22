SessionM Unity Android Plugin

If you have other Android plugins installed, and cannot set Prime31’s BaseActivity as the LAUNCHER activity, you need to add our BaseActivity code to your custom LAUNCHER activity – which presumably inherits from either Unity's UnityBaseActivity or a third part class inheriting from it.

For instance, if you want to use com.facebook.unity.FBUnityPlayerActivity as your LAUNCHER activity, please create a new class that derives from this activity.

Basic steps would be:
- Add a new jar Library project in Eclipse or Android Studio if you don’t already have one.
- Add both Facebook Android SDK, and our SessionM.jar file to the project.
- If you have not already done so, create a new class inheriting from com.facebook.unity.FBUnityPlayerActivity, or optionally start from the class we created but be sure to name it with your organization name.
- Update this new class to include our code, which is shown in this folder.
- Build it as a new jar file library, which is the customized Unity plugin.
- Put the new jar file in Assets/Plugins/Android/ to replace the current SessionMUnity.jar file.
- In the AndroidManifest.xml, set YourBaseActivity as the basic LAUNCHER activity.

For more information on how to setup this custom class, see our detailed Android integration page: http://www.sessionm.com/documentation/android-integration.php#detailed-walkthough
