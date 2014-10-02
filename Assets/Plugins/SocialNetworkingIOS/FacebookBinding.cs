using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;


#if UNITY_IPHONE
public enum FacebookSessionDefaultAudience
{
	None = 0,
	OnlyMe = 10,
	Friends = 20,
	Everyone = 30
}

public enum FacebookSessionLoginBehavior
{
	WithFallbackToWebView,
	WithNoFallbackToWebView,
	ForcingWebView,
	UseSystemAccountIfPresent
}


public class FacebookBinding
{
	static FacebookBinding()
	{
		// on login, set the access token
		FacebookManager.preLoginSucceededEvent += () =>
		{
			Facebook.instance.accessToken = getAccessToken();
		};
	}


	// internal doesn't really do anything here due to this being in the firstpass DLL but it does keep our test harness from touching it
	// you should never need to call this method. The plugin will call it for you when necessary
	internal static void babysitRequest( bool requiresPublishPermissions, Action afterAuthAction )
	{
		new FacebookAuthHelper( requiresPublishPermissions, afterAuthAction ).start();
	}


    [DllImport("__Internal")]
    private static extern void _facebookInit();

	// Initializes the Facebook plugin for your application
    public static void init()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookInit();

		// grab the access token in case it is saved
		Facebook.instance.accessToken = getAccessToken();
    }


	[DllImport("__Internal")]
	private static extern string _facebookGetAppLaunchUrl();

	// Gets the url used to launch the application. If no url was used returns string.Empty
	public static string getAppLaunchUrl()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _facebookGetAppLaunchUrl();

		return string.Empty;
	}


	[DllImport("__Internal")]
	private static extern void _facebookSetSessionLoginBehavior( int behavior );

	// Sets the login behavior. Must be called before any login calls! Understand what the login behaviors are and how they work before using this!
    public static void setSessionLoginBehavior( FacebookSessionLoginBehavior loginBehavior )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookSetSessionLoginBehavior( (int)loginBehavior );
    }


    [DllImport("__Internal")]
    private static extern void _facebookEnableFrictionlessRequests();

	// Enables frictionless requests
    public static void enableFrictionlessRequests()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookEnableFrictionlessRequests();
    }


    [DllImport("__Internal")]
    private static extern void _facebookRenewCredentialsForAllFacebookAccounts();

	// iOS 6 only. Renews the credentials that iOS stores for any native Facebook accounts. You can safely call this at app launch or when logging a user out.
    public static void renewCredentialsForAllFacebookAccounts()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookRenewCredentialsForAllFacebookAccounts();
    }


    [DllImport("__Internal")]
    private static extern bool _facebookIsLoggedIn();

	// Checks to see if the current session is valid
    public static bool isSessionValid()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _facebookIsLoggedIn();
		return false;
    }


	[DllImport("__Internal")]
	private static extern string _facebookGetFacebookAccessToken();

	// Gets the current access token
	public static string getAccessToken()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _facebookGetFacebookAccessToken();

		return string.Empty;
	}


	[DllImport("__Internal")]
	private static extern string _facebookGetSessionPermissions();

	// Gets the permissions granted to the current access token
	public static List<object> getSessionPermissions()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			var permissions = _facebookGetSessionPermissions();
			return permissions.listFromJson();
		}

		return new List<object>();
	}


    [DllImport("__Internal")]
    private static extern void _facebookLoginUsingDeprecatedAuthorizationFlowWithRequestedPermissions( string perms, string urlSchemeSuffix );

	// Shows the native authorization dialog (iOS 6), opens the Facebook single sign on login in Safari or the official Facebook app with the requested read (not publish!) permissions
	[System.Obsolete( "Note that this auth flow has been deprecated by Facebook and could be removed at any time at Facebook's discretion" )]
	public static void loginUsingDeprecatedAuthorizationFlowWithRequestedPermissions( string[] permissions )
	{
#pragma warning disable 0618
		loginUsingDeprecatedAuthorizationFlowWithRequestedPermissions( permissions, null );
#pragma warning restore 0618
	}

	[System.Obsolete( "Note that this auth flow has been deprecated by Facebook and could be removed at any time at Facebook's discretion" )]
    public static void loginUsingDeprecatedAuthorizationFlowWithRequestedPermissions( string[] permissions, string urlSchemeSuffix )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			var permissionsString = string.Join( ",", permissions );
			_facebookLoginUsingDeprecatedAuthorizationFlowWithRequestedPermissions( permissionsString, urlSchemeSuffix );
		}
    }


	// Opens the Facebook single sign on login in Safari, the official Facebook app or uses iOS 6 Accounts if available
    public static void login()
    {
        loginWithReadPermissions( new string[] {} );
    }

    public static void loginWithReadPermissions( string[] permissions )
    {
        loginWithReadPermissions( permissions, null );
    }


    [DllImport("__Internal")]
    private static extern void _facebookLoginWithRequestedPermissions( string perms, string urlSchemeSuffix );

	// Shows the native authorization dialog (iOS 6), opens the Facebook single sign on login in Safari or the official Facebook app with the requested read (not publish!) permissions
    public static void loginWithReadPermissions( string[] permissions, string urlSchemeSuffix )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			string permissionsString = null;
			if( permissions == null || permissions.Length == 0 )
				permissionsString = string.Empty;
			else
				permissionsString = string.Join( ",", permissions );

			_facebookLoginWithRequestedPermissions( permissionsString, urlSchemeSuffix );
		}
    }


    [DllImport("__Internal")]
    private static extern void _facebookReauthorizeWithReadPermissions( string perms );

	// Reauthorizes with the requested read permissions
    public static void reauthorizeWithReadPermissions( string[] permissions )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			var permissionsString = string.Join( ",", permissions );
			_facebookReauthorizeWithReadPermissions( permissionsString );
		}
    }


	[DllImport("__Internal")]
    private static extern void _facebookReauthorizeWithPublishPermissions( string perms, int defaultAudience );

	// Reauthorizes with the requested publish permissions and audience
    public static void reauthorizeWithPublishPermissions( string[] permissions, FacebookSessionDefaultAudience defaultAudience )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			var permissionsString = string.Join( ",", permissions );
			_facebookReauthorizeWithPublishPermissions( permissionsString, (int)defaultAudience );
		}
    }


    [DllImport("__Internal")]
    private static extern void _facebookLogout();

	// Logs the user out and invalidates the token
    public static void logout()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookLogout();

		Facebook.instance.accessToken = string.Empty;
    }


    [DllImport("__Internal")]
    private static extern void _facebookShowDialog( string dialogType, string json );

	// Full access to any existing or new Facebook dialogs that get added.  See Facebooks documentation for parameters and dialog types
    public static void showDialog( string dialogType, Dictionary<string,string> options )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			// if we arent logged in start up the babysitter
			if( !isSessionValid() )
			{
				babysitRequest( false, () => { _facebookShowDialog( dialogType, options.toJson() ); } );
			}
			else
			{
				_facebookShowDialog( dialogType, options.toJson() );
			}
		}
    }


    [DllImport("__Internal")]
    private static extern void _facebookGraphRequest( string graphPath, string httpMethod, string jsonDict );

	// Allows you to use any available Facebook Graph API method
    public static void graphRequest( string graphPath, string httpMethod, Dictionary<string,object> keyValueHash )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			// convert the Dictionary to JSON
			string jsonDict = keyValueHash.toJson();
			if( jsonDict != null )
			{
				// if we arent logged in start up the babysitter
				if( !isSessionValid() )
				{
					babysitRequest( true, () => { _facebookGraphRequest( graphPath, httpMethod, jsonDict ); } );
				}
				else
				{
					_facebookGraphRequest( graphPath, httpMethod, jsonDict );
				}
			}
 		}
	}


	#region iOS6 Facebook composer and Share Dialog

	[DllImport("__Internal")]
	private static extern bool _facebookCanUserUseFacebookComposer();

	// Checks to see if the user is using a version of iOS that supports the Facebook composer and if they have an account setup
	public static bool canUserUseFacebookComposer()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _facebookCanUserUseFacebookComposer();

		return false;
	}


	[DllImport("__Internal")]
	private static extern void _facebookShowFacebookComposer( string message, string imagePath, string link );

	public static void showFacebookComposer( string message )
	{
		showFacebookComposer( message, null, null );
	}


	// Shows the Facebook composer with optional image path and link
	public static void showFacebookComposer( string message, string imagePath, string link )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookShowFacebookComposer( message, imagePath, link );
	}


	[DllImport("__Internal")]
	private static extern void _facebookShowFacebookShareDialog( string json );

	// Shows the Facebook share dialog. Valid dictionary keys (from FBShareDialogParams) are: link, name, caption, description, picture, friends (array)
	public static void showFacebookShareDialog( Dictionary<string,object> parameters )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_facebookShowFacebookShareDialog( parameters.toJson() );
	}


	#endregion


	#region Facebook App Events

	[DllImport("__Internal")]
	private static extern void _facebookSetAppVersion( string version );

	// Sets the app version that Facebook will use for all events
	public static void setAppVersion( string version )
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;

		_facebookSetAppVersion( version );
	}


	[DllImport("__Internal")]
	private static extern void _facebookLogEvent( string eventName );

	[DllImport("__Internal")]
	private static extern void _facebookLogEventWithParameters( string eventName, string json );

	// Logs an event with optional parameters
	public static void logEvent( string eventName, Dictionary<string,object> parameters = null )
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;

		if( parameters != null )
			_facebookLogEventWithParameters( eventName, Json.encode( parameters ) );
		else
			_facebookLogEvent( eventName );
	}


	[DllImport("__Internal")]
	private static extern void _facebookLogEventAndValueToSum( string eventName, double valueToSum );

	[DllImport("__Internal")]
	private static extern void _facebookLogEventAndValueToSumWithParameters( string eventName, double valueToSum, string json );

	// Logs an event, valueToSum and optional parameters
	public static void logEvent( string eventName, double valueToSum, Dictionary<string,object> parameters = null )
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;

		if( parameters != null )
			_facebookLogEventAndValueToSumWithParameters( eventName, valueToSum, Json.encode( parameters ) );
		else
			_facebookLogEventAndValueToSum( eventName, valueToSum );
	}

	#endregion

}



#region AuthHelper babysitter

public class FacebookAuthHelper
{
	public Action afterAuthAction;
	public bool requiresPublishPermissions;

	#pragma warning disable
	private static FacebookAuthHelper _instance;
	#pragma warning restore


	public FacebookAuthHelper( bool requiresPublishPermissions, Action afterAuthAction )
	{
		_instance = this;
		this.requiresPublishPermissions = requiresPublishPermissions;
		this.afterAuthAction = afterAuthAction;

		// login
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;

		// reauth
		if( requiresPublishPermissions )
		{
			FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
			FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		}
	}


	~FacebookAuthHelper()
	{
		cleanup();
	}


	public void cleanup()
	{
		// if the afterAuthAction is not null we have not yet cleaned up
		if( afterAuthAction != null )
		{
			FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
			FacebookManager.loginFailedEvent -= loginFailedEvent;

			if( requiresPublishPermissions )
			{
				FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
				FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
			}
		}

		_instance = null;
	}


	public void start()
	{
		FacebookBinding.login();
	}


	#region Event handlers

	void sessionOpenedEvent()
	{
		// do we need publish permissions?
		if( requiresPublishPermissions && !FacebookBinding.getSessionPermissions().Contains( "publish_stream" ) )
		{
			FacebookBinding.reauthorizeWithPublishPermissions( new string[] { "publish_actions", "publish_stream" }, FacebookSessionDefaultAudience.Everyone );
		}
		else
		{
			afterAuthAction();
			cleanup();
		}
	}


	void loginFailedEvent( P31Error error )
	{
		cleanup();
	}


	void reauthorizationSucceededEvent()
	{
		afterAuthAction();
		cleanup();
	}


	void reauthorizationFailedEvent( P31Error error )
	{
		cleanup();
	}

	#endregion

}

#endregion

#endif