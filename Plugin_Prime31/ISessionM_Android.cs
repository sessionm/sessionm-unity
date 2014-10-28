using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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
	private AndroidJavaClass sessionMObject = new AndroidJavaClass ("com.sessionm.unity.SessionMPlugin");
	private Boolean isPresented = false;
	
	public ISessionM_Android(SessionM sessionMParent)
	{
		sessionMGameObject = sessionMParent;
		initAndroidInstance();
		
		CreateListenerObject();
		
		if(sessionMGameObject.androidAppId != null) {
			StartSession(null);
		}
	}
	
	private void CreateListenerObject()
	{
		listener = sessionMGameObject.gameObject.AddComponent<SessionMEventListener>();

		sessionMObject.CallStatic("setCallbackGameObjectName", sessionMGameObject.gameObject.name);
		
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
	
	public int GetUnclaimedAchievementCount()
	{
		int count = 0;

			count = sessionMObject.CallStatic<int>("getUnclaimedAchievementCount");			
		
		return count;
	}
	
	public string GetUnclaimedAchievementData() 
	{
		string achievementJSON = null;

		achievementJSON = sessionMObject.CallStatic<string>("getUnclaimedAchievementJSON");
		
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
		using (AndroidJavaObject activityType = GetAndroidActivityTypeObject(type)) {
			isPresented = androidInstance.Call<bool>("presentActivity", activityType);			
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
		using (AndroidJavaObject activityType = GetAndroidActivityTypeObject(type)) {
			available = sessionMObject.CallStatic<bool>("isActivityAvailable", activityType);			
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
	
	public void SetMetaData(string data, string key)
	{
		androidInstance.Call("setMetaData", key, data);
	}
	
	public void NotifyPresented()
	{
		isPresented = sessionMObject.CallStatic<bool>("notifyCustomAchievementPresented");
	}
	
	public void NotifyDismissed()
	{
		if (isPresented) {
			sessionMObject.CallStatic ("notifyCustomAchievementCancelled");
			isPresented = false;
		}
	}
	
	public void NotifyClaimed()
	{
		if (isPresented) {
			sessionMObject.CallStatic ("notifyCustomAchievementClaimed");
			isPresented = false;
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

	public AndroidJavaObject GetCurrentActivity() 
	{
		using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			return playerClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}
	
	// MonoBehavior 
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