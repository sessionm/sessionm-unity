using UnityEngine;
using System.Collections;

public class FlurryTest : MonoBehaviour 
{
	private const string FLURRY_AD_SPACE = "SessionMTest Ad";
	private const string FLURRY_IOS_SPACE = "iOSTestAd";

	private void Start()
	{
		#if UNITY_IOS
		FlurryAnalytics.startSession("922TMF4MK4JJ8584BMS7");

		FlurryAds.enableAds(true);
		FlurryAds.fetchAdForSpace(FLURRY_IOS_SPACE, FlurryAdSize.Bottom);
		FlurryAnalytics.setSessionReportsOnCloseEnabled(true);
		#endif

		#if UNITY_ANDROID
		FlurryAndroid.onStartSession("T2WT38V6P457MSD72X67", true, true);
		FlurryAndroid.fetchAdsForSpace(FLURRY_AD_SPACE, FlurryAdPlacement.BannerBottom);
		#endif
	}

	private void OnEnable()
	{
		#if UNITY_IOS
		FlurryManager.spaceDidReceiveAdEvent += OnSpaceDidRecieveAdEvent;
		FlurryManager.spaceDidFailToReceiveAdEvent += OnSpaceDidFailToRecieveAdEvent;
		#endif

		#if UNITY_ANDROID
		FlurryAndroidManager.adAvailableForSpaceEvent += OnAdAvailable;
		FlurryAndroidManager.adNotAvailableForSpaceEvent += OnAdNotAvailable;
		FlurryAndroidManager.spaceDidReceiveAdEvent += OnSpaceDidRecieveAdEvent;
		FlurryAndroidManager.spaceDidFailToReceiveAdEvent += OnSpaceDidFailToRecieveAdEvent;
		#endif
	}

	private void OnDisable()
	{
		#if UNITY_IOS
		FlurryManager.spaceDidReceiveAdEvent -= OnSpaceDidRecieveAdEvent;
		FlurryManager.spaceDidFailToReceiveAdEvent -= OnSpaceDidFailToRecieveAdEvent;
		#endif

		#if UNITY_ANDROID
		FlurryAndroidManager.adAvailableForSpaceEvent -= OnAdAvailable;
		FlurryAndroidManager.adNotAvailableForSpaceEvent -= OnAdNotAvailable;
		FlurryAndroidManager.spaceDidReceiveAdEvent -= OnSpaceDidRecieveAdEvent;
		FlurryAndroidManager.spaceDidFailToReceiveAdEvent -= OnSpaceDidFailToRecieveAdEvent;
		#endif
	}

	private void OnApplicationQuit()
	{

		#if UNITY_ANDROID
		FlurryAndroid.onEndSession();
		#endif
	}

	public void LogEvent1()
	{
		Debug.Log("Log Event 1");

		#if UNITY_IOS
		FlurryAnalytics.logEvent("Event1", false);
		#endif

		#if UNITY_ANDROID
		FlurryAndroid.logEvent("Event1");
		#endif
	}

	public void LogEvent2()
	{
		Debug.Log("Log Event 2");

		#if UNITY_IOS
		FlurryAnalytics.logEvent("Event2", false);
		#endif

		#if UNITY_ANDROID
		FlurryAndroid.logEvent("Log Event 2");
		#endif
	}

	public void LogEvent3()
	{
		Debug.Log("Log Event 3");

		#if UNITY_IOS
		FlurryAnalytics.logEvent("Event3", false);
		#endif

		#if UNITY_ANDROID
		FlurryAndroid.logEvent("Log Event 3");
		#endif
	}

	public void ShowAds()
	{
		Debug.Log("Show Ads");

		#if UNITY_IOS
		FlurryAds.fetchAndDisplayAdForSpace(FLURRY_IOS_SPACE, FlurryAdSize.Bottom);
		#endif

		#if UNITY_ANDROID
		FlurryAndroid.displayAd(FLURRY_AD_SPACE, FlurryAdPlacement.BannerBottom, 10);
		#endif
	}

	public void HideAds()
	{
		Debug.Log("Hide Ads");

		#if UNITY_IOS
		FlurryAds.removeAdFromSpace(FLURRY_IOS_SPACE);
		#endif

		#if UNITY_ANDROID
		FlurryAndroid.removeAd("SessionMTest Ad");
		#endif
	}

	public UILabel adsAvailable;
	public UILabel spaceEvent;

	public void OnAdAvailable(string message)
	{
		adsAvailable.text = "Ads Available: " + message;
	}

	public void OnAdNotAvailable(string message)
	{
		adsAvailable.text = "Ads Not Available: " + message;
	}

	public void OnSpaceDidFailToRecieveAdEvent(string message)
	{
		spaceEvent.text = "Space Failed to Recieve Ad Event: " + message;
	}

	public void OnSpaceDidRecieveAdEvent(string message)
	{
		spaceEvent.text = "Space Recieved Ad Event: " + message;
	}
}
