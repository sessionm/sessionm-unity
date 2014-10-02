using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class GPGTBMultiplayerEventListener : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID

	void OnEnable()
	{
		GPGTurnBasedManager.matchChangedEvent += matchChangedEvent;
		GPGTurnBasedManager.matchFailedEvent += matchFailedEvent;
		GPGTurnBasedManager.matchEndedEvent += matchEndedEvent;
		GPGTurnBasedManager.playerSelectorCanceledEvent += playerSelectorCanceledEvent;
		GPGTurnBasedManager.loadMatchesCompletedEvent += loadMatchesCompletedEvent;
		GPGTurnBasedManager.takeTurnCompletedEvent += takeTurnCompletedEvent;
		GPGTurnBasedManager.finishMatchCompletedEvent += finishMatchCompletedEvent;
		GPGTurnBasedManager.dismissMatchCompletedEvent += dismissMatchCompletedEvent;
		GPGTurnBasedManager.leaveDuringTurnCompletedEvent += leaveDuringTurnCompletedEvent;
		GPGTurnBasedManager.leaveOutOfTurnCompletedEvent += leaveOutOfTurnCompletedEvent;
		GPGTurnBasedManager.invitationReceivedEvent += invitationReceivedEvent;
	}


	void OnDisable()
	{
		GPGTurnBasedManager.matchChangedEvent -= matchChangedEvent;
		GPGTurnBasedManager.matchFailedEvent -= matchFailedEvent;
		GPGTurnBasedManager.matchEndedEvent -= matchEndedEvent;
		GPGTurnBasedManager.playerSelectorCanceledEvent -= playerSelectorCanceledEvent;
		GPGTurnBasedManager.loadMatchesCompletedEvent -= loadMatchesCompletedEvent;
		GPGTurnBasedManager.takeTurnCompletedEvent -= takeTurnCompletedEvent;
		GPGTurnBasedManager.finishMatchCompletedEvent -= finishMatchCompletedEvent;
		GPGTurnBasedManager.dismissMatchCompletedEvent -= dismissMatchCompletedEvent;
		GPGTurnBasedManager.leaveDuringTurnCompletedEvent -= leaveDuringTurnCompletedEvent;
		GPGTurnBasedManager.leaveOutOfTurnCompletedEvent -= leaveOutOfTurnCompletedEvent;
		GPGTurnBasedManager.invitationReceivedEvent -= invitationReceivedEvent;
	}


	void matchChangedEvent( GPGTurnBasedMatch match )
	{
		Debug.Log( "matchChangedEvent" );
		Debug.Log( match );
	}


	void matchFailedEvent( string error )
	{
		Debug.Log( "matchFailedEvent: " + error );
	}


	void matchEndedEvent( GPGTurnBasedMatch match )
	{
		Debug.Log( "matchEndedEvent" );
		Debug.Log( match );
	}


	void playerSelectorCanceledEvent()
	{
		Debug.Log( "playerSelectorCanceledEvent" );
	}


	void loadMatchesCompletedEvent( bool didSucceed, string error, List<GPGTurnBasedMatch> matches )
	{
		if( didSucceed )
		{
			Debug.Log( "loadMatchesCompletedEvent" );
			Utils.logObject( matches );
		}
		else
		{
			Debug.Log( "loadMatchesCompletedEvent. didSucceed: " + didSucceed + ", error: " + error );
		}
	}


	void takeTurnCompletedEvent( bool didSucceed, string error )
	{
		Debug.Log( "takeTurnCompletedEvent. didSucceed: " + didSucceed + ", error: " + error );
	}


	void finishMatchCompletedEvent( bool didSucceed, string error )
	{
		Debug.Log( "finishMatchCompletedEvent. didSucceed: " + didSucceed + ", error: " + error );
	}


	void dismissMatchCompletedEvent( bool didSucceed, string error )
	{
		Debug.Log( "dismissMatchCompletedEvent. didSucceed: " + didSucceed + ", error: " + error );
	}


	void leaveDuringTurnCompletedEvent( bool didSucceed, string error )
	{
		Debug.Log( "leaveDuringTurnCompletedEvent. didSucceed: " + didSucceed + ", error: " + error );
	}


	void leaveOutOfTurnCompletedEvent( bool didSucceed, string error )
	{
		Debug.Log( "leaveOutOfTurnCompletedEvent. didSucceed: " + didSucceed + ", error: " + error );
	}


	void invitationReceivedEvent( GPGTurnBasedInvitation invitation )
	{
		Debug.Log( "invitationReceivedEvent" );
		Debug.Log( invitation );
	}

#endif
}
