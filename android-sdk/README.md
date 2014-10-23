SessionM Unity Android Plugin

If you have other Android plugins installed, and cannot set Prime31’s BaseActivity as the LAUNCHER activity, you need to add our BaseActivity code to your custom LAUNCHER activity – which presumably inherits from either Unity's UnityBaseActivity or a third party class inheriting from it.

For instance, if you want to use com.facebook.unity.FBUnityPlayerActivity as your LAUNCHER activity, please create a new class that derives from this activity. We are providing the Gradle project here, so you can also build from our project.

Basic steps would be:
- Add a new jar Library project in Eclipse or Android Studio if you don’t already have one. You can also create the new project by simply cloning code from this repo. By default, we are supporting Android Studio, you can just clone this folder and open build.gradle with Android Studio.
- Add both Facebook android SDK, and our SessionM.jar file to the project libs/.
- If you have not already done so, create a new class inheriting from com.facebook.unity.FBUnityPlayerActivity, or optionally start from BaseActivity.
- Build it as a new jar file library, which is the customized Unity plugin. If you are using this project, simply run build.sh script. You will see SessionMUnity.jar file in current folder. You can also modify build.sh to name your own jar file.
- Put the new jar file in Assets/Plugins/Android/ to replace the current SessionMUnity.jar file.
- In the AndroidManifest.xml, set YourBaseActivity as the basic LAUNCHER activity.

For more information on how to setup this custom class, see our detailed Android integration page: http://www.sessionm.com/documentation/android-integration.php#detailed-walkthough
