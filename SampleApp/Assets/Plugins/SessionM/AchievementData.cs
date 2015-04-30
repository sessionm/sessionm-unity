using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class AchievementData : IAchievementData
{
	
	private string identifier = null;
	private string importID = null;
	private string instructions = null;
	private string achievementIconURL = null;
	private string action = null;
	private string name = null;
	private string message = null;
	private string limitText = null;
	private int mpointValue;
	private bool isCustom;
	private DateTime lastEarnedDate;
	private int timesEarned;
	private int unclaimedCount;
	private int distance;
	
	public AchievementData(string identifier, string importID, string instructions, string achievementIconURL, string action, string name, string message, string limitText, int mpointValue, bool isCustom, DateTime lastEarnedDate, int timesEarned, int unclaimedCount, int distance)
	{
		this.identifier = identifier;
		this.importID = importID;
		this.instructions = instructions;
		this.achievementIconURL = achievementIconURL;
		this.action = action;
		this.name = name;
		this.message = message;
		this.limitText = limitText;
		this.mpointValue = mpointValue;
		this.isCustom = isCustom;
		this.lastEarnedDate = lastEarnedDate;
		this.timesEarned = timesEarned;
		this.unclaimedCount = unclaimedCount;
		this.distance = distance;
	}
	
	public string GetIdentifier() { return identifier; }
	public string GetImportID() { return importID; }
	public string GetInstructions() { return instructions; }
	public string GetAchievementIconURL() { return achievementIconURL; }
	public string GetAction() { return action; }
	public string GetName() { return name; }
	public string GetMessage() { return message; }
	public string GetLimitText() { return limitText; }
	public int GetMpointValue() { return mpointValue; }
	public bool GetIsCustom() { return isCustom; }
	public DateTime GetLastEarnedDate() { return lastEarnedDate; }
	public int GetTimesEarned() { return timesEarned; }
	public int GetUnclaimedCount() { return unclaimedCount; }
	public int GetDistance() { return distance; }
}
