using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using MiniJSON;

public class SessionMEventListener : MonoBehaviour 
{
	// Notifies that session state has changed.  
	public static event Action<SessionState> NotifySessionStateChanged;
	// Notifies that session start error has occured.
	public static event Action<int, string> NotifySessionError;

	// Notifies that interactable display has started.
	public static event Action<ActivityType>  NotifyActivityPresented;
	// Notifies that interactable display has finished.
	public static event Action<ActivityType> NotifyActivityDismissed;

	// Notifies that user info (achievement details, etc) has changed. 
	// This method is reserved for future use. Please, contact Session M for more information. 
	public static event Action<IDictionary<string, object>>  NotifyUserInfoChanged;

	// Notifies that current unclaimed achievement data has been updated. 
	// This method is called when (1) new achievement has been earned with respective achievement data object, (2) last earned achievement has been claimed in which case achievement data object is null. 
	public static event Action<IAchievementData> NotifyUnclaimedAchievementDataUpdated;

	// Notifies that user performed action withing context of current 	activity
	public static event Action<UserAction, IDictionary<string, object>> NotifyUserAction;

	private ISessionM nativeParent;
	private ISessionMCallback callback;

	public void SetNativeParent(ISessionM nativeParent)
	{
		this.nativeParent = nativeParent;
	}

	public void SetCallback(ISessionMCallback callback)
	{
		this.callback = callback;
	}

	// native callback handling 

	private void _sessionM_HandleStateTransitionMessage(string message) 
	{
		SessionState state = (SessionState)int.Parse(message);

		//Register Event
		if(NotifySessionStateChanged != null) {
			NotifySessionStateChanged(state);
		}

		if(callback != null) {
			callback.NotifySessionStateChanged(nativeParent, state);
		}
	}

	private void _sessionM_HandleSessionFailedMessage(string message)
	{

		// scan message and extract error information
		// expecting 2 components - error code and description	
		List<string> components = GetStringComponents(message);
		if(components == null || components.Count != 2) {
			string reason = components == null ? "components is null" : "unexpected component count";
			Debug.Log("_sessionM_HandleSessionFailedMessage: malformatted message: '" + message + "', reason: " + reason);
			return;
		}

		string errorCodeString = components[0];
		int errorCode = int.Parse(errorCodeString);
		string description = components[1];

		//Register Event
		if(NotifySessionError != null) {
			NotifySessionError(errorCode, description);
		}

		if(callback != null) {
			callback.NotifySessionError(nativeParent, errorCode, description);
		}
	}

	private void _sessionM_HandlePresentedActivityMessage(string message) 
	{

		if(message == null) {
			Debug.Log("_sessionM_HandlePresentedActivityMessage: malformatted message: '" + message);
			return;
		}

		ActivityType activityType = (ActivityType)int.Parse(message);

		//Register Event
		if(NotifyActivityPresented != null)
			NotifyActivityPresented(activityType);

		if(callback != null) {
			callback.NotifyActivityPresented(nativeParent, activityType);
		}
	}

	private void _sessionM_HandleDismissedActivityMessage(string message)
	{

		if(message == null) {
			Debug.Log("_sessionM_HandleDismissedActivityMessage: malformatted message: '" + message);
			return;
		}

		ActivityType activityType = (ActivityType)int.Parse(message);

		//Register Event
		if(NotifyActivityDismissed != null) {
			NotifyActivityDismissed(activityType);
		}
			
		if(callback != null) {
			callback.NotifyActivityDismissed(nativeParent, activityType);
		}
	}

	private void _sessionM_HandleUserInfoChangedMessage(string message)
	{
		Dictionary<string, object> userInfo = Json.Deserialize(message) as Dictionary<string,object>;

		//Register Event
		if(NotifyUserInfoChanged != null) {
			NotifyUserInfoChanged(userInfo);
		}

		if(callback != null) {
			callback.NotifyUserInfoChanged(nativeParent, userInfo);
		}
	}

	private void _sessionM_HandleUserActionMessage(string message) 
	{
		Dictionary<string, object> userActioniDict = Json.Deserialize(message) as Dictionary<string,object>;
		long userAction = (Int64)userActioniDict["userAction"];
		Dictionary<string, object> data = (Dictionary<string, object>)userActioniDict["data"];

		//Register Event
		if(NotifyUserAction != null) {
			NotifyUserAction((UserAction)userAction, data);
		}

		if(callback != null) {
			callback.NotifyUserAction(nativeParent, (UserAction)userAction, data);
		}
	}

	private void _sessionM_HandleUpdatedUnclaimedAchievementMessage(string message)
	{
		IAchievementData achievementData = SessionM.GetAchievementData(message);

		//Register Event
		if(NotifyUnclaimedAchievementDataUpdated != null) {
			NotifyUnclaimedAchievementDataUpdated(achievementData);
		}

		if(callback != null) {
			callback.NotifyUnclaimedAchievementDataUpdated(nativeParent, achievementData);
		}
	}

	// parse the string extracting components; 
	// input string has the following format: "<length1>:<string1><length2>:<string2><length3>:<string3>" where "<length>:<string>" is a token representing a string (input string consists of many such tokens)
	private List<string> GetStringComponents(string inputString)
	{
		List<string> components = new List<string>();

		int tokenStartIndex = 0;
		while(tokenStartIndex < inputString.Length) {
			int separatorIndex = inputString.IndexOf(':', tokenStartIndex);	
			if(separatorIndex < 0) {
				// this should never happen in a well-formatted string	
				return null;
			}

			string length = inputString.Substring(tokenStartIndex, separatorIndex - tokenStartIndex);
			int stringLength = int.Parse(length);
			string stringValue = inputString.Substring(separatorIndex + 1, stringLength);
			components.Add(stringValue);
			tokenStartIndex = separatorIndex + stringLength + 1;
		}
		return components;	
	}
}
