using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GPGSMultiplayerEventListener : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID

	void OnEnable()
	{
		// general events
		GPGMultiplayerManager.onInvitationReceivedEvent += onInvitationReceivedEvent;
		GPGMultiplayerManager.onInvitationRemovedEvent += onInvitationRemovedEvent;
		GPGMultiplayerManager.onWaitingRoomCompletedEvent += onWaitingRoomCompletedEvent;
		GPGMultiplayerManager.onInvitationInboxCompletedEvent += onInvitationInboxCompletedEvent;
		GPGMultiplayerManager.onInvitePlayersCompletedEvent += onInvitePlayersCompletedEvent;

		// room events
		GPGMultiplayerManager.onJoinedRoomEvent += onJoinedRoomEvent;
		GPGMultiplayerManager.onLeftRoomEvent += onLeftRoomEvent;
		GPGMultiplayerManager.onRoomConnectedEvent += onRoomConnectedEvent;
		GPGMultiplayerManager.onRoomCreatedEvent += onRoomCreatedEvent;

		GPGMultiplayerManager.onRealTimeMessageReceivedEvent += onRealTimeMessageReceivedEvent;

		// room update events
		GPGMultiplayerManager.onConnectedToRoomEvent += onConnectedToRoomEvent;
		GPGMultiplayerManager.onDisconnectedFromRoomEvent += onDisconnectedFromRoomEvent;
		GPGMultiplayerManager.onP2PConnectedEvent += onP2PConnectedEvent;
		GPGMultiplayerManager.onP2PDisconnectedEvent += onP2PDisconnectedEvent;
		GPGMultiplayerManager.onPeerDeclinedEvent += onPeerDeclinedEvent;
		GPGMultiplayerManager.onPeerInvitedToRoomEvent += onPeerInvitedToRoomEvent;
		GPGMultiplayerManager.onPeerJoinedEvent += onPeerJoinedEvent;
		GPGMultiplayerManager.onPeerLeftEvent += onPeerLeftEvent;
		GPGMultiplayerManager.onPeerConnectedEvent += onPeerConnectedEvent;
		GPGMultiplayerManager.onPeerDisconnectedEvent += onPeerDisconnectedEvent;
		GPGMultiplayerManager.onRoomAutoMatchingEvent += onRoomAutoMatchingEvent;
		GPGMultiplayerManager.onRoomConnectingEvent += onRoomConnectingEvent;
	}


	void OnDisable()
	{
		GPGMultiplayerManager.onInvitationReceivedEvent -= onInvitationReceivedEvent;
		GPGMultiplayerManager.onInvitationRemovedEvent -= onInvitationRemovedEvent;
		GPGMultiplayerManager.onWaitingRoomCompletedEvent -= onWaitingRoomCompletedEvent;
		GPGMultiplayerManager.onInvitationInboxCompletedEvent -= onInvitationInboxCompletedEvent;
		GPGMultiplayerManager.onInvitePlayersCompletedEvent -= onInvitePlayersCompletedEvent;

		GPGMultiplayerManager.onJoinedRoomEvent -= onJoinedRoomEvent;
		GPGMultiplayerManager.onLeftRoomEvent -= onLeftRoomEvent;
		GPGMultiplayerManager.onRoomConnectedEvent -= onRoomConnectedEvent;
		GPGMultiplayerManager.onRoomCreatedEvent -= onRoomCreatedEvent;

		GPGMultiplayerManager.onRealTimeMessageReceivedEvent -= onRealTimeMessageReceivedEvent;

		GPGMultiplayerManager.onConnectedToRoomEvent -= onConnectedToRoomEvent;
		GPGMultiplayerManager.onDisconnectedFromRoomEvent -= onDisconnectedFromRoomEvent;
		GPGMultiplayerManager.onP2PConnectedEvent -= onP2PConnectedEvent;
		GPGMultiplayerManager.onP2PDisconnectedEvent -= onP2PDisconnectedEvent;
		GPGMultiplayerManager.onPeerDeclinedEvent -= onPeerDeclinedEvent;
		GPGMultiplayerManager.onPeerInvitedToRoomEvent -= onPeerInvitedToRoomEvent;
		GPGMultiplayerManager.onPeerJoinedEvent -= onPeerJoinedEvent;
		GPGMultiplayerManager.onPeerLeftEvent -= onPeerLeftEvent;
		GPGMultiplayerManager.onPeerConnectedEvent -= onPeerConnectedEvent;
		GPGMultiplayerManager.onPeerDisconnectedEvent -= onPeerDisconnectedEvent;
		GPGMultiplayerManager.onRoomAutoMatchingEvent -= onRoomAutoMatchingEvent;
		GPGMultiplayerManager.onRoomConnectingEvent -= onRoomConnectingEvent;
	}



	void onInvitationReceivedEvent( string invitationId )
	{
		Debug.Log( "onInvitationReceivedEvent: " + invitationId );
	}


	void onInvitationRemovedEvent( string invitationId )
	{
		Debug.Log( "onInvitationRemovedEvent: " + invitationId );
	}


	void onWaitingRoomCompletedEvent( bool didSucceed )
	{
		Debug.Log( "onWaitingRoomCompletedEvent. didSucceed: " + didSucceed );
	}


	void onInvitationInboxCompletedEvent( bool didSucceed )
	{
		Debug.Log( "onInvitationInboxCompletedEvent. didSucceed: " + didSucceed );
	}


	void onInvitePlayersCompletedEvent( bool didSucceed )
	{
		Debug.Log( "onInvitePlayersCompletedEvent. didSucceed: " + didSucceed );
	}


	void onJoinedRoomEvent( GPGRoom room, GPGRoomUpdateStatus statusCode )
	{
		Debug.Log( "onJoinedRoomEvent. room: " + room + ", statusCode: " + statusCode );
	}


	void onLeftRoomEvent()
	{
		Debug.Log( "onLeftRoomEvent" );
	}


	void onRoomConnectedEvent( GPGRoom room, GPGRoomUpdateStatus statusCode )
	{
		Debug.Log( "onRoomConnectedEvent. room: " + room + ", statusCode: " + statusCode );
	}


	void onRoomCreatedEvent( GPGRoom room, GPGRoomUpdateStatus statusCode )
	{
		Debug.Log( "onRoomCreatedEvent. room: " + room + ", statusCode: " + statusCode );
	}


	void onRealTimeMessageReceivedEvent( string senderParticipantId, byte[] bytes )
	{
		Debug.Log( "onRealTimeMessageReceivedEvent. senderParticipantId: " + senderParticipantId + ", message length: " + bytes.Length );
	}


	void onConnectedToRoomEvent()
	{
		Debug.Log( "onConnectedToRoomEvent" );
	}


	void onDisconnectedFromRoomEvent()
	{
		Debug.Log( "onDisconnectedFromRoomEvent" );
	}


	void onP2PConnectedEvent( string participantId )
	{
		Debug.Log( "onP2PConnectedEvent: " + participantId );
	}


	void onP2PDisconnectedEvent( string participantId )
	{
		Debug.Log( "onP2PDisconnectedEvent: " + participantId );
	}


	void onPeerDeclinedEvent( string participantId )
	{
		Debug.Log( "onPeerDeclinedEvent: " + participantId );
	}


	void onPeerInvitedToRoomEvent( string participantId )
	{
		Debug.Log( "onPeerInvitedToRoomEvent: " + participantId );
	}


	void onPeerJoinedEvent( string participantId )
	{
		Debug.Log( "onPeerJoinedEvent: " + participantId );
	}


	void onPeerLeftEvent( string participantId )
	{
		Debug.Log( "onPeerLeftEvent: " + participantId );
	}


	void onPeerConnectedEvent( string participantId )
	{
		Debug.Log( "onPeerConnectedEvent: " + participantId );
	}


	void onPeerDisconnectedEvent( string participantId )
	{
		Debug.Log( "onPeerDisconnectedEvent: " + participantId );
	}


	void onRoomAutoMatchingEvent( GPGRoom room )
	{
		Debug.Log( "onRoomAutoMatchingEvent: " + room );
	}


	void onRoomConnectingEvent( GPGRoom room )
	{
		Debug.Log( "onRoomConnectingEvent: " + room );
	}

#endif
}


