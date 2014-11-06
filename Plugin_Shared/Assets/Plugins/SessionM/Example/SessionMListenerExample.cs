using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SessionMListenerExample : MonoBehaviour 
{

	//To react to events happening in the SessionM Service, you can listen for the following events:
	private void OnEnable()
	{
		SessionMEventListener.NotifySessionStateChanged += NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError += NotifySessionError;
		SessionMEventListener.NotifyActivityPresented += NotifyActivityPresented;
		SessionMEventListener.NotifyActivityDismissed += NotifyActivityDismissed;
		SessionMEventListener.NotifyUserInfoChanged += NotifyUserInfoChanged;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated += NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserAction += NotifyUserAction;
	}

	private void OnDisable()
	{
		SessionMEventListener.NotifySessionStateChanged -= NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError -= NotifySessionError;
		SessionMEventListener.NotifyActivityPresented -= NotifyActivityPresented;
		SessionMEventListener.NotifyActivityDismissed -= NotifyActivityDismissed;
		SessionMEventListener.NotifyUserInfoChanged -= NotifyUserInfoChanged;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated -= NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserAction -= NotifyUserAction;
	}

	private void NotifyActivityPresented(ActivityType activityPresented)
	{
		Debug.Log("Event: NotifyActivityPresented Fired");
		//Use this method to pause the appropiate app elements when your Unity App is backgrounded.
	}

	private void NotifyActivityDismissed(ActivityType activityDismissed)
	{
		Debug.Log("Event: NotifyActivityDismissed Fired");
		//You can monitor this to unpause app elements when the SessionM overlay is dismissed.
	}

	private void NotifySessionError(int errorCode, string error)
	{
		Debug.Log("Event: NotifySessionError Fired");
		//This is a good way to tell if your user is in a valid SessionM region, if they aren't you'll get back the error code: 100
	}

	private void NotifySessionStateChanged(SessionState state)
	{
		Debug.Log("Event: NotifySessionStateChanged Fired");
	}

	private void NotifyUnclaimedAchievementDataUpdated(IAchievementData achievementData)
	{
		Debug.Log ("Event: NotifyUnclaimedAchievementDataUpdated Fired");
		//This callback is currently in Beta.  For now, don't depend on it to report all of your user's unclaimed achievement data.
	}

	private void NotifyUserAction(UserAction arg1, IDictionary<string, object> arg2)
	{
		Debug.Log("Event: NotifyUserAction Fired");
		//The following are potential UserActions:
		//		AchievementViewAction = 100,
		//		AchievementEngagedAction = 101,
		//		AchievementDismissedAction = 102,
		//		SponsorContentViewedAction = 103,
		//		SponsorContentEngagedAction = 104,
		//		SponsorContentDismissedAction = 105,
		//		PortalViewedAction = 106,
		//		SignInAction = 107,
		//		SignOutAction = 108,
		//		RegisteredAction = 109,
		//		PortalDismissedAction = 110,
		//		RedeemedRewardAction = 111,
		//		CheckinCompletedAction = 112,
		//		VirtualItemRewardAction = 113
	}

	private void NotifyUserInfoChanged(IDictionary<string, object> newUserInfo)
	{
		Debug.Log("Event: NotifyUserInfoChanged Fired");
		//This event can notify you if users
	}

}
