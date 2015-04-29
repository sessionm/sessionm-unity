using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievementToast : MonoBehaviour 
{
	private const float TOAST_ANIMATION_TIME = 1.0f;
	private const float TOAST_DISPLAY_TIME = 3.0f;

	public enum ToastState
	{
		Offscreen,
		Toasting,
		Displaying,
		Untoasting
	}

	public Transform offScreenPosition;
	public Transform onScreenPosition;
	public Text titleText;
	public Text mPointsText;

	private float toastTimer;
	public ToastState toastState;

	//Exposed Methods
	public void ShowAchievementToast(string achievementTitle, int mPointValue)
	{
		//Exit this call if there is currently an active Toast 
		//(This shouldn't occur because we have notified SessionM we are currently displaying an achievement.)
		if(toastState != ToastState.Offscreen)
			return;

		titleText.text = achievementTitle;
		mPointsText.text = "mPoints: " + mPointValue.ToString();
		toastState = ToastState.Toasting;
		toastTimer = 0f;

		//Notify SessionM We are about to display the achievment.
		SessionM.GetInstance().NotifyPresented();
	}

	//Helper Methods

	private void LerpToast()
	{
		float positionPercentage = 1f;

		switch(toastState) {
		case ToastState.Toasting:
			toastTimer += Time.deltaTime;
			positionPercentage = toastTimer / TOAST_ANIMATION_TIME;

			if(positionPercentage > 1f) {
				positionPercentage = 1f;
				toastState = ToastState.Displaying;
				toastTimer = 0f;
			}

			this.transform.position = Vector3.Lerp(offScreenPosition.position, onScreenPosition.position, positionPercentage);
			break;
		case ToastState.Displaying:
			toastTimer += Time.deltaTime;

			if(toastTimer > TOAST_DISPLAY_TIME) {
				toastState = ToastState.Untoasting;
				toastTimer = 0f;
			}
			break;
		case ToastState.Untoasting:
			toastTimer += Time.deltaTime;
			positionPercentage = toastTimer / TOAST_ANIMATION_TIME;

			if(positionPercentage > 1f) {
				positionPercentage = 1f;
				toastState = ToastState.Offscreen;
				toastTimer = 0f;

				//Our Native Achievement has come and gone without the user clicking on it
				//We need to notify SessionM that the achievment was dismissed.
				SessionM.GetInstance().NotifyDismissed();
			}

			this.transform.position = Vector3.Lerp(onScreenPosition.position, offScreenPosition.position, positionPercentage);
			break;
		default:
			Debug.LogWarning("Invalid Toast State in LerpToast.");
			break;
		}
	}

	private void ResetToastPosition()
	{
		this.transform.position = offScreenPosition.transform.position;
		toastTimer = 0f;
		toastState = ToastState.Offscreen;
		titleText.text = "";
		mPointsText.text = "";
	}


	//Unity Lifecycle
	private void Awake()
	{
		ResetToastPosition();
	}

	private void Update()
	{
		if(toastState != ToastState.Offscreen) {
			LerpToast();
		}
	}

	private void OnMouseDown()
	{
		//The user has clicked on the toast, so we should let SessionM know they want to claim it.
		//SessionM will take care of the rest (opening the portal.)
		//However, we'll want to reset our Toast so the user doesn't click on it again.
		ResetToastPosition();
		SessionM.GetInstance().NotifyClaimed();
	}
}
