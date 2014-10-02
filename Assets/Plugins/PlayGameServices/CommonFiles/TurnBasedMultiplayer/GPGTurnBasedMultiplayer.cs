using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;


#if UNITY_IPHONE
public class GPGTurnBasedMultiplayer
{
	// Android only. You should call this every launch after the local player is signed in. If there is an invite or match available
	// it will fire the invitationReceivedEvent or the matchChangedEvent respectively.
	public static void checkForInvitesAndMatches()
	{}


	[DllImport("__Internal")]
	private static extern void _gplayShowInbox();

	// Shows the default match list UI allowing users to accept/decline invitations, select a match to play and dismiss matches
	public static void showInbox()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayShowInbox();
	}


	[DllImport("__Internal")]
	private static extern void _gplayShowTurnBasedPlayerSelector( int minPlayersToPick, int maxPlayersToPick );

	// Shows the player selector allowing the user to select G+ friends or auto-match players to start a match.
	// The match is started once the matchChangedEvent fires.
	public static void showPlayerSelector( int minPlayersToPick, int maxPlayersToPick )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayShowTurnBasedPlayerSelector( minPlayersToPick, maxPlayersToPick );
	}


	[DllImport("__Internal")]
	private static extern void _gplayCreateMatchProgrammatically( int minAutoMatchPlayers, int maxAutoMatchPlayers, int exclusiveBitmask, int variant );

	// Creates a match programmatically with the given auto-match criteria. The match is started once the matchChangedEvent fires.
	// Exclusive bitmasks are for the automatching request. The logical AND of each pairing of automatching requests must equal zero for auto-match.
	// If there are no exclusivity requirements for the game, this value should just be set to 0
	// When variant is set, only other players with the same variant will be matched up.
	public static void createMatchProgrammatically( int minAutoMatchPlayers, int maxAutoMatchPlayers, int exclusiveBitmask = 0, int variant = 0 )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayCreateMatchProgrammatically( minAutoMatchPlayers, maxAutoMatchPlayers, exclusiveBitmask, variant );
	}


	[DllImport("__Internal")]
	private static extern void _gplayLoadAllMatches();

	// Loads all the matches the player is currently taking part in. Results in the loadMatchesCompletedEvent firing.
	public static void loadAllMatches()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayLoadAllMatches();
	}


	[DllImport("__Internal")]
	private static extern void _gplayTakeTurn( string matchId, byte[] matchData, int matchDataLength, string pendingParticipantId );

	// Commits the results of a player's turn. Results in the takeTurnSucceeded/FailedEvent firing.
	public static void takeTurn( string matchId, byte[] matchData, string pendingParticipantId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayTakeTurn( matchId, matchData, matchData.Length, pendingParticipantId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayLeaveDuringTurn( string matchId, string pendingParticipantId );

	// Leaves a turn-based match when it is the current player's turn, without canceling the match.
	public static void leaveDuringTurn( string matchId, string pendingParticipantId )
	{
		if( pendingParticipantId == null )
		{
			Debug.LogWarning( "leaveDuringTurn called with a null pendingParticipantId which is invalid" );
			return;
		}

		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayLeaveDuringTurn( matchId, pendingParticipantId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayLeaveOutOfTurn( string matchId );

	// Leaves a turn-based match when it is not the current player's turn, without canceling the match.
	public static void leaveOutOfTurn( string matchId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayLeaveOutOfTurn( matchId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayFinishMatchWithData( string matchId, byte[] matchData, int matchDataLength, string resultsJson );

	// Finishes a turn-based match. The last client to update a match is responsible for calling finish on that match.
	// Important! See the note above in the "Important note about finishing a match" section for more information.
	public static void finishMatchWithData( string matchId, byte[] matchData, List<GPGTurnBasedParticipantResult> results )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayFinishMatchWithData( matchId, matchData, matchData.Length, Json.encode( results ) );
	}


	[DllImport("__Internal")]
	private static extern void _gplayFinishMatchWithoutData( string matchId );

	// Finishes a turn-based match. The last client to update a match is responsible for calling finish on that match.
	// Important! See the note above in the "Important note about finishing a match" section for more information.
	public static void finishMatchWithoutData( string matchId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayFinishMatchWithoutData( matchId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayDismissMatch( string matchId );

	// Dismisses a turn-based match from the match list.
	// The match will no longer show up in the list and will not generate notifications.
	public static void dismissMatch( string matchId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayDismissMatch( matchId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayRematch( string matchId );

	// Creates a rematch of a match that was previously completed, with the same participants.
	// This can be called by only one player on a match that is still in their list.
	// The player must have called finish first and it will be the caller's turn.
	public static void rematch( string matchId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayRematch( matchId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayJoinMatchWithInvitation( string invitationId );

	// Joins a turn-based match that the player has been invited to
	public static void joinMatchWithInvitation( string invitationId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayJoinMatchWithInvitation( invitationId );
	}


	[DllImport("__Internal")]
	private static extern void _gplayDeclineMatchWithInvitation( string invitationId );

	// Declines an invitation to play a turn-based match
	public static void declineMatchWithInvitation( string invitationId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_gplayDeclineMatchWithInvitation( invitationId );
	}

}
#endif