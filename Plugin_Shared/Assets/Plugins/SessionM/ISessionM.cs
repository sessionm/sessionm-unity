using UnityEngine;
using System.Collections;

// UI Activity type
public enum ActivityType
{
	// User achievement.  
	Achievement = 1,
	// User portal.
	Portal = 2
}

// Log levels
public enum LogLevel
{
	Off = 0,
	Info = 1, 
	Debug = 3, 
}

// Session state
public enum SessionState
{
	Stopped = 0,
	StartedOnline = 1, 
	StartedOffline = 2
}

// Service region
public enum ServiceRegion
{
	Unknown = 0,
	Japan = 1,
	USA = 2
}

/* User actions. 
 * User action within Session M UI activity. Identifies events such as user engaging with achievement prompt, etc.
 */ 
public enum UserAction
{
	AchievementViewAction = 100,
	AchievementEngagedAction = 101,
	AchievementDismissedAction = 102,
	SponsorContentViewedAction = 103,
	SponsorContentEngagedAction = 104,
	SponsorContentDismissedAction = 105,
	PortalViewedAction = 106,
	SignInAction = 107,
	SignOutAction = 108,
	RegisteredAction = 109,
	PortalDismissedAction = 110,
	RedeemedRewardAction = 111,
	CheckinCompletedAction = 112,
	VirtualItemRewardAction = 113
}

/*
 * Session M service interface. 
 *
 * In order to enable session M service in your Unity application make sure to do the following:
 * 1. Register with Session M developer portal and obtain valid application ID for your application. 
 * 2. Copy SessionM Unity plugin into your application project. More information about Unity plugin integration can be found at http://unity3d.com/support/documentation/Manual/Plugins.html.
 * 3. In the Unity editor associate SessionM.cs script with a game object in your application. Any game object can be used for this purpose but should preferrably be a long-lived one, such as main camera. 
 *    This enables service callbacks into ISessionMCallback instance from the native application layer. 
 * 3. Optionally, use SessionMManager.cs to jump-start SessionM service integration. Associate this script with one of the game objects in your application and inject your application logic in 
 *    appropriate places in the script to get your SessionM enabled application up and running.   
 * 
 * Following is the basic pattern of integrating Session M service within your appliction code:
 * 1. Configure SessionM script associated with the game object with a valid application ID. 
 * 2. Set callback object to get notified about service events and, in particular, earned achievements.
 *    ISessionMCallback callback = ... <your callback object>
 *    service.SetCallback(callback);
 * 3. Log user actions in your application code earning user achievements, e.g. reaching new game level, daily visit, killing the scariest monster, etc.        
 *    service.LogAction("Level X");
 * 4. Present an UI activity, such as achievement when it becomes available, user portal or Session M program introduction at the appropriate time in your application flow. 
 *    if(service.IsActivityAvailable(ActivityType.Achievement)) {
 *        service.PresentActivity(ActivityType.Achievement);
 *    }
 * 
 * Achievement UI Customization.
 * 
 * Application can customize achievement presentation to suit its style and UI flow by implementing the following steps:
 * 1. In developer portal configure an achievement as custom.
 * 2. Use object IAchievementData to access information about achievement and method NotifyUnclaimedAchievementDataUpdated(ISessionM, IAchievementData) in ISessionMCallback interface to get notified when new achievement is earned. 
 * 3. When displaying an achievement UI make sure to call the following methods on IAchievementData object:
 *    a. NotifyPresented() when achievement alert UI has been displayed. 
 *    b. NotifyDismissed() when user or application has dismissed the achievement alert without engaging. 
 *    c. NotifyClaimed() when user claimed the achievement. 
 */ 
public interface ISessionM
{
	
	// Sets callback object.
	void SetCallback(ISessionMCallback callback); 
	
	ISessionMCallback GetCallback(); 
	
	// Starts session with an application identifier.
	void StartSession(string appId);
	
	// Returns session state
	SessionState GetSessionState();

	//Gets Current User JSON Object (Deserializes to UserData)
	string GetUser();

	//Sets Current user opt-out status locally
	void SetUserOptOutStatus(bool status);

	//Sets value of shouldAutoUpdateAchievementsList (default is false)
	void SetShouldAutoUpdateAchievementsList(bool shouldAutoUpdate);

	//Manually updates user's achievementsList field. Has no effect is shouldAutoUpdateAchievementsList is set to true.
	void UpdateAchievementsList();

	// Returns number of unclaimed achievements
	int GetUnclaimedAchievementCount();
	
	// Returns current unclaimed achievement data
	string GetUnclaimedAchievementData();
	
	// Logs action. Calling this method may trigger an achievement which application will be notified about via ISessionMCallback.NotifyUnclaimedAchievementDataUpdated(ISessionM,IAchievementData) callback.  
	void LogAction(string action); 
	
	// Logs a number of actions - equivalent of calling LogAction(string) a number of times specified in argument "count".
	void LogAction(string action, int count);
	
	// Presents UI activity of specified type if available. 
	bool PresentActivity(ActivityType type);
	// Dismisses currently presented UI activity.
	void DismissActivity();
	
	// Returns true if specified UI activity is available for presentation, false - otherwise. User portal (ActivityType.Portal) is always available. 
	// Achievement activity (ActivityType.Achievement) become available when new achievement is earned as a result of user action.  
	bool IsActivityAvailable(ActivityType type);
	// Returns true if UI activity is currently being presented, false - otherwise.
	bool IsActivityPresented();
	
	// Return logging level (iOS only)
	LogLevel GetLogLevel();
	// Set logging level (iOS only)
	void SetLogLevel(LogLevel level);

	// Sets service region
	void SetServiceRegion(ServiceRegion region);

	// Returns SDK version 
	string GetSDKVersion();
	
        // Returns a list of all rewards the user can redeem
	string GetRewards();

	// Set meta data properties
	void SetMetaData(string data, string key);
	
	// Notifies the SessionM SDK that a custom achievement has been presented
	void NotifyPresented();
	
	// Notifies the SessionM SDK that a custom achievement has been dismissed
	void NotifyDismissed();
	
	// Notifies the SessionM SDK that a custom achievement has been claimed
	void NotifyClaimed();
}

