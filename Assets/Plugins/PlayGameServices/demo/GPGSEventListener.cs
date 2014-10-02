using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GPGSEventListener : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	void OnEnable()
	{
		GPGManager.authenticationSucceededEvent += authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent += authenticationFailedEvent;
		GPGManager.licenseCheckFailedEvent += licenseCheckFailedEvent;
		GPGManager.profileImageLoadedAtPathEvent += profileImageLoadedAtPathEvent;
		GPGManager.finishedSharingEvent += finishedSharingEvent;
		GPGManager.loadPlayerCompletedEvent += loadPlayerCompletedEvent;
		GPGManager.userSignedOutEvent += userSignedOutEvent;

		GPGManager.reloadDataForKeyFailedEvent += reloadDataForKeyFailedEvent;
		GPGManager.reloadDataForKeySucceededEvent += reloadDataForKeySucceededEvent;
		GPGManager.loadCloudDataForKeyFailedEvent += loadCloudDataForKeyFailedEvent;
		GPGManager.loadCloudDataForKeySucceededEvent += loadCloudDataForKeySucceededEvent;
		GPGManager.updateCloudDataForKeyFailedEvent += updateCloudDataForKeyFailedEvent;
		GPGManager.updateCloudDataForKeySucceededEvent += updateCloudDataForKeySucceededEvent;
		GPGManager.clearCloudDataForKeyFailedEvent += clearCloudDataForKeyFailedEvent;
		GPGManager.clearCloudDataForKeySucceededEvent += clearCloudDataForKeySucceededEvent;
		GPGManager.deleteCloudDataForKeyFailedEvent += deleteCloudDataForKeyFailedEvent;
		GPGManager.deleteCloudDataForKeySucceededEvent += deleteCloudDataForKeySucceededEvent;

		GPGManager.unlockAchievementFailedEvent += unlockAchievementFailedEvent;
		GPGManager.unlockAchievementSucceededEvent += unlockAchievementSucceededEvent;
		GPGManager.incrementAchievementFailedEvent += incrementAchievementFailedEvent;
		GPGManager.incrementAchievementSucceededEvent += incrementAchievementSucceededEvent;
		GPGManager.revealAchievementFailedEvent += revealAchievementFailedEvent;
		GPGManager.revealAchievementSucceededEvent += revealAchievementSucceededEvent;

		GPGManager.submitScoreFailedEvent += submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent += submitScoreSucceededEvent;
		GPGManager.loadScoresFailedEvent += loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent += loadScoresSucceededEvent;

		GPGManager.allEventsLoadedEvent += allEventsLoadedEvent;
		GPGManager.questListLauncherAcceptedQuestEvent += questListLauncherAcceptedQuestEvent;
		GPGManager.questClaimedRewardsForQuestMilestoneEvent += questClaimedRewardsForQuestMilestoneEvent;
		GPGManager.questCompletedEvent += questCompletedEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		GPGManager.authenticationSucceededEvent -= authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent -= authenticationFailedEvent;
		GPGManager.licenseCheckFailedEvent -= licenseCheckFailedEvent;
		GPGManager.profileImageLoadedAtPathEvent -= profileImageLoadedAtPathEvent;
		GPGManager.finishedSharingEvent -= finishedSharingEvent;
		GPGManager.loadPlayerCompletedEvent -= loadPlayerCompletedEvent;
		GPGManager.userSignedOutEvent -= userSignedOutEvent;

		GPGManager.reloadDataForKeyFailedEvent -= reloadDataForKeyFailedEvent;
		GPGManager.reloadDataForKeySucceededEvent -= reloadDataForKeySucceededEvent;
		GPGManager.loadCloudDataForKeyFailedEvent -= loadCloudDataForKeyFailedEvent;
		GPGManager.loadCloudDataForKeySucceededEvent -= loadCloudDataForKeySucceededEvent;
		GPGManager.updateCloudDataForKeyFailedEvent -= updateCloudDataForKeyFailedEvent;
		GPGManager.updateCloudDataForKeySucceededEvent -= updateCloudDataForKeySucceededEvent;
		GPGManager.clearCloudDataForKeyFailedEvent -= clearCloudDataForKeyFailedEvent;
		GPGManager.clearCloudDataForKeySucceededEvent -= clearCloudDataForKeySucceededEvent;
		GPGManager.deleteCloudDataForKeyFailedEvent -= deleteCloudDataForKeyFailedEvent;
		GPGManager.deleteCloudDataForKeySucceededEvent -= deleteCloudDataForKeySucceededEvent;

		GPGManager.unlockAchievementFailedEvent -= unlockAchievementFailedEvent;
		GPGManager.unlockAchievementSucceededEvent -= unlockAchievementSucceededEvent;
		GPGManager.incrementAchievementFailedEvent -= incrementAchievementFailedEvent;
		GPGManager.incrementAchievementSucceededEvent -= incrementAchievementSucceededEvent;
		GPGManager.revealAchievementFailedEvent -= revealAchievementFailedEvent;
		GPGManager.revealAchievementSucceededEvent -= revealAchievementSucceededEvent;

		GPGManager.submitScoreFailedEvent -= submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent -= submitScoreSucceededEvent;
		GPGManager.loadScoresFailedEvent -= loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent -= loadScoresSucceededEvent;

		GPGManager.allEventsLoadedEvent -= allEventsLoadedEvent;
		GPGManager.questListLauncherAcceptedQuestEvent -= questListLauncherAcceptedQuestEvent;
		GPGManager.questClaimedRewardsForQuestMilestoneEvent -= questClaimedRewardsForQuestMilestoneEvent;
		GPGManager.questCompletedEvent -= questCompletedEvent;
	}





	void authenticationSucceededEvent( string param )
	{
		Debug.Log( "authenticationSucceededEvent: " + param );
	}


	void authenticationFailedEvent( string error )
	{
		Debug.Log( "authenticationFailedEvent: " + error );
	}


	void licenseCheckFailedEvent()
	{
		Debug.Log( "licenseCheckFailedEvent" );
	}


	void profileImageLoadedAtPathEvent( string path )
	{
		Debug.Log( "profileImageLoadedAtPathEvent: " + path );
	}


	void finishedSharingEvent( string errorOrNull )
	{
		Debug.Log( "finishedSharingEvent. errorOrNull param: " + errorOrNull );
	}


	void loadPlayerCompletedEvent( GPGPlayerInfo playerInfo, string error )
	{
		Debug.Log( "loadPlayerCompletedEvent: " );
		if( playerInfo != null )
			Prime31.Utils.logObject( playerInfo );
		else
			Debug.Log( error );
	}


	void userSignedOutEvent()
	{
		Debug.Log( "userSignedOutEvent" );
	}


	void reloadDataForKeyFailedEvent( string error )
	{
		Debug.Log( "reloadDataForKeyFailedEvent: " + error );
	}


	void reloadDataForKeySucceededEvent( string param )
	{
		Debug.Log( "reloadDataForKeySucceededEvent: " + param );
	}


	void loadCloudDataForKeyFailedEvent( string error )
	{
		Debug.Log( "loadCloudDataForKeyFailedEvent: " + error );
	}


	void loadCloudDataForKeySucceededEvent( int key, string data )
	{
		Debug.Log( "loadCloudDataForKeySucceededEvent:" + data );
	}


	void updateCloudDataForKeyFailedEvent( string error )
	{
		Debug.Log( "updateCloudDataForKeyFailedEvent: " + error );
	}


	void updateCloudDataForKeySucceededEvent( int key, string data )
	{
		Debug.Log( "updateCloudDataForKeySucceededEvent: " + data );
	}


	void clearCloudDataForKeyFailedEvent( string error )
	{
		Debug.Log( "clearCloudDataForKeyFailedEvent: " + error );
	}


	void clearCloudDataForKeySucceededEvent( string param )
	{
		Debug.Log( "clearCloudDataForKeySucceededEvent: " + param );
	}


	void deleteCloudDataForKeyFailedEvent( string error )
	{
		Debug.Log( "deleteCloudDataForKeyFailedEvent: " + error );
	}


	void deleteCloudDataForKeySucceededEvent( string param )
	{
		Debug.Log( "deleteCloudDataForKeySucceededEvent: " + param );
	}


	void unlockAchievementFailedEvent( string achievementId, string error )
	{
		Debug.Log( "unlockAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error );
	}


	void unlockAchievementSucceededEvent( string achievementId, bool newlyUnlocked )
	{
		Debug.Log( "unlockAchievementSucceededEvent. achievementId: " + achievementId + ", newlyUnlocked: " + newlyUnlocked );
	}


	void incrementAchievementFailedEvent( string achievementId, string error )
	{
		Debug.Log( "incrementAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error );
	}


	void incrementAchievementSucceededEvent( string achievementId, bool newlyUnlocked )
	{
		Debug.Log( "incrementAchievementSucceededEvent. achievementId: " + achievementId + ", newlyUnlocked: " + newlyUnlocked );
	}


	void revealAchievementFailedEvent( string achievementId, string error )
	{
		Debug.Log( "revealAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error );
	}


	void revealAchievementSucceededEvent( string achievementId )
	{
		Debug.Log( "revealAchievementSucceededEvent: " + achievementId );
	}


	void submitScoreFailedEvent( string leaderboardId, string error )
	{
		Debug.Log( "submitScoreFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error );
	}


	void submitScoreSucceededEvent( string leaderboardId, Dictionary<string,object> scoreReport )
	{
		Debug.Log( "submitScoreSucceededEvent" );
		Prime31.Utils.logObject( scoreReport );
	}


	void loadScoresFailedEvent( string leaderboardId, string error )
	{
		Debug.Log( "loadScoresFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error );
	}


	void loadScoresSucceededEvent( List<GPGScore> scores )
	{
		Debug.Log( "loadScoresSucceededEvent" );
		Prime31.Utils.logObject( scores );
	}


	void allEventsLoadedEvent( List<GPGEvent> events )
	{
		Debug.Log( "allEventsLoadedEvent" );
		Prime31.Utils.logObject( events );
	}


	void questListLauncherAcceptedQuestEvent( GPGQuest quest )
	{
		Debug.Log( "questListLauncherAcceptedQuestEvent" );
		Prime31.Utils.logObject( quest );
	}


	void questClaimedRewardsForQuestMilestoneEvent( GPGQuestMilestone milestone )
	{
		Debug.Log( "questClaimedRewardsForQuestMilestoneEvent" );
		Prime31.Utils.logObject( milestone );
	}


	void questCompletedEvent( GPGQuest quest )
	{
		Debug.Log( "questCompletedEvent" );
		Prime31.Utils.logObject( quest );
	}

#endif
}


