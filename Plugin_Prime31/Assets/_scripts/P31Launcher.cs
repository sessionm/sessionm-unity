
using UnityEngine;
using System.Collections;
//using Prime31;

public class P31Launcher : MonoBehaviour 
{

//	public UILabel callbackLabel;
//	public UILabel iapCallbackLabel;
//
//	private bool billingEnabled = false;
//
//	private void Start()
//	{
//		#if UNITY_ANDROID
//		PlayGameServices.authenticate();
//		GPGManager.authenticationSucceededEvent += OnAuthSuccess;
//		GPGManager.authenticationFailedEvent += OnAuthFail;
//		GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgEClRbLnaBooFIRQ+6xZOJt14TvIixJSG4wuv3PLLs6xjGHK2kjvmEeJ3pavtYQw9NRbpv0gowSfP9qd3JRrJa0vIsdcXDLx6xceA6SqmfqVqrbzDl7kiD5byTH6b2gJ7cpfnYblDVWQT4jRHzVF8kDxHxmFMYrpcUvGmx3eFjFP2guaoTdgTjZT9GUag9N0B++iNohcSWklpJecTzFh8HEzOyxB7/y88C5EbyeaYNTZklze6fsjNKN7Yi+Xichv/i2yWw8msiz4CsvXbb6Me0qNwOF+jbsQuDOQcrPAHjy9Oqim7T8VSQ+ISEssPCiofytM6kLeZN7FtM55y6NLCQIDAQAB");
//		GoogleIABManager.billingSupportedEvent += OnBillingSuccess;
//		GoogleIABManager.billingNotSupportedEvent += OnBillingFailed;
//		#endif
//	}
//
//	public void OnAuthSuccess(string message)
//	{
//		callbackLabel.text = "GPlay - Authentication Success: " + message;
//	}
//
//	public void OnAuthFail(string message)
//	{
//		callbackLabel.text = "GPlay - Authentication Failed: " + message;
//	}
//
//	public void OnBillingSuccess()
//	{
//		iapCallbackLabel.text = "IAB Initailized Success";
//		billingEnabled = true;
//	}
//
//	public void OnBillingFailed(string message)
//	{
//		iapCallbackLabel.text = "IAB Initailized Failed: " + message;
//		billingEnabled = false;
//	}
//
//	public void ShowPrompt()
//	{
//		Debug.Log ("Show Alert");
//
//		#if UNITY_IOS
//		EtceteraBinding.showAlertWithTitleMessageAndButton("TEST WINDOW", "Test window success.", "OK");
//		#endif
//
//		#if UNITY_ANDROID
//		EtceteraAndroid.showAlert("TEST WINDOW", "Test window success.", "OK", "Cancel");
//		#endif
//	}
//
//	public void ShowWebPage()
//	{
//		Debug.Log ("Show WWW");
//		#if UNITY_IOS
//		EtceteraBinding.showWebPage("http://smorpheus.com/", false);
//		#endif
//
//		#if UNITY_ANDROID
//		EtceteraAndroid.showWebView("http://smorpheus.com/");
//		#endif
//	}
//
//	public void ShowMailComposer()
//	{
//		Debug.Log ("Show Mail Composer");
//
//		#if UNITY_IOS
//		EtceteraBinding.showMailComposer("thom@smorpheus.com", "Test Email", "This is a test email to demonstrate Prime31 is working.", false);
//		#endif
//
//		#if UNITY_ANDROID
//		EtceteraAndroid.showEmailComposer("thom@smorpheus.com", "Test Email", "This is a test email to demonstrate Prime31 is working.", false);
//		#endif
//	}
//
//	public void RegisterAcheivement1()
//	{
//		Debug.Log("Registering Achievement #1");
//
//		#if UNITY_IOS
//
//		#endif
//
//		#if UNITY_ANDROID
//		if(PlayGameServices.isSignedIn())
//			PlayGameServices.unlockAchievement("CgkIzai72q4FEAIQAA");
//		#endif
//	}
//
//	public void RegisterAcheivement2()
//	{
//		Debug.Log("Registering Achievement #2");
//
//		#if UNITY_IOS
//
//		#endif
//
//		#if UNITY_ANDROID
//		if(PlayGameServices.isSignedIn())
//			PlayGameServices.unlockAchievement("CgkIzai72q4FEAIQAQ");
//		#endif
//	}
//
//	public void RegisterAcheivement3()
//	{
//		Debug.Log("Registering Achievement #3");
//
//		#if UNITY_IOS
//
//		#endif
//
//		#if UNITY_ANDROID
//		if(PlayGameServices.isSignedIn())
//			PlayGameServices.incrementAchievement("CgkIzai72q4FEAIQAg", 1);
//		#endif
//	}
//
//	public void GenerateLeaderboardEntry()
//	{
//		int randomScore = Random.Range(0, 1000000);
//
//		Debug.Log("Registering Leaderboard Entry: " + randomScore);
//
//		#if UNITY_IOS
//
//		#endif
//
//		#if UNITY_ANDROID
//		if(PlayGameServices.isSignedIn())
//			PlayGameServices.submitScore("CgkIzai72q4FEAIQAw", randomScore);
//		#endif
//	}
//
//	public void PurchaseIAP1()
//	{
//		Debug.Log("Purchasing IAP #1");
//
//		#if UNITY_IOS
//
//		#endif
//
//		#if UNITY_ANDROID
//		if(billingEnabled)
//			GoogleIAB.purchaseProduct("100gizmos");
//		#endif
//	}
//
//	public void PurchaseIAP2()
//	{
//		Debug.Log("Purchasing IAP #2");
//
//		#if UNITY_IOS
//
//		#endif
//
//		#if UNITY_ANDROID
//		if(billingEnabled)
//			GoogleIAB.purchaseProduct("300fatgizmos");
//		#endif
//	}
//
//	
//	public void ShowAchievements()
//	{
//		Debug.Log("Showing Achievements");
//		
//		#if UNITY_IOS
//		
//		#endif
//		
//		#if UNITY_ANDROID
//		if(PlayGameServices.isSignedIn())
//			PlayGameServices.showAchievements();
//		#endif
//	}
//
//	public void ShowLeaderboards()
//	{
//		Debug.Log("Showing Leaderboards");
//		
//		#if UNITY_IOS
//		
//		#endif
//		
//		#if UNITY_ANDROID
//		if(PlayGameServices.isSignedIn())
//			PlayGameServices.showLeaderboards();
//		#endif
//	}

}
