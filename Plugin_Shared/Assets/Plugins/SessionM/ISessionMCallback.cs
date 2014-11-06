using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Session M service callback interface. 
 * 
 * This interface is used in conjunction with ISessionM service interface to receive notifications from session M service. 
 */ 
public interface ISessionMCallback
{
	// Notifies that session state has changed.  
	void NotifySessionStateChanged(ISessionM sessionM, SessionState state);
	// Notifies that session start error has occured.
	void NotifySessionError(ISessionM sessionM, int code, string description);

	// Notifies that interactable display has started.
	void NotifyActivityPresented(ISessionM sessionM, ActivityType type);
	// Notifies that interactable display has finished.
	void NotifyActivityDismissed(ISessionM sessionM, ActivityType type);
	
	// Notifies that user info (achievement details, etc) has changed. 
	// This method is reserved for future use. Please, contact Session M for more information. 
	void NotifyUserInfoChanged(ISessionM sessionM, IDictionary<string, object> info);
	
	// Notifies that current unclaimed achievement data has been updated. 
	// This method is called when (1) new achievement has been earned with respective achievement data object, (2) last earned achievement has been claimed in which case achievement data object is null. 
	void NotifyUnclaimedAchievementDataUpdated(ISessionM sessionM, IAchievementData achievementData);

	// Notifies that user performed action withing context of current activity
	void NotifyUserAction(ISessionM sessionM, UserAction userAction, IDictionary<string, object> data);


	// This method is deprecated. Please, use return value from ISessionM.PresentActivity(ActivityType) to determine is UI activitiy will be presented. 
	//void NotifyActivityUnavailable(ISessionM sessionM, ActivityType type);
	// This method is deprecated. Please, use ISessionM.IsActivityAvailable(ActivityType) method to determine if activity is available. 
	// For achievement use NotifyUnclaimedAchievementDataUpdated(ISessionM, IAchievementData)
	//void NotifyActivityAvailable(ISessionM sessionM, ActivityType type);
}

