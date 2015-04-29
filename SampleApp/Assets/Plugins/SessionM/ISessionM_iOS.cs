using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniJSON;

#if UNITY_IOS
/*
 * SessionM iOS Native Implementation.
 */ 
public class ISessionM_iOS : ISessionM
{	
	private SessionM sessionMGameObject;
	private ISessionMCallback callback;
	private SessionMEventListener listener;
	
	[DllImport ("__Internal")]
	protected static extern void SMSetCallbackGameObjectName(string gameObjectName);
	public ISessionM_iOS(SessionM sessionMParent) 
	{
		sessionMGameObject = sessionMParent;
		
		if(sessionMParent.iosAppId != null) {
			StartSession(null);
			SetLogLevel(sessionMParent.logLevel);
		}
		
		CreateListenerObject();
	}
	
	private void CreateListenerObject()
	{
		listener = sessionMGameObject.gameObject.AddComponent<SessionMEventListener>();
		
		SMSetCallbackGameObjectName(sessionMGameObject.gameObject.name);
		Debug.Log("Setting Callback Object: " + sessionMGameObject.gameObject.name);
		listener.SetNativeParent(this);
		
		if(callback != null) {
			listener.SetCallback(callback);
		}
	}
	
	[DllImport ("__Internal")]
	private static extern void SMStartSession(string appId);
	public void StartSession(string appId)
	{
		if(sessionMGameObject.iosAppId != null) {
			SMStartSession(sessionMGameObject.iosAppId);
		} else {
			SMStartSession(appId);
		}
	}
	
	[DllImport ("__Internal")]
	private static extern int SMGetSessionState();
	public SessionState GetSessionState()
	{
		return (SessionState)SMGetSessionState();
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMPlayerDataGetUserOptOutStatus();
	public bool GetUserOptOutStatus()
	{
		return SMPlayerDataGetUserOptOutStatus();
	}

	[DllImport ("__Internal")]
	private static extern string SMGetUserJSON();
	public string GetUser() 
	{
		return SMGetUserJSON(); 
	}

	[DllImport ("__Internal")]
	private static extern int SMPlayerDataGetUnclaimedAchievementCount();
	public int GetUnclaimedAchievementCount()
	{
		return SMPlayerDataGetUnclaimedAchievementCount();
	}
	
	[DllImport ("__Internal")]
	private static extern string SMGetUnclaimedAchievementJSON();
	public string GetUnclaimedAchievementData() 
	{
		return SMGetUnclaimedAchievementJSON(); 
	}
	
	[DllImport ("__Internal")]
	private static extern void SMLogAction(string action);
	public void LogAction(string action) 
	{
		SMLogAction(action);		
	}
	
	[DllImport ("__Internal")]
	private static extern void SMLogActions(string action, int count);
	public void LogAction(string action, int count) 
	{
		SMLogActions(action, count);		
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMPresentActivity(int type);
	public bool PresentActivity(ActivityType type)
	{
		return SMPresentActivity((int)type);
	}
	
	[DllImport ("__Internal")]
	private static extern void SMDismissActivity();
	public void DismissActivity()
	{
		SMDismissActivity();
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMIsActivityPresented();
	public bool IsActivityPresented()
	{
		return SMIsActivityPresented();
	}
	
	[DllImport ("__Internal")]
	private static extern bool SMIsActivityAvailable(int type); 
	public bool IsActivityAvailable(ActivityType type)
	{
		return SMIsActivityAvailable((int)type); 
	}
	
	[DllImport ("__Internal")]
	private static extern void SMSetLogLevel(int level);
	public void SetLogLevel(LogLevel level)
	{
		SMSetLogLevel((int)level);	
	}
	
	[DllImport ("__Internal")]
	private static extern int SMGetLogLevel();
	public LogLevel GetLogLevel()
	{
		return (LogLevel)SMGetLogLevel();
	}
	
	[DllImport ("__Internal")]
	private static extern string SMGetSDKVersion();
	public string GetSDKVersion()
	{
		return SMGetSDKVersion();
	}
	
	[DllImport ("__Internal")]
	private static extern void SMSetMetaData(string data, string key);
	public void SetMetaData(string data, string key)
	{
		SMSetMetaData(data, key);
	}
	
	[DllImport ("__Internal")]
	private static extern void SMNotifyCustomAchievementPresented();
	public void NotifyPresented()
	{
		SMNotifyCustomAchievementPresented();
	}
	
	[DllImport ("__Internal")]
	private static extern void SMNotifyCustomAchievementDismissed();
	public void NotifyDismissed()
	{
		SMNotifyCustomAchievementDismissed();
	}
	
	[DllImport ("__Internal")]
	private static extern void SMNotifyCustomAchievementClaimed();
	public void NotifyClaimed()
	{
		SMNotifyCustomAchievementClaimed();
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
	
}
#endif