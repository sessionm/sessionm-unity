using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SessionMSample : MonoBehaviour 
{
	public SessionM sessionM;

	public Text sessionMStateLabel;

	public Text optOutLabel;
	public Text isRegisteredLabel;
	public Text isLoggedInLabel;
	public Text pointBalanceLabel;
	public Text unclaimedAchCountLabel;
	public Text unclaimedAchValueLable;

	public string action1;
	public string action2;
	public string action3;

	public AchievementToast toaster;

	//Exposed Methods

	public void OnAction1Clicked()
	{
		sessionM.LogAction(action1);
	}

	public void OnAction2Clicked()
	{
		sessionM.LogAction(action2);
	}

	public void OnAction3Clicked()
	{
		sessionM.LogAction(action3);
	}

	public void OnPortalClicked()
	{
		sessionM.ShowPortal();
	}

	//Helper Methods 

	private void NotifySessionError(int errorCode, string error)
	{
		Debug.LogError("SessionM Error Reported: " + errorCode.ToString() + " - " + error);
	}

	private void NotifySessionStateChanged(SessionState state)
	{
		Debug.Log("Event: NotifySessionStateChanged Fired");
		sessionMStateLabel.text = "SessionM State: " + state.ToString();
	}

	private void NotifyUnclaimedAchievementDataUpdated(IAchievementData achievementData)
	{
		Debug.Log("Recieved New Achievement: " + achievementData.GetName() + " - Worth: " + achievementData.GetMpointValue() + "\n With Message: " + achievementData.GetMessage());
		toaster.ShowAchievementToast(achievementData.GetName(), achievementData.GetMpointValue());
	}

	private void UserChanged(IDictionary<string, object> userInfo)
	{
		UserData user = SessionM.GetInstance().GetUserData();

		if(user == null)
			return;

		optOutLabel.text = "Opt Out: " + user.IsOptedOut().ToString();
		isRegisteredLabel.text = "Is Registered: " + user.IsRegistered();
		isLoggedInLabel.text = "Is Logged In: " + user.IsLoggedIn();
		pointBalanceLabel.text = "Point Balance: " + user.GetUserPointBalance();
		unclaimedAchCountLabel.text = "Unclaimed Achievement Count: " + user.GetUnclaimedAchievementCount();
		unclaimedAchValueLable.text = "Unclaimed Achievement Value: " + user.GetUnclaimedAchievementValue();
	}

	//Unity Lifecycle

	private void OnEnable()
	{
		//Assign useful events to Helper Functions in the class.
		SessionMEventListener.NotifySessionStateChanged += NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError += NotifySessionError;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated += NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserInfoChanged += UserChanged;

		UserChanged(null);
	}

	private void OnDisable()
	{
		//Clean Up the events in case this object is destroyed.
		SessionMEventListener.NotifySessionStateChanged -= NotifySessionStateChanged;
		SessionMEventListener.NotifySessionError -= NotifySessionError;
		SessionMEventListener.NotifyUnclaimedAchievementDataUpdated -= NotifyUnclaimedAchievementDataUpdated;
		SessionMEventListener.NotifyUserInfoChanged -= UserChanged;
	}

}
