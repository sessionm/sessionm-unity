using UnityEngine;

/*
 * Achievement data interface. 
 * 
 * Defines information about achievement used to customize achievement alert presentation.
 */ 
public interface IAchievementData
{
	/*
	 * Achievement name.
	 */
	string GetName();
	/*
	 * Achievement message.
	 */ 
	string GetMessage();
	/*
	 * mPoint value.
	 */
	int GetMpointValue();
	/*
	 * Boolean indicating if achievement is custom. 
	 */
	bool IsCustom();
}
