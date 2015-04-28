using System;
using UnityEngine;

/*
 * Achievement data interface. 
 * 
 * Defines information about achievement used to customize achievement alert presentation.
 */ 
public interface IAchievementData
{
	/*
	 * Achievement identifier string.
	 */
	string GetIdentifier();
	/*
	 * ID assigned to the achievement in the csv file exported from the SessionM Developer Portal.
	 */
	string GetImportID();
	/*
	 * Instructions explaining how to earn the achievement.
	 */
	string GetInstructions();
	/*
	 * The path to the achievement's icon.
	 */
	string GetAchievementIconURL();
	/*
	 * The name of the action performed by the user to earn the achievement.
	 */
	string GetAction();
	/*
	 * Achievement name.
	 */
	string GetName();
	/*
	 * Achievement message.
	 */ 
	string GetMessage();
	/*
	 * Description of amount of times the achievement can be earned (e.g. "Once" or "1 time every day").
	 */
	string GetLimitText();
	/*
	 * mPoint value.
	 */
	int GetMpointValue();
	/*
	 * Boolean indicating if achievement is custom. 
	 */
	bool GetIsCustom();
	/*
	 * The last date the achievement was earned.
	 */
	DateTime GetLastEarnedDate();
	/*
	 * The amount of times the achievement has been earned by the user.
	 */
	int GetTimesEarned();
	/*
	 * The current amount of this achievement the user has earned, but not yet claimed. -1 if achievement is not accessed from UserData.GetAchievementsList().
	 */
	int GetUnclaimedCount();
	/*
	 * The number of actions required to earn a new achievement. -1 if achievement can't be earned in the current session.
	 */
	int GetDistance();
}
