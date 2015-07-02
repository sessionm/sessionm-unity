using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using MiniJSON;

#if UNITY_ANDROID
/*
 * SessionM Android Native Implementation.
 */ 
public class ISessionM_Android : ISessionM
{	
	private SessionM sessionMGameObject;
	private ISessionMCallback callback;
	private SessionMEventListener listener;
	
	private static AndroidJavaObject androidInstance;
	
	private Boolean isPresented = false;
	
	public ISessionM_Android(SessionM sessionMParent)
	{
		sessionMGameObject = sessionMParent;
		
		initAndroidInstance();
		
		CreateListenerObject();
		
		if(sessionMGameObject.androidAppId != null) {
			SetServiceRegion(SessionM.serviceRegion);
			StartSession(null);
		}
	}
	
	private void CreateListenerObject()
	{
		listener = sessionMGameObject.gameObject.AddComponent<SessionMEventListener>();
		
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			activityObject.CallStatic("setCallbackGameObjectName", sessionMGameObject.gameObject.name);
		}
		
		listener.SetNativeParent(this);
		
		if(callback != null) {
			listener.SetCallback(callback);
		}
	}
	
	public void StartSession(string appId)
	{
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			if(sessionMGameObject.androidAppId != null) {
				androidInstance.Call("startSession", activityObject, sessionMGameObject.androidAppId);			
			} else {
				androidInstance.Call("startSession", activityObject, appId);
			}
		}
	}
	
	public SessionState GetSessionState()
	{
		SessionState state = SessionState.Stopped;
		
		using (AndroidJavaObject stateObject = androidInstance.Call<AndroidJavaObject>("getSessionState")) {
			string stateName = stateObject.Call<string>("name");
			if(stateName.Equals("STOPPED")) {
				state = SessionState.Stopped;
			} else if(stateName.Equals("STARTED_ONLINE")) {
				state = SessionState.StartedOnline;
			} else if(stateName.Equals("STARTED_OFFLINE")) {
				state = SessionState.StartedOffline;
			}
		}
		
		return state;
	}
	
	public string GetUser()
	{
		string userJSON = null;
		
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			userJSON = activityObject.Call<string>("getUser");
		}
		
		return userJSON;
	}

	public void SetUserOptOutStatus(bool status){
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			activityObject.Call("setUserOptOutStatus", status);
		}
	}
	
	public void SetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate)
	{
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			activityObject.Call("setShouldAutoUpdateAchievementsList", shouldAutoUpdate);                   
		}
	}
	
	public void UpdateAchievementsList()
	{
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			activityObject.Call("updateAchievementsList");                  
		}
	}
	
	public int GetUnclaimedAchievementCount()
	{
		int count = 0;
		
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			count = activityObject.Call<int>("getUnclaimedAchievementCount");			
		}
		
		return count;
	}
	
	public string GetUnclaimedAchievementData() 
	{
		string achievementJSON = null;
		
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			achievementJSON = activityObject.Call<string>("getUnclaimedAchievementJSON");			
		}
		
		return achievementJSON;
	}
	
	
	public void LogAction(string action) 
	{
		androidInstance.Call("logAction", action);
	}
	
	public void LogAction(string action, int count) 
	{
		androidInstance.Call("logAction", action, count);
	}
	
	public bool PresentActivity(ActivityType type)
	{
		using (AndroidJavaObject activityType = GetAndroidActivityTypeObject(type),
		       activityObject = GetCurrentActivity()) {
			isPresented = activityObject.Call<bool>("presentActivity", activityType);			
		}
		return isPresented;
	}
	
	public void DismissActivity()
	{
		if (isPresented) {
			androidInstance.Call ("dismissActivity");
			isPresented = false;
		}
	}
	
	public bool IsActivityPresented()
	{
		bool presented = false;
		presented = androidInstance.Call<bool>("isActivityPresented");
		
		return presented;
	}
	
	public bool IsActivityAvailable(ActivityType type)
	{
		bool available = false;

		using (AndroidJavaObject activityType = GetAndroidActivityTypeObject(type),
		       activityObject = GetCurrentActivity()) {
			available = activityObject.Call<bool>("isActivityAvailable", activityType);			
		}
		return available;
	}
	
	public void SetLogLevel(LogLevel level)
	{
		// use logcat on Android
	}
	
	public LogLevel GetLogLevel()
	{
		return LogLevel.Off;
	}

	public string GetSDKVersion()
	{
		return androidInstance.Call<string>("getSDKVersion");			
	}
	
	public string GetRewards()
	{
		string rewardsJSON = null;
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			rewardsJSON = activityObject.Call<string>("getRewardsJSON");
		}
		return rewardsJSON;
	}
	
	public void SetMetaData(string data, string key)
	{
		androidInstance.Call("setMetaData", key, data);
	}

	public void SetServiceRegion(ServiceRegion serviceRegion)
	{
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
            //Always 0 for now
			activityObject.Call("setServiceRegion", 0);                  
		}
	}
	
	public void NotifyPresented()
	{
		using (AndroidJavaObject activityObject = GetCurrentActivity()) {
			isPresented = activityObject.Call<bool>("notifyCustomAchievementPresented");
		}
	}
	
	public void NotifyDismissed()
	{
		if (isPresented) {
			using (AndroidJavaObject activityObject = GetCurrentActivity()) {
				activityObject.Call ("notifyCustomAchievementCancelled");
				isPresented = false;
			}
		}
	}
	
	public void NotifyClaimed()
	{	
		if (isPresented) {
			using (AndroidJavaObject activityObject = GetCurrentActivity()) {
				activityObject.Call ("notifyCustomAchievementClaimed");
				isPresented = false;
			}
		}
	}
	
	public void SetCallback(ISessionMCallback callback) 
	{
		this.callback = callback;
		listener.SetCallback(callback);
	}
	
	public ISessionMCallback GetCallback() 
	{
		return this.callback;
	}
	
	// MonoBehavior 
	
	public AndroidJavaObject GetCurrentActivity() 
	{
		using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			return playerClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}
	
	private AndroidJavaObject GetAndroidActivityTypeObject(ActivityType type) 
	{
		if(Application.platform != RuntimePlatform.Android) {
			return null;	
		}
		
		using (AndroidJavaClass typeClass = new AndroidJavaClass("com.sessionm.api.SessionM$ActivityType")) {
			string typeString = null;
			if(type == ActivityType.Achievement) {
				typeString = "ACHIEVEMENT";	
			} else if(type == ActivityType.Portal) {
				typeString = "PORTAL";	
			}
			
			AndroidJavaObject activityType = typeClass.CallStatic<AndroidJavaObject>("valueOf", typeString);
			return activityType;		
		}
	}
	
	protected static void initAndroidInstance()
	{
		using (AndroidJavaClass sessionMClass = new AndroidJavaClass("com.sessionm.api.SessionM")) {
			androidInstance = sessionMClass.CallStatic<AndroidJavaObject>("getInstance"); 
		}
	}
}
#endif
