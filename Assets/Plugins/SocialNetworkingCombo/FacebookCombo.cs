using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_IPHONE || UNITY_ANDROID

#if UNITY_IPHONE
using FB = FacebookBinding;
#else
using FB = FacebookAndroid;
#endif


public static class FacebookCombo
{
	// Prepares the Facebook plugin for use
	public static void init()
	{
		FB.init();
	}


	// Gets the url used to launch the application. If no url was used returns string.Empty
	public static string getAppLaunchUrl()
	{
		return FB.getAppLaunchUrl();
	}


	// Authenticates the user with no additional permissions
	public static void login()
	{
		loginWithReadPermissions( new string[] {} );
	}


	// Authenticates the user for the provided permissions
	public static void loginWithReadPermissions( string[] permissions )
	{
		FB.loginWithReadPermissions( permissions );
	}


	// Reauthorizes with the requested read permissions
	public static void reauthorizeWithReadPermissions( string[] permissions )
	{
		FB.reauthorizeWithReadPermissions( permissions );
	}


	// Reauthorizes with the requested publish permissions and audience
	public static void reauthorizeWithPublishPermissions( string[] permissions, FacebookSessionDefaultAudience defaultAudience )
	{
		FB.reauthorizeWithPublishPermissions( permissions, defaultAudience );
	}


	// Checks to see if the current session is valid
	public static bool isSessionValid()
	{
		return FB.isSessionValid();
	}


	// Gets the current access token
	public static string getAccessToken()
	{
		return FB.getAccessToken();
	}


	// Gets the permissions granted to the current access token
	public static List<object> getSessionPermissions()
	{
		return FB.getSessionPermissions();
	}


	// Logs the user out and invalidates the token
	public static void logout()
	{
		FB.logout();
	}


	// Full access to any existing or new Facebook dialogs that get added. See Facebooks documentation for parameters and dialog types
	public static void showDialog( string dialogType, Dictionary<string,string> options )
	{
		FB.showDialog( dialogType, options );
	}


	// Shows the native Facebook Share Dialog. Valid dictionary keys (from FBShareDialogParams) are: link, name, caption, description, picture, friends (array)
	public static void showFacebookShareDialog( Dictionary<string,object> parameters )
	{
		FB.showFacebookShareDialog( parameters );
	}


	#region App Events

	// Sets the app version that Facebook will use for all events
	public static void setAppVersion( string version )
	{
		FB.setAppVersion( version );
	}


	// Logs an event with optional parameters
	public static void logEvent( string eventName, Dictionary<string,object> parameters = null )
	{
		FB.logEvent( eventName, parameters );
	}


	// Logs an event, valueToSum and optional parameters
	public static void logEvent( string eventName, double valueToSum, Dictionary<string,object> parameters = null )
	{
		FB.logEvent( eventName, valueToSum, parameters );
	}

	#endregion

}
#endif