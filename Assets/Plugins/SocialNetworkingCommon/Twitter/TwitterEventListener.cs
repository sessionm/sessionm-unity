using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TwitterEventListener : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		TwitterManager.loginSucceededEvent += loginSucceeded;
		TwitterManager.loginFailedEvent += loginFailed;
		TwitterManager.requestDidFinishEvent += requestDidFinishEvent;
		TwitterManager.requestDidFailEvent += requestDidFailEvent;
		TwitterManager.tweetSheetCompletedEvent += tweetSheetCompletedEvent;
	}

	
	void OnDisable()
	{
		// Remove all the event handlers
		// Twitter
		TwitterManager.loginSucceededEvent -= loginSucceeded;
		TwitterManager.loginFailedEvent -= loginFailed;
		TwitterManager.requestDidFinishEvent -= requestDidFinishEvent;
		TwitterManager.requestDidFailEvent -= requestDidFailEvent;
		TwitterManager.tweetSheetCompletedEvent -= tweetSheetCompletedEvent;
	}
	
	
	// Twitter events
	void loginSucceeded( string username )
	{
		Debug.Log( "Successfully logged in to Twitter: " + username );
	}
	
	
	void loginFailed( string error )
	{
		Debug.Log( "Twitter login failed: " + error );
	}

	
	void requestDidFailEvent( string error )
	{
		Debug.Log( "requestDidFailEvent: " + error );
	}
	
	
	void requestDidFinishEvent( object result )
	{
		if( result != null )
		{
			Debug.Log( "requestDidFinishEvent" );
			Prime31.Utils.logObject( result );
		}
	}
	
	
	void tweetSheetCompletedEvent( bool didSucceed )
	{
		Debug.Log( "tweetSheetCompletedEvent didSucceed: " + didSucceed );
	}
	
#endif
}
