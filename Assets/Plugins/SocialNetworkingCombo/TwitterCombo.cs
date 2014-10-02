using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_IPHONE || UNITY_ANDROID

#if UNITY_IPHONE
using TWITTER = TwitterBinding;
#else
using TWITTER = TwitterAndroid;
#endif

public static class TwitterCombo
{
	private static string _username;
	
	
	static TwitterCombo()
	{
		TwitterManager.loginSucceededEvent += username => { _username = username; };
	}

	
	// Initializes the Twitter plugin and sets up the required oAuth information
	public static void init( string consumerKey, string consumerSecret )
	{
		TWITTER.init( consumerKey, consumerSecret );
	}
	
	
	// Checks to see if there is a currently logged in user
	public static bool isLoggedIn()
	{
		return TWITTER.isLoggedIn();
	}
	
	
	// Retuns the currently logged in user's username
	public static string loggedInUsername()
	{
		return _username;
	}
	
	
	// Logs in the user using oAuth which request displaying the login prompt via an in app browser
	public static void showLoginDialog()
	{
		TWITTER.showLoginDialog();
	}
	
	
	// Logs out the current user
	public static void logout()
	{
		TWITTER.logout();
	}
	
	
	// Posts the status text.  Be sure status text is less than 140 characters!
	public static void postStatusUpdate( string status )
	{
		TWITTER.postStatusUpdate( status );
	}
	
	
	// Posts the status text and an image.  Note that the url will be appended onto the tweet so you don\'t have the full 140 characters
	public static void postStatusUpdate( string status, string pathToImage )
	{
#if UNITY_ANDROID
		TWITTER.postStatusUpdate( status, System.IO.File.ReadAllBytes( pathToImage ) );
#elif UNITY_IPHONE
		TWITTER.postStatusUpdate( status, pathToImage );
#endif
	}
	
	
	// Performs a request for any available Twitter API methods. methodType must be either "get" or "post".  path is the
	// url fragment from the API docs (excluding https://api.twitter.com) and parameters is a dictionary of key/value pairs
	// for the given method.  See Twitter's API docs for all available methods
	public static void performRequest( string methodType, string path, Dictionary<string,string> parameters )
	{
		TWITTER.performRequest( methodType, path, parameters );
	}

}
#endif