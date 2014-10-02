using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class TwitterUIManager : MonoBehaviourGUI
{
#if UNITY_ANDROID
	void OnGUI()
	{
		beginColumn();


		if( GUILayout.Button( "Initialize Twitter" ) )
		{
			// Replace these with your own CONSUMER_KEY and CONSUMER_SECRET!
			TwitterAndroid.init( "jZVHZaGxJkOLenVPe23fnQ", "7nZQtvTjIXnKqYHbjAUKneUTp1QEWEkeD6nKVfPw" );
		}


		if( GUILayout.Button( "Login" ) )
		{
			TwitterAndroid.showLoginDialog();
		}


		if( GUILayout.Button( "Is Logged In?" ) )
		{
			var isLoggedIn = TwitterAndroid.isLoggedIn();
			Debug.Log( "Is logged in?: " + isLoggedIn );
		}


		if( GUILayout.Button( "Post Update with Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + FacebookUIManager.screenshotFilename;
			var bytes = System.IO.File.ReadAllBytes( pathToImage );

			TwitterAndroid.postStatusUpdate( "test update from Unity!", bytes );
		}


		endColumn( true );


		if( GUILayout.Button( "Logout" ) )
		{
			TwitterAndroid.logout();
		}


		if( GUILayout.Button( "Post Update" ) )
		{
			TwitterAndroid.postStatusUpdate( "im an update from the Twitter Android Plugin" );
		}


		if( GUILayout.Button( "Get Home Timeline" ) )
		{
			TwitterAndroid.getHomeTimeline();
		}


		if( GUILayout.Button( "Get Followers" ) )
		{
			TwitterAndroid.getFollowers();
		}


		if( GUILayout.Button( "Custom Request" ) )
		{
			var dict = new Dictionary<string,string>();
			dict.Add( "count", "2" );
			TwitterAndroid.performRequest( "GET", "1.1/statuses/home_timeline.json", dict );
		}

		endColumn();




		if( bottomLeftButton( "Facebook Scene" ) )
		{
			Application.LoadLevel( "FacebookTestScene" );
		}

	}
#endif
}
