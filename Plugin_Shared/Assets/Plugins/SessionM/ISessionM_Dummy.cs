using System;

//SessionMDummy.cs
//
//Dummy class used for platforms SessionM does not support and in editor mode.

public class ISessionM_Dummy : ISessionM
{
	public ISessionM_Dummy()
	{
	}
	
	public void StartSession(string appId)
	{
	}
	
	public SessionState GetSessionState()
	{
		return SessionState.Stopped;
	}
	
	public bool GetUserOptOutStatus()
	{
		return false;
	}

	public string GetUser() 
	{
		return null;
	}
	
	public int GetUnclaimedAchievementCount()
	{
		return 0;
	}
	
	public string GetUnclaimedAchievementData() 
	{
		return null;
	}
	
	
	public void LogAction(string action) 
	{
	}
	
	public void LogAction(string action, int count) 
	{	
	}
	
	public bool PresentActivity(ActivityType type)
	{
		return false;
	}
	
	public void DismissActivity()
	{
	}
	
	public bool IsActivityPresented()
	{
		return false;
	}
	
	public bool IsActivityAvailable(ActivityType type)
	{
		return false;
	}
	
	public void SetLogLevel(LogLevel level)
	{
	}
	
	public LogLevel GetLogLevel()
	{	
		return LogLevel.Off;
	}
	
	public string GetSDKVersion()
	{
		return null;
	}
	
	public void SetMetaData(string data, string key)
	{
	}
	
	public void NotifyPresented()
	{
	}
	
	public void NotifyDismissed()
	{
	}
	
	public void NotifyClaimed()
	{
	}
	
	public void SetCallback(ISessionMCallback callback) 
	{
	}
	
	public ISessionMCallback GetCallback() 
	{
		return null;
	}
	
}

