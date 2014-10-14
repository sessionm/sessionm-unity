using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * IMPORTANT. This is example code and therefore subject to changes without prior notice. 
 * You can use this class as a starting point to integrate session M service into your application.
 * 
 * Session M manager. Starts session M service, registers and listens to service callbacks to display UI activies and perform other functions. 
 */
public class SessionMManager : MonoBehaviour, ISessionMCallback
{
	void OnEnable()
	{
		DontDestroyOnLoad(this);
		SessionM.GetInstance().SetCallback(this);
	}
	
	void OnDisable() 
	{
		SessionM.GetInstance().SetCallback(null);
	}
	
	// Notifies that session state has changed.  
	public void NotifySessionStateChanged(ISessionM sessionM, SessionState state) 
	{
		// Insert your code here
		//if(state == SessionState.StartedOnline || state == SessionState.StartedOffline) {
		//	Debug.Log("SessionM started successfully!");
		//}
	}
	
	public void NotifySessionError(ISessionM sessionM, int code, string description) 
	{
		// Insert your code here
		// Debug.Log("SessionM failed with error: " + description);
	}

	public void NotifyActivityPresented(ISessionM sessionM, ActivityType type)
	{
		// Insert your code here
		// e.g. suspend your game
		// Time.timeScale = 0.0f;
	}
	
	public void NotifyActivityDismissed(ISessionM sessionM, ActivityType type)
	{
		// Insert your code here
		// e.g. resume your game
		// Time.timeScale = 1.0f;
	}
	
	public void NotifyActivityUnavailable(ISessionM sessionM, ActivityType type)
	{
		// Deprecated
	}
	
	public void NotifyActivityAvailable(ISessionM sessionM, ActivityType type)
	{
		// Deprecated
	}

	public void NotifyUserInfoChanged(ISessionM sessionM, IDictionary<string, object> info)
	{
		// Insert your code here
	}

	public void NotifyUnclaimedAchievementDataUpdated(ISessionM sessionM, IAchievementData achievementData)
	{
		// Present achievement immediatelly, e.g
		// sessionM.PresentActivity(ActivityType.Achievement)
	}
	
	public void NotifyUserAction(ISessionM sessionM, UserAction userAction, IDictionary<string, object> data)
	{
		// Insert your code here
		// e.g. suspend your game
		// if(userAction == UserAction.AchievementEngagedAction) {
		//     Time.timeScale = 0.0f;
		// }
	}

}

