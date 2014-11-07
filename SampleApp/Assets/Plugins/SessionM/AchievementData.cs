using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class AchievementData : IAchievementData
{
	
	private string identifier = null;
	private string name = null;
	private string message = null;
	private int mpointValue;
	private bool isCustom ;
	
	public AchievementData(string identifier, string name, string message, int mpointValue, bool isCustom) 
	{
		this.identifier = identifier;
		this.name = name;
		this.message = message;
		this.mpointValue = mpointValue;
		this.isCustom = isCustom;
	}
	
	public string Identifier() { return identifier; }
	public string GetName() { return name; }
	public string GetMessage() { return message; }
	public int GetMpointValue() { return mpointValue; }
	public bool IsCustom() { return isCustom;	 }
}
