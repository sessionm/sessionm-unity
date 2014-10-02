using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SessionMTesterGUI : MonoBehaviour 
{
	[Serializable]
	public class EventsCallbacks
	{
		public UILabel eventNotifySessionChanged;
		public UILabel eventNotifySessionError;
		public UILabel eventNotifyActivityPresented;
		public UILabel eventNotifyActivityDismissed;
		public UILabel eventNotifyUserInfoChanged;
		public UILabel eventNotifyUnclaimedAchievementDataUpdated;
		public UILabel eventNotifyUseraction;
		
		public UILabel callbackNotifySessionChanged;
		public UILabel callbackNotifySessionError;
		public UILabel callbackNotifyActivityPresented;
		public UILabel callbackNotifyActivityDismissed;
		public UILabel callbackNotifyUserInfoChanged;
		public UILabel callbackNotifyUnclaimedAchievementDataUpdated;
		public UILabel callbackNotifyUseraction;
		
	}
	
	public SessionM sessionM;
	
	public UILabel iOSID;
	public UILabel androidID;
	public UILabel unclaimedAchievements;
	public UILabel sessionMInterface;
	
	public UILabel activityAvailableAchievement;
	public UILabel activityAvailablePortal;
	public UILabel activityAvailableIntro;
	public UILabel activityAvailableInterstitial;
	public UILabel sdkVersion;
	
	public UILabel logLevel;
	
	public EventsCallbacks eventsAndCallbacks;
	
	private AchievementData unclaimedAch;
	
	public void OnOpenPortal()
	{
		Debug.Log("OnOpenPortal");
		SessionM.GetInstance().ShowPortal();
	}
	
	public void OnActionPlayed()
	{
		Debug.Log("OnActionPlayed");
		SessionM.GetInstance().LogAction("Played");
	}
	
	public void OnActionCrocJump()
	{
		Debug.Log("OnActionCrocJump");
		SessionM.GetInstance().LogAction("CrocJump");
	}
	
	public void OnAction100Bubbles()
	{
		Debug.Log("OnAction100Bubbles");
		SessionM.GetInstance().LogAction("100Bubbles");
	}
	
	public void OnPresentAchievement()
	{
		Debug.Log("OnPresentAchievement");
		SessionM.GetInstance().PresentActivity(ActivityType.Achievement);
	}
	
	public void OnPresentInterstitial()
	{
		Debug.Log("OnPresentInterstitial");
		SessionM.GetInstance().PresentActivity(ActivityType.Interstitial);
	}
	
	public void OnCycleLogLevel()
	{
		Debug.Log("OnCycleLogLevel");
		switch(sessionM.GetLogLevel()) {
		case LogLevel.Off:
			sessionM.SetLogLevel(LogLevel.Info);
			break;
		case LogLevel.Info:
			sessionM.SetLogLevel(LogLevel.Debug);
			break;
		case LogLevel.Debug:
			sessionM.SetLogLevel(LogLevel.Off);
			break;
		}
		
		logLevel.text = SessionM.GetInstance().GetLogLevel().ToString();
	}
	
	public void OnNotifyAcheivementPresented()
	{
		if(unclaimedAch != null && SessionM.GetInstance().IsActivityPresented() == false) {
			Debug.Log ("Notifying Achievement: " + unclaimedAch.Identifier() + "has been presented.");
			SessionM.GetInstance().NotifyPresented();
		}
	}
	
	public void OnNotifyAchievementDismissed()
	{
		SessionM.GetInstance ().NotifyDismissed ();
		Debug.Log ("Notifying achievement has been dismissed.");
	}
	
	public void OnNotifyAchievementClaimed()
	{
		SessionM.GetInstance().NotifyClaimed();
		Debug.Log ("Notifying achievement has been claimed.");
	}
	
	private void Start()
	{
		iOSID.text = sessionM.iosAppId;
		androidID.text = sessionM.androidAppId;
		DetectSessionMInterface();
		sdkVersion.text = sessionM.GetSDKVersion();
		logLevel.text = SessionM.GetInstance().GetLogLevel().ToString();
	}
	
	private void DetectSessionMInterface()
	{
		if(SessionM.GetInstance().SessionMNative is ISessionM_Dummy)
			sessionMInterface.text = "ISessionM_Dummy";
		
		#if UNITY_IOS
		if(SessionM.GetInstance().SessionMNative is ISessionM_iOS)
			sessionMInterface.text = "ISessionM_iOS";
		#endif
		
		#if UNITY_ANDROID
		if(SessionM.GetInstance().SessionMNative is ISessionM_Android)
			sessionMInterface.text = "ISessionM_Android";
		#endif
	}
	
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
	
	
	private void NotifyActivityDismissed(ActivityType activityDismissed)
	{
		Debug.Log("Event: NotifyActivityDismissed Fired");
		eventsAndCallbacks.eventNotifyActivityDismissed.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s" + " - Activity: " + activityDismissed.ToString();
	}
	
	private void NotifyActivityPresented(ActivityType activityPresented)
	{
		Debug.Log("Event: NotifyActivityPresented Fired");
		eventsAndCallbacks.eventNotifyActivityPresented.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s" + " - Activity: " + activityPresented.ToString();
	}
	
	private void NotifySessionError(int errorCode, string error)
	{
		Debug.Log("Event: NotifySessionError Fired");
		eventsAndCallbacks.eventNotifySessionError.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s"+ "Error Code: " + errorCode.ToString() + " - Error: " + error;
	}
	
	private void NotifySessionStateChanged(SessionState state)
	{
		Debug.Log("Event: NotifySessionStateChanged Fired");
		eventsAndCallbacks.eventNotifySessionChanged.text = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s" + " - State: " + state.ToString();
	}
	
	private void NotifyUnclaimedAchievementDataUpdated(IAchievementData achievementData)
	{
		Debug.Log("Event: NotifyUnclaimedAchievementDataUpdated Fired");
		string fullAchData = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s";
		fullAchData += " | Name: " + achievementData.GetName();
		fullAchData += " | Val: " + achievementData.GetMpointValue();
		fullAchData += " | Message: " + achievementData.GetMessage();
		fullAchData += " | IsCustom: " + achievementData.IsCustom().ToString();
		eventsAndCallbacks.eventNotifyUnclaimedAchievementDataUpdated.text = fullAchData;
	}
	
	private void NotifyUserAction(UserAction arg1, IDictionary<string, object> arg2)
	{
		Debug.Log("Event: NotifyUserAction Fired");
		string userAction = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s";
		userAction += " | UserAction: " + arg1;
		foreach(KeyValuePair<string, object> entry in arg2) {
			userAction += " | " + entry.Key;
			userAction += " - " + entry.Value;
		}
		eventsAndCallbacks.eventNotifyUseraction.text = userAction;
	}
	
	private void NotifyUserInfoChanged(IDictionary<string, object> newUserInfo)
	{
		Debug.Log("Event: NotifyUserInfoChanged Fired");
		string userInfo = "Fired@" + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString() + "s";
		foreach(KeyValuePair<string, object> entry in newUserInfo) {
			userInfo += " | " + entry.Key;
			userInfo += " - " + entry.Value;
		}
		
		eventsAndCallbacks.eventNotifyUserInfoChanged.text = userInfo;
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
	
	private void Update()
	{
		unclaimedAchievements.text = SessionM.GetInstance().GetUnclaimedAchievementCount().ToString();
		activityAvailableAchievement.text = SessionM.GetInstance().IsActivityAvailable(ActivityType.Achievement) ? "yes" : "no";
		activityAvailableInterstitial.text = SessionM.GetInstance().IsActivityAvailable(ActivityType.Interstitial) ? "yes" : "no";
		activityAvailableIntro.text = SessionM.GetInstance().IsActivityAvailable(ActivityType.Introduction) ? "yes" : "no";
		activityAvailablePortal.text = SessionM.GetInstance().IsActivityAvailable(ActivityType.Portal) ? "yes" : "no";
		
		UpdateAchievementGUI();
	}
	
	public UILabel achNameLabel;
	public UILabel achMessageLabel;
	public UILabel achMPointsLabel;
	public UILabel achIdentiferLabel;
	public UILabel achIsCustomLabel;
	
	private void UpdateAchievementGUI()
	{
		unclaimedAch = SessionM.GetInstance().GetUnclaimedAchievementData();
		if (unclaimedAch == null) {
			achNameLabel.text = "null";
			achMessageLabel.text = "null";
			achMPointsLabel.text = "null";
			achIdentiferLabel.text = "null";
			achIsCustomLabel.text = "null";
			return;
		}
		
		achNameLabel.text = unclaimedAch.GetName();
		achMessageLabel.text = unclaimedAch.GetMessage();
		achMPointsLabel.text = unclaimedAch.GetMpointValue().ToString();
		achIdentiferLabel.text = unclaimedAch.Identifier();
		achIsCustomLabel.text = unclaimedAch.IsCustom().ToString();
	}
}
