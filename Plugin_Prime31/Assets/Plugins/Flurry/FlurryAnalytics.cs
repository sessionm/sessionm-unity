using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;




#if UNITY_IPHONE
public class FlurryAnalytics
{
	[DllImport("__Internal")]
	private static extern void _flurryStartSession( string apiKey );

	// Starts up your Flurry analytics session.  Call on application startup.
	public static void startSession( string apiKey )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurryStartSession( apiKey );
	}


	[DllImport("__Internal")]
	private static extern void _flurryLogEvent( string eventName, bool isTimed );

	// Logs an event to Flurry.  If isTimed is true, this will be a timed event and it should be paired with a call to endTimedEvent
	public static void logEvent( string eventName, bool isTimed )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurryLogEvent( eventName, isTimed );
	}


	[DllImport("__Internal")]
	private static extern void _flurryLogEventWithParameters( string eventName, string parameters, bool isTimed );

	// Logs an event with optional key/value pairs
	public static void logEventWithParameters( string eventName, Dictionary<string,string> parameters, bool isTimed )
	{
		if( parameters == null )
			parameters = new Dictionary<string, string>();

		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurryLogEventWithParameters( eventName, parameters.toJson(), isTimed );
	}


	[DllImport("__Internal")]
	private static extern void _flurryEndTimedEvent( string eventName, string parameters );

	// Ends a timed event that was previously started
	public static void endTimedEvent( string eventName )
	{
		endTimedEvent( eventName, new Dictionary<string,string>() );
	}

	public static void endTimedEvent( string eventName, Dictionary<string,string> parameters )
	{
		if( parameters == null )
			parameters = new Dictionary<string, string>();

		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurryEndTimedEvent( eventName, parameters.toJson() );
	}


	[DllImport("__Internal")]
	private static extern void _flurrySetAge( int age );

	// Sets the users age
	public static void setAge( int age )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurrySetAge( age );
	}


	[DllImport("__Internal")]
	private static extern void _flurrySetGender( string gender );

	// Sets the users gender
	public static void setGender( string gender )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurrySetGender( gender );
	}


	[DllImport("__Internal")]
	private static extern void _flurrySetUserId( string userId );

	// Sets the users unique id
	public static void setUserId( string userId )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurrySetUserId( userId );
	}


	[DllImport("__Internal")]
	private static extern void _flurrySetSessionReportsOnCloseEnabled( bool sendSessionReportsOnClose );

	// Sets whether Flurry should upload session reports when your app closes
	public static void setSessionReportsOnCloseEnabled( bool sendSessionReportsOnClose )

	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurrySetSessionReportsOnCloseEnabled( sendSessionReportsOnClose );
	}


	[DllImport("__Internal")]
	private static extern void _flurrySetSessionReportsOnPauseEnabled( bool setSessionReportsOnPauseEnabled );

	// Sets whether Flurry should upload session reports when your app is paused
	public static void setSessionReportsOnPauseEnabled( bool setSessionReportsOnPauseEnabled )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_flurrySetSessionReportsOnPauseEnabled( setSessionReportsOnPauseEnabled );
	}
}
#endif