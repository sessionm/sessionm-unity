using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Prime31;


public class TwitterManager : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	// Fired after a successful login attempt was made. Provides the screenname of the user.
	public static event Action<string> loginSucceededEvent;
	
	// Fired when an error occurs while logging in
	public static event Action<string> loginFailedEvent;
	
	// Fired when a custom request completes. The object will be either an IList or an IDictionary
	public static event Action<object> requestDidFinishEvent;
	
	// Fired when a custom request fails
	public static event Action<string> requestDidFailEvent;
	
	// iOS only. Fired when the tweet sheet completes. True indicates success and false cancel/failure.
	public static event Action<bool> tweetSheetCompletedEvent;
	
	

	static TwitterManager()
	{
		AbstractManager.initialize( typeof( TwitterManager ) );
	}
	
	
	public static void noop() {}
	
	
	public void loginSucceeded( string screenname )
	{
		if( loginSucceededEvent != null )
			loginSucceededEvent( screenname );
	}
	
	
	public void loginFailed( string error )
	{
		if( loginFailedEvent != null )
			loginFailedEvent( error );
	}

	
	public void requestSucceeded( string results )
	{
		if( requestDidFinishEvent != null )
			requestDidFinishEvent( Json.decode( results ) );
	}
	
	
	public void requestFailed( string error )
	{
		if( requestDidFailEvent != null )
			requestDidFailEvent( error );
	}
	
	
	public void tweetSheetCompleted( string oneOrZero )
	{
		if( tweetSheetCompletedEvent != null )
			tweetSheetCompletedEvent( oneOrZero == "1" );
	}

#endif
}
