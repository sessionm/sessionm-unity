 SessionM Test App 
 
This app is designed to test all functionality in SessionM via Unity on an actual
device.

Both iOS and Android are supported.

Additionally, this app tests Prime31 compatibility.  This is important for iOS, but very
important for Android because previously, SessionM overrode P31's basenativeactivity.

In order to correctly build, you need to setup the various provisioning/keystore 
requirements.

iOS:
You need to set the bundle identifier to something your developer identifier can work with.
Additionally, you'll need to create an app profile and setup Achievements, Leaderboards, and IAP
correctly in iTunes Connect. 

Achievements:

Leaderboards:

IAP:


ANDROID:
For Android, you can use the existing keystore to build an APK for testing.  The keystore
password is:
sessionm

You need to check to make sure your Manifest is correct.  

SessionM Test Account:
U: SessionMTester092914@gmail.com
P: sessionm092914

As it's setup, we need to reset the account to do the testing each time, we should setup
a reset tool like this one in the future: https://github.com/playgameservices/management-tools/tree/master/tools

