using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class TwitterComboUI : MonoBehaviourGUI
{
#if UNITY_IPHONE || UNITY_ANDROID
	void Start()
	{
		Application.CaptureScreenshot( FacebookComboUI.screenshotFilename );
	}


	// common event handler used for all Twitter API requests that logs the data to the console
	void completionHandler( string error, object result )
	{
		if( error != null )
			Debug.LogError( error );
		else
			Prime31.Utils.logObject( result );
	}


	void OnGUI()
	{
		beginColumn();

		if( GUILayout.Button( "Initialize Twitter" ) )
		{
			// Replace these with your own CONSUMER_KEY and CONSUMER_SECRET!
			TwitterCombo.init( "I1hxdhKOrQm6IsR0szOxQ", "lZDRqdzWJq3cATgfXMDjk0kaYajsP9450wKXYXAnpw" );
		}


		if( GUILayout.Button( "Login with Oauth" ) )
		{
			TwitterCombo.showLoginDialog();
		}


		if( GUILayout.Button( "Logout" ) )
		{
			TwitterCombo.logout();
		}


		if( GUILayout.Button( "Is Logged In?" ) )
		{
			bool isLoggedIn = TwitterCombo.isLoggedIn();
			Debug.Log( "Twitter is logged in: " + isLoggedIn );
		}


		if( GUILayout.Button( "Logged in Username" ) )
		{
			string username = TwitterCombo.loggedInUsername();
			Debug.Log( "Twitter username: " + username );
		}


		endColumn( true );


		if( GUILayout.Button( "Post Status Update" ) )
		{
			TwitterCombo.postStatusUpdate( "im posting this from Unity: " + Time.deltaTime );
		}


		if( GUILayout.Button( "Post Status Update + Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + FacebookComboUI.screenshotFilename;
			TwitterCombo.postStatusUpdate( "I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage );
		}


		if( GUILayout.Button( "Custom Request" ) )
		{
			var dict = new Dictionary<string,string>();
			dict.Add( "count", "2" );
			TwitterCombo.performRequest( "GET", "1.1/statuses/home_timeline.json", dict );
		}


		endColumn();
	}
#endif
}
