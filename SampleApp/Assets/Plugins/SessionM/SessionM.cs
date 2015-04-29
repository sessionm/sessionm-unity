using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniJSON;

/*
 * Session M service implementation. Implements and provides access (via SessionM.GetInstance()) to the SessionM service.
 */ 
public class SessionM : MonoBehaviour
{
	private ISessionMCallback callback;
	
	public string iosAppId;
	public string androidAppId;
	public LogLevel logLevel;
	
	
	//The SessionM Class is a monobehaviour Singleton.  Drop the Gameobject in your scene and set it up instead of trying to instantiate it via code.
	//Put the SessionM object as early in your project as possible.  The object will survive loads, so there's never a reason to put it in more than one place in your scenes.
	private static SessionM instance;
	public static SessionM GetInstance() 
	{
		if(instance == null) {
			SessionM existingSessionM = GameObject.FindObjectOfType<SessionM>();
			if(existingSessionM == null) {
				Debug.LogError("There is no SessionM GameObject set up in the scene.  Please add one and set it up as per the SessionM Plug-In Documentation.");
				return null;
			}
			existingSessionM.SetSessionMNative();
			instance = existingSessionM;
		}
		
		return instance;
	}
	
	//Here, SessionM instantiates the appropiate Native interface to be used on each platform.
	//iOS: iSessionM_IOS
	//Android: iSessionM_Android
	//All others: iSessionM_Dummy (The Dummy simply catches all calls coming into SessionM un unsupported platforms.)
	//If you need to modify how SessionM is interacting with either iOS or Android natively, please look in the respective Interface Class.
	private ISessionM sessionMNative;
	public ISessionM SessionMNative 
	{
		get { return sessionMNative; }
	}
	
	//The following methods can be called at anytime via the SessionM Singleton.
	//For instance, you can call GetSessionState from anywhere in Unity program by aclling SessionM.GetInstance().GetSessionState()
	
	//Returns SessionM's current SessionState
	//Can be: Stopped, Started Online, Started Offline
	//Use this method to determine if your user is in a valid region for SessionM.  If SessionM is in a Stopped State, you should 
	//suppress SessionM elements.
	public SessionState GetSessionState()
	{
		return sessionMNative.GetSessionState();
	}

	//Returns user opt out status.
	public bool GetUserOptOutStatus(){
		return sessionMNative.GetUserOptOutStatus();
	}
	
	//Use this method for displaying a badge or other SessionM tools.  Remember, your Acheivement count can accumulate over days, so be sure to support at least
	//triple digit numbers.
	public int GetUnclaimedAchievementCount()
	{
		return sessionMNative.GetUnclaimedAchievementCount();
	}

	public UserData GetUserData()
	{
		UserData userData = null;
		string userDataJSON = null;

		userDataJSON = sessionMNative.GetUser();

		if(userDataJSON == null) {
			return null;
		}

		userData = GetUserData(userDataJSON);

		return userData;
	}

	//This method is required for displaying Native Acheivements.  Fore more information, please see the Unity plugin documetnation.
	public AchievementData GetUnclaimedAchievementData() 
	{
		IAchievementData achievementData = null;
		string achievementJSON = null;
		
		achievementJSON = sessionMNative.GetUnclaimedAchievementData();
		
		if(achievementJSON == null) {
			return null;
		}
		
		achievementData = GetAchievementData(achievementJSON);
		return achievementData as AchievementData;
	}
	
	//This method is vital to using SessionM, whenever your user completes an action that contributes towards a SessionM Acheivement
	//report it to SessionM using this method.
	public void LogAction(string action) 
	{
		sessionMNative.LogAction(action);
	}
	
	//You can use this method if multiple actions were achieved simultaneously.
	public void LogAction(string action, int count) 
	{
		sessionMNative.LogAction(action, count);
	}
	
	//Use this method to display an Acheivement if there is an unclaimed Achievement ready.  SessionM will automatically display an overlay
	//Acheivement display for you.  You can see if there is an achievement ready by running the IsActivityAvailable method below.
	public bool PresentActivity(ActivityType type)
	{
		return sessionMNative.PresentActivity(type);
	}
	
	public bool IsActivityAvailable(ActivityType type)
	{
		return sessionMNative.IsActivityAvailable(type);
	}
	
	//Use this to display the SessionM Portal.  You can use this after users have clicked on a Native Acheivement, or when they click on a SessionM
	//button in your app.
	public bool ShowPortal()
	{
		return PresentActivity(ActivityType.Portal);
	}
	
	//The following methods are generally used for debugging and won't be utilized by most SessionM Developers.
	
	public string GetSDKVersion()
	{
		return sessionMNative.GetSDKVersion();
	}
	
	public LogLevel GetLogLevel()
	{
		return sessionMNative.GetLogLevel();
	}
	
	public void SetLogLevel(LogLevel level)
	{
		//Note Log Level only works on iOS.  For Android, use logcat.
		//LogLevel can also be set on the SessionM Object.
		sessionMNative.SetLogLevel(level);
	}
	
	public bool IsActivityPresented()
	{
		return sessionMNative.IsActivityPresented();
	}
	
	public void SetMetaData(string data, string key)
	{
		sessionMNative.SetMetaData(data, key);
	}
	
	public void NotifyPresented()
	{
		sessionMNative.NotifyPresented();
	}
	
	public void NotifyDismissed()
	{
		sessionMNative.NotifyDismissed();
	}
	
	public void NotifyClaimed()
	{
		sessionMNative.NotifyClaimed();
	}
	
	public void DismissActivity()
	{
		sessionMNative.DismissActivity();
	}
	
	public void SetCallback(ISessionMCallback callback)
	{
		sessionMNative.SetCallback(callback);
	}
	
	public ISessionMCallback GetCallback() 
	{
		return sessionMNative.GetCallback();
	}
	
	// Unity Lifecycle
	
	private void Awake() 
	{
		SetSessionMNative();
		GameObject.DontDestroyOnLoad(this.gameObject);
		instance = this;
		SetLogLevel (logLevel);
	}
	
	private void SetSessionMNative()
	{
		if(sessionMNative != null)
			return;
		
		//Assign the appropiate Native Class to handle method calls here.
		#if UNITY_EDITOR
		sessionMNative = new ISessionM_Dummy();
		#elif UNITY_IOS
		sessionMNative = new ISessionM_iOS(this);
		#elif UNITY_ANDROID
		sessionMNative = new ISessionM_Android(this);
		#else
		sessionMNative = new ISessionM_Dummy();
		#endif
	}
	
	//This is a useful method you can call whenever you need to parse a JSON string into a the IAchievementData custom class.
	public static IAchievementData GetAchievementData(string jsonString) 
	{
		Dictionary<string, object> achievementDict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		long mpointValue = (Int64)achievementDict["mpointValue"];
		bool isCustom = (bool)achievementDict["isCustom"];
		string identifier = (string)achievementDict["identifier"];
		string name = (string)achievementDict["name"];
		string message = (string)achievementDict["message"];
		IAchievementData achievementData = new AchievementData(identifier, name, message, (int)mpointValue, isCustom);
		return achievementData;
	}

	public static UserData GetUserData(string jsonString)
	{
		Dictionary<string, object> userDict = Json.Deserialize(jsonString) as Dictionary<string, object>;
		bool isOptedOut = (bool)userDict["isOptedOut"];
		bool isRegistered = (bool)userDict["isRegistered"];
		bool isLoggedIn = (bool)userDict["isLoggedIn"];
		long userPointBalance = (Int64)userDict["getPointBalance"];
		long unclaimedAchievementCount = (Int64)userDict["getUnclaimedAchievementCount"];
		long unclaimedAchievementValue = (Int64)userDict["getUnclaimedAchievementValue"];

		UserData userData = new UserData(isOptedOut, isRegistered, isLoggedIn, (int)userPointBalance, (int)unclaimedAchievementCount, (int)unclaimedAchievementValue);
		return userData;
	}
}