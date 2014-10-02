using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prime31;


/// <summary>
/// This demo scene is setup to show how to get a turn-based multiplayer game started in a variety of ways:
/// via received invitation, programmatically or via the player selector. It has been simplified as much as possible
/// in order to make it as easy as possible to read and understand.
///
/// The demo defines an enum (TurnBasedDemoState) in an effort to break up the actions that can be accomplished at
/// any given time. Depending on the enum's value a different GUI will be shown. For simplicity, the act of taking
/// a turn is just appending a random letter onto a string and passing that on to the next player.
/// </summary>
public class GPGTurnBasedMultiplayerUI : MonoBehaviourGUI
{
#if UNITY_IPHONE || UNITY_ANDROID

	internal enum TurnBasedDemoState
	{
		// the user has not yet authenticated
		NotAuthenticated,

		// we have an authenticated user but there is no loaded match yet
		AuthenticatedWithNoMatchLoaded,

		// we have an authenticated user and a match with Active status is loaded
		AuthenticatedWithActiveMatch,

		// we have an authenticated user and an inactive match is loaded
		AuthenticatedWithInactiveMatch
	}

	private TurnBasedDemoState _demoState = TurnBasedDemoState.NotAuthenticated;
	private GPGTurnBasedMatch _currentMatch;
	private GPGTurnBasedMatch currentMatch
	{
		get { return _currentMatch; }
		set
		{
			_currentMatch = value;

			if( _currentMatch == null )
				_demoState = TurnBasedDemoState.AuthenticatedWithNoMatchLoaded;
			else if( _currentMatch.status == GPGTurnBasedMatchStatus.Active )
				_demoState = TurnBasedDemoState.AuthenticatedWithActiveMatch;
			else
				_demoState = TurnBasedDemoState.AuthenticatedWithInactiveMatch;
		}
	}


	void Start()
	{
		PlayGameServices.enableDebugLog( true );

#if UNITY_IPHONE
		// we always want to call init as soon as possible after launch. Be sure to pass your own clientId to init!
		// This call is not required on Android.
		PlayGameServices.init( "160040154367.apps.googleusercontent.com", true );

		// on iOS we will need to register for push notifications and send the device token to Google so they can send out
		// push notifications on your behalf
		Debug.Log( "registering for push notifications" );
		NotificationServices.RegisterForRemoteNotificationTypes( RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound );
#endif
	}


	void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		beginColumn();

		// we use the TurnBasedDemoState to decide what GUI should be available
		switch( _demoState )
		{
			case TurnBasedDemoState.NotAuthenticated:
				notauthenticatedGUI();
				break;
			case TurnBasedDemoState.AuthenticatedWithNoMatchLoaded:
				authenticatedWithNoMatchLoadedGUI();
				break;
			case TurnBasedDemoState.AuthenticatedWithActiveMatch:
				authenticatedWithActiveMatchGUI();
				break;
			case TurnBasedDemoState.AuthenticatedWithInactiveMatch:
				authenticatedWithInactiveMatchGUI();
				break;
		}

		endColumn();
	}


	/// <summary>
	/// GUI displayed when the user has not yet authenticated
	/// </summary>
	private void notauthenticatedGUI()
	{
		if( GUILayout.Button( "Authenticate" ) )
		{
			PlayGameServices.authenticate();
		}
	}


	/// <summary>
	/// GUI displayed when the user is authenticated but there is no match loaded
	/// </summary>
	private void authenticatedWithNoMatchLoadedGUI()
	{
#if UNITY_IPHONE
		// iOS only
		if( GUILayout.Button( "Register Device Token" ) )
		{
			if( NotificationServices.deviceToken != null )
				GPGMultiplayer.registerDeviceToken( NotificationServices.deviceToken, false );
			else
				Debug.LogWarning( "NotificationServices.deviceToken is null so we are not registering with Google" );
		}
#else

		// Android only
		if( GUILayout.Button( "Check for Invites and Matches after Launch" ) )
		{
			GPGTurnBasedMultiplayer.checkForInvitesAndMatches();
		}
#endif


		GUILayout.Label( "Match Creation and Management" );

		if( GUILayout.Button( "Show Match Inbox" ) )
		{
			GPGTurnBasedMultiplayer.showInbox();
		}


		if( GUILayout.Button( "Show Player Selector" ) )
		{
			GPGTurnBasedMultiplayer.showPlayerSelector( 1, 2 );
		}


		if( GUILayout.Button( "Create Match Programmatically" ) )
		{
			GPGTurnBasedMultiplayer.createMatchProgrammatically( 1, 1, 0 );
		}


		if( GUILayout.Button( "Load All Matches" ) )
		{
			GPGTurnBasedMultiplayer.loadAllMatches();
		}
	}


	/// <summary>
	/// GUI displayed when the user is authenticated and we have an active match loaded
	/// </summary>
	private void authenticatedWithActiveMatchGUI()
	{
		GUILayout.Label( "Match Status: " + currentMatch.status + ", Description: " + currentMatch.matchDescription );

		// is it our turn?
		if( currentMatch.isLocalPlayersTurn )
		{
			GUILayout.Label( "It is Our Turn" );

			if( GUILayout.Button( "Take Turn" ) )
			{
				GPGTurnBasedMultiplayer.takeTurn( currentMatch.matchId, getMatchDataWithNewDataAppended(), getPendingParticipantId() );
				currentMatch = null;
			}


			if( GUILayout.Button( "Finish Match Without Data" ) )
			{
				GPGTurnBasedMultiplayer.finishMatchWithoutData( currentMatch.matchId );
				currentMatch = null;
			}


			if( GUILayout.Button( "Finish Match With Data" ) )
			{
				var results = new List<GPGTurnBasedParticipantResult>();

				// we will need to create a GPGTurnBasedParticipantResult for each player in the match
				// for demonstration purposes we will just set the result to Tie
				foreach( var player in currentMatch.players )
					results.Add( new GPGTurnBasedParticipantResult( player.participantId, GPGTurnBasedParticipantResultStatus.Tie ) );

				GPGTurnBasedMultiplayer.finishMatchWithData( currentMatch.matchId, getMatchDataWithNewDataAppended(), results );
				currentMatch = null;
			}


			if( GUILayout.Button( "Leave Match During Turn" ) )
			{
				GPGTurnBasedMultiplayer.leaveDuringTurn( currentMatch.matchId, getPendingParticipantId() );
				currentMatch = null;
			}

			dismissMatchGuiButton();
		}
		else
		{
			GUILayout.Label( "It is Not Our Turn" );

			if( GUILayout.Button( "Leave Match Out of Turn" ) )
			{
				GPGTurnBasedMultiplayer.leaveOutOfTurn( currentMatch.matchId );
				currentMatch = null;
			}

			dismissMatchGuiButton();
		}
	}


	/// <summary>
	/// GUI displayed when the user is authenticated and we have an inactive match loaded
	/// </summary>
	private void authenticatedWithInactiveMatchGUI()
	{
		GUILayout.Label( "Match Status: " + currentMatch.status + ", Description: " + currentMatch.matchDescription );

		// we have this check here to work around what appears to be a Play bug on Android. Sometimes it is possible to get
		// a match in a state where it is our turn even though the match is completed. We need to get that status properly set
		// to MatchCompleted so we call finishMatch to do so.
		if( currentMatch.isLocalPlayersTurn )
		{
			if( GUILayout.Button( "Finish Match Without Data" ) )
			{
				GPGTurnBasedMultiplayer.finishMatchWithoutData( currentMatch.matchId );
				currentMatch = null;
			}
		}
		else
		{
			if( currentMatch.canRematch )
			{
				if( GUILayout.Button( "Rematch" ) )
				{
					GPGTurnBasedMultiplayer.rematch( currentMatch.matchId );
					currentMatch = null;
				}
			}
		}

		dismissMatchGuiButton();
	}



	#region Events and Helpers

	string getPendingParticipantId()
	{
		// your actual game logic to determine the next player would go here. For this demo, we will just look for
		// a player that is not us and make that the next player
		string pendingParticipantId = null;

		// if availableAutoMatchSlots is > 0 then we pass null as the next participant (which is an automatch player)
		// else we find a player that is not us
		if( currentMatch.availableAutoMatchSlots == 0 )
		{
			var participant = currentMatch.players.Where( p => p.participantId != currentMatch.localParticipantId ).FirstOrDefault();
			Debug.Log( "no auto match slots left so looking for a participant that is not us out of a total player count of " + currentMatch.players.Count );
			if( participant != null )
				pendingParticipantId = participant.participantId;
		}

		Debug.Log( "using pendingParticipantId: " + pendingParticipantId );
		return pendingParticipantId;
	}


	byte[] getMatchDataWithNewDataAppended()
	{
		var baseString = string.Empty;

		if( currentMatch.hasDataAvailable )
			baseString = System.Text.Encoding.UTF8.GetString( currentMatch.matchData );

		baseString += Utils.randomString( 1 );
		Debug.Log( "using match data: " + baseString );

		return System.Text.Encoding.UTF8.GetBytes( baseString );
	}


	// this is only broken out due to it being present 3 times
	void dismissMatchGuiButton()
	{
		if( GUILayout.Button( "Dismiss Match" ) )
		{
			GPGTurnBasedMultiplayer.dismissMatch( currentMatch.matchId );
			currentMatch = null;
		}


		if( GUILayout.Button( "Clear Current Local Match" ) )
		{
			currentMatch = null;
		}
	}


	void OnEnable()
	{
		GPGManager.authenticationSucceededEvent += authenticationSucceededEvent;
		GPGTurnBasedManager.matchChangedEvent += matchChanged;
		GPGTurnBasedManager.matchEndedEvent += matchChanged;
	}


	void OnDisable()
	{
		GPGManager.authenticationSucceededEvent -= authenticationSucceededEvent;
		GPGTurnBasedManager.matchChangedEvent -= matchChanged;
		GPGTurnBasedManager.matchEndedEvent -= matchChanged;
	}


	void authenticationSucceededEvent( string playerId )
	{
		currentMatch = null;
	}


	void matchChanged( GPGTurnBasedMatch match )
	{
		// in this simple demo, any time we get a matchChangedEvent we load it up immediately which updates the GUI
		currentMatch = match;
	}

	#endregion

#endif
}
