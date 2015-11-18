using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserData
{
	
	private bool isOptedOut;
	private bool isRegistered;
	private bool isLoggedIn;
	private int pointBalance;
	private int unclaimedAchievementCount;
	private int unclaimedAchievementValue;
	private List<AchievementData> achievements;
	private List<AchievementData> achievementsList;
	private string tierName;
	private string tierPercentage;
	private string tierAnniversaryDate;

	public UserData(bool isOptedOut, bool isRegistered, bool isLoggedIn, int pointBalance, int unclaimedAchievementCount, int unclaimedAcheivementValue, List<AchievementData> achievements, List<AchievementData> achievementsList, string tierName, string tierPercentage, string tierAnniversaryDate)
	{
		this.isOptedOut = isOptedOut;
		this.isRegistered = isRegistered;
		this.isLoggedIn = isLoggedIn;
		this.pointBalance = pointBalance;
		this.unclaimedAchievementCount = unclaimedAchievementCount;
		this.unclaimedAchievementValue = unclaimedAcheivementValue;
		this.achievements = achievements;
		this.achievementsList = achievementsList;
		this.tierName = tierName;
		this.tierPercentage = tierPercentage;
		this.tierAnniversaryDate = tierAnniversaryDate;
	}

	public bool IsOptedOut() { return this.isOptedOut; }
	public bool IsRegistered() { return this.isRegistered; }
	public bool IsLoggedIn() { return this.isLoggedIn; }
	public int GetUserPointBalance() { return this.pointBalance; }
	public int GetUnclaimedAchievementCount() { return this.unclaimedAchievementCount; }
	public int GetUnclaimedAchievementValue() { return this.unclaimedAchievementValue; }
	public List<AchievementData> GetAchievements() { return this.achievements; }
	public List<AchievementData> GetAchievementsList() { return this.achievementsList; }
	public string GetTierName() {
		return this.tierName;
	}
	public string GetTierPercentage() {
		return this.tierPercentage;
	}
	public string GetTierAnniversaryDate() {
		return this.tierAnniversaryDate;
	}

}
