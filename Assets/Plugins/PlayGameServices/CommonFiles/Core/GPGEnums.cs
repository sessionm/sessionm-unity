using UnityEngine;
using System.Collections;


#if UNITY_IPHONE || UNITY_ANDROID
public enum GPGToastPlacement
{
	Top,
	Bottom,
	Center
}


public enum GPGLeaderboardTimeScope
{
	Unknown = -1,
	Today = 1,
	ThisWeek = 2,
	AllTime = 3
}


public enum GPGQuestState
{
	// Upcoming quest, before start time.
	Upcoming,
	// Open quest, in between start time and expiration time.
	Open,
	// Quest accepted by player, in between start time and expiration time.
	Accepted,
	// Quest completed by player, rewards been claimed.
	Completed,
	// Quest expired, not accepted by player, after expiration time.
	Expired,
	// Quest expired, accepted by player, after expiration time.
	Failed
}


public enum GPGQuestMilestoneState
{
	// Milestone is not completed.
	NotCompleted,
	// Milestone is completed, not claimed.
	CompletedNotClaimed,
	// Milestone is completed, claimed.
	Claimed
}

#endif