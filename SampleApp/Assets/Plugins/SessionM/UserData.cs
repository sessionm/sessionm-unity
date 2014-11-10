using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class UserData
{
	
	private bool isOptedOut;
	private bool isRegistered;
	private bool isLoggedIn;
	private int pointBalance;
	private int unclaimedAchievementCount;
	private int unclaimedAchievementValue;
	
	public UserData(bool isOptedOut, bool isRegistered, bool isLoggedIn, int pointBalance, int unclaimedAchievementCount, int unclaimedAcheivementValue) 
	{
		this.isOptedOut = isOptedOut;
		this.isRegistered = isRegistered;
		this.isLoggedIn = isLoggedIn;
		this.pointBalance = pointBalance;
		this.unclaimedAchievementCount = unclaimedAchievementCount;
		this.unclaimedAchievementValue = unclaimedAcheivementValue;
	}

	public bool IsOptedOut() { return this.isOptedOut; }
	public bool IsRegistered() { return this.isRegistered; }
	public bool IsLoggedIn() { return this.isLoggedIn; }
	public int GetUserPointBalance() { return this.pointBalance; }
	public int GetUnclaimedAchievementCount() { return this.unclaimedAchievementCount; }
	public int GetUnclaimedAchievementValue() { return this.unclaimedAchievementValue; }

}
