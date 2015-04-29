using UnityEngine;
using UnityEngine.UI;

public class SessionMNotificationBadge : MonoBehaviour 
{
	public Image[] badgeImages;
	public Text badgeText;

	private bool badgeVisible = true;

	void Update ()
	{
		UpdateBadge();
	}

	private void UpdateBadge()
	{
		int badgeCount = SessionM.GetInstance().GetUnclaimedAchievementCount();

		if(badgeVisible == false && badgeCount > 0) {
			ShowBadge();
		}

		if(badgeVisible == true && badgeCount == 0) {
			HideBadge();
		}

		if(badgeCount > 99) {
			badgeCount = 99;
		}

		badgeText.text = badgeCount.ToString();
	}

	private void ShowBadge()
	{
		badgeVisible = true;

		foreach(Image badgeImage in badgeImages) {
			badgeImage.enabled = true;
		}
		badgeText.enabled = true;
	}

	private void HideBadge()
	{
		badgeVisible = false;

		foreach(Image badgeImage in badgeImages) {
			badgeImage.enabled = false;
		}
		badgeText.enabled = false;
	}
}