using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class FacebookEventListener : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;
		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedWithUrlEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;

		FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;

		FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;

		FacebookManager.shareDialogFailedEvent += shareDialogFailedEvent;
		FacebookManager.shareDialogSucceededEvent += shareDialogSucceededEvent;
	}


	void OnDisable()
	{
		// Remove all the event handlers when disabled
		FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManager.loginFailedEvent -= loginFailedEvent;
		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedWithUrlEvent;
		FacebookManager.dialogFailedEvent -= dialogFailedEvent;

		FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;

		FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;

		FacebookManager.shareDialogFailedEvent -= shareDialogFailedEvent;
		FacebookManager.shareDialogSucceededEvent -= shareDialogSucceededEvent;
	}



	void sessionOpenedEvent()
	{
		Debug.Log( "Successfully logged in to Facebook" );
	}


	void loginFailedEvent( P31Error error )
	{
		Debug.Log( "Facebook login failed: " + error );
	}
	
	
	void dialogCompletedWithUrlEvent( string url )
	{
		Debug.Log( "dialogCompletedWithUrlEvent: " + url );
	}


	void dialogFailedEvent( P31Error error )
	{
		Debug.Log( "dialogFailedEvent: " + error );
	}


	void facebokDialogCompleted()
	{
		Debug.Log( "facebokDialogCompleted" );
	}


	void graphRequestCompletedEvent( object obj )
	{
		Debug.Log( "graphRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}


	void facebookCustomRequestFailed( P31Error error )
	{
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
	}


	void facebookComposerCompletedEvent( bool didSucceed )
	{
		Debug.Log( "facebookComposerCompletedEvent did succeed: " + didSucceed );
	}


	void reauthorizationSucceededEvent()
	{
		Debug.Log( "reauthorizationSucceededEvent" );
	}


	void reauthorizationFailedEvent( P31Error error )
	{
		Debug.Log( "reauthorizationFailedEvent: " + error );
	}


	void shareDialogFailedEvent( P31Error error )
	{
		Debug.Log( "shareDialogFailedEvent: " + error );
	}


	void shareDialogSucceededEvent( Dictionary<string,object> dict )
	{
		Debug.Log( "shareDialogSucceededEvent" );
		Prime31.Utils.logObject( dict );
	}

#endif
}
