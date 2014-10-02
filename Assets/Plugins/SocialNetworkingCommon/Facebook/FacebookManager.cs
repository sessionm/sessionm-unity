using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Prime31;


public class FacebookManager : AbstractManager
{
#if UNITY_ANDROID || UNITY_IPHONE
	// Fired after a successful login attempt was made
	public static event Action sessionOpenedEvent;

	// Fired just before the login succeeded event. For interal use only.
	public static event Action preLoginSucceededEvent;

	// Fired when an error occurs while logging in
	public static event Action<P31Error> loginFailedEvent;

	// Fired when a custom dialog completes with the url passed back from the dialog
	public static event Action<string> dialogCompletedWithUrlEvent;

	// Fired when the post message or custom dialog fails
	public static event Action<P31Error> dialogFailedEvent;

	// Fired when a graph request finishes
	public static event Action<object> graphRequestCompletedEvent;

	// Fired when a graph request fails
	public static event Action<P31Error> graphRequestFailedEvent;

	// iOS only. Fired when the Facebook composer completes. True indicates success and false cancel/failure.
	public static event Action<bool> facebookComposerCompletedEvent;

	// Fired when reauthorization succeeds
	public static event Action reauthorizationSucceededEvent;

	// Fired when reauthorization fails
	public static event Action<P31Error> reauthorizationFailedEvent;
	
	// Fired when the share dialog succeeds
	public static event Action<Dictionary<string,object>> shareDialogSucceededEvent;
	
	// Fired when the share dialog fails
	public static event Action<P31Error> shareDialogFailedEvent;



	static FacebookManager()
	{
		AbstractManager.initialize( typeof( FacebookManager ) );
	}



	public void sessionOpened( string accessToken )
	{
		preLoginSucceededEvent.fire();
		Facebook.instance.accessToken = accessToken;

		sessionOpenedEvent.fire();
	}


	public void loginFailed( string json )
	{
		loginFailedEvent.fire( P31Error.errorFromJson( json ) );
	}


	public void dialogCompletedWithUrl( string url )
	{
		dialogCompletedWithUrlEvent.fire( url );
	}


	public void dialogFailedWithError( string json )
	{
		dialogFailedEvent.fire( P31Error.errorFromJson( json ) );
	}


	public void graphRequestCompleted( string json )
	{
		if( graphRequestCompletedEvent != null )
		{
			object obj = Prime31.Json.decode( json );
			graphRequestCompletedEvent.fire( obj );
		}
	}


	public void graphRequestFailed( string json )
	{
		graphRequestFailedEvent.fire( P31Error.errorFromJson( json ) );
	}


	// iOS only
	public void facebookComposerCompleted( string result )
	{
		facebookComposerCompletedEvent.fire( result == "1" );
	}


	public void reauthorizationSucceeded( string empty )
	{
		reauthorizationSucceededEvent.fire();
	}


	public void reauthorizationFailed( string json )
	{
		reauthorizationFailedEvent.fire( P31Error.errorFromJson( json ) );
	}


	public void shareDialogFailed( string json )
	{
		shareDialogFailedEvent.fire( P31Error.errorFromJson( json ) );
	}


	public void shareDialogSucceeded( string json )
	{
		shareDialogSucceededEvent.fire( json.dictionaryFromJson() );
	}

#endif
}