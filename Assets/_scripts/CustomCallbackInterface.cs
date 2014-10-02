using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomCallbackInterface : MonoBehaviour, ISessionMCallback
{
	public SessionMTesterGUI gui;

	private void Awake()
	{
		SessionM.GetInstance().SetCallback(this);
	}

	public void NotifySessionStateChanged(ISessionM sessionM, SessionState state) 
	{
		Debug.Log("Callback: NotifySessionStateChanged Fired");
		gui.eventsAndCallbacks.callbackNotifySessionChanged.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s" + " - State: " + state.ToString();
	}

	public void NotifySessionError(ISessionM sessionM, int code, string description) 
	{
		Debug.Log("Callback: NotifySessionError Fired");
		gui.eventsAndCallbacks.callbackNotifySessionError.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s"+ "Error Code: " + code.ToString() + " - Error: " + description;
	}
		
	public void NotifyActivityPresented(ISessionM sessionM, ActivityType type)
	{
		Debug.Log("Callback: NotifyActivityPresented Fired");
		gui.eventsAndCallbacks.callbackNotifyActivityPresented.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s" + " - Activity: " + type.ToString();
	}

	public void NotifyActivityDismissed(ISessionM sessionM, ActivityType type)
	{
		Debug.Log("Callback: NotifyActivityDismissed Fired");
		gui.eventsAndCallbacks.callbackNotifyActivityDismissed.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s" + " - Activity: " + type.ToString();
	}
		
	public void NotifyUserInfoChanged(ISessionM sessionM, IDictionary<string, object> info)
	{
		Debug.Log("Callback: NotifyUserInfoChanged Fired");
		string userInfo = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s";
		foreach(KeyValuePair<string, object> entry in info) {
			userInfo += " | " + entry.Key;
			userInfo += " - " + entry.Value;
		}

		gui.eventsAndCallbacks.callbackNotifyUserInfoChanged.text = userInfo;
	}
		
	public void NotifyUnclaimedAchievementDataUpdated(ISessionM sessionM, IAchievementData achievementData)
	{
		Debug.Log("Callback: NotifyUnclaimedAchievementDataUpdated Fired");
		string fullAchData = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s";
		fullAchData += " | Name: " + achievementData.GetName();
		fullAchData += " | Val: " + achievementData.GetMpointValue();
		fullAchData += " | Message: " + achievementData.GetMessage();
		fullAchData += " | IsCustom: " + achievementData.IsCustom().ToString();
		gui.eventsAndCallbacks.callbackNotifyUnclaimedAchievementDataUpdated.text = fullAchData;
	}
		
	public void NotifyUserAction(ISessionM sessionM, UserAction userAction, IDictionary<string, object> data)
	{
		Debug.Log("Callback: NotifyUserAction Fired");
		string userActionString = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s";
		userActionString += " | UserAction: " + userAction.ToString();
		foreach(KeyValuePair<string, object> entry in data) {
			userActionString += " | " + entry.Key;
			userActionString += " - " + entry.Value;
		}

		gui.eventsAndCallbacks.callbackNotifyUseraction.text = userActionString;
	}

	public void NotifyActivityAvailable(ISessionM sessionM, ActivityType type) {

	}

	public void NotifyActivityUnavailable(ISessionM sessionM, ActivityType type) {

	}
}
