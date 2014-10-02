using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;



public class FacebookUIManager : MonoBehaviourGUI
{
#if UNITY_ANDROID
	public static string screenshotFilename = "someScreenshot.png";


	// common event handler used for all Facebook graph requests that logs the data to the console
	void completionHandler( string error, object result )
	{
		if( error != null )
			Debug.LogError( error );
		else
			Prime31.Utils.logObject( result );
	}


	void Start()
	{
		// grab a screenshot for later use
		Application.CaptureScreenshot( screenshotFilename );

		// optionally enable logging of all requests that go through the Facebook class
		//Facebook.instance.debugRequests = true;
	}


	void OnGUI()
	{
		beginColumn();


		if( GUILayout.Button( "Initialize Facebook" ) )
		{
			FacebookAndroid.init();
		}


		if( GUILayout.Button( "Set Login Behavior to SUPPRESS_SSO" ) )
		{
			FacebookAndroid.setSessionLoginBehavior( FacebookSessionLoginBehavior.SUPPRESS_SSO );
		}


		if( GUILayout.Button( "Login" ) )
		{
			FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		}


		if( GUILayout.Button( "Reauthorize with Publish Permissions" ) )
		{
			FacebookAndroid.reauthorizeWithPublishPermissions( new string[] { "publish_actions", "manage_friendlists" }, FacebookSessionDefaultAudience.Everyone );
		}


		if( GUILayout.Button( "Logout" ) )
		{
			FacebookAndroid.logout();
		}


		if( GUILayout.Button( "Is Session Valid?" ) )
		{
			var isSessionValid = FacebookAndroid.isSessionValid();
			Debug.Log( "Is session valid?: " + isSessionValid );
		}


		if( GUILayout.Button( "Get Session Token" ) )
		{
			var token = FacebookAndroid.getAccessToken();
			Debug.Log( "session token: " + token );
		}


		if( GUILayout.Button( "Get Granted Permissions" ) )
		{
			var permissions = FacebookAndroid.getSessionPermissions();
			Debug.Log( "granted permissions: " + permissions.Count );
			Prime31.Utils.logObject( permissions );
		}


		endColumn( true );


		if( GUILayout.Button( "Post Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
			var bytes = System.IO.File.ReadAllBytes( pathToImage );

			Facebook.instance.postImage( bytes, "im an image posted from Android", completionHandler );
		}


		if( GUILayout.Button( "Graph Request (me)" ) )
		{
			Facebook.instance.graphRequest( "me", completionHandler );
		}


		if( GUILayout.Button( "Post Message" ) )
		{
			Facebook.instance.postMessage( "im posting this from Unity: " + Time.deltaTime, completionHandler );
		}


		if( GUILayout.Button( "Post Message & Extras" ) )
		{
			Facebook.instance.postMessageWithLinkAndLinkToImage( "link post from Unity: " + Time.deltaTime, "http://prime31.com", "prime[31]", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo", completionHandler );
		}


		if( GUILayout.Button( "Show Share Dialog" ) )
		{
			var parameters = new Dictionary<string,object>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" },
				{ "description", "description of what this share dialog is all about" }
			};
			FacebookAndroid.showFacebookShareDialog( parameters );
		}


		if( GUILayout.Button( "Show Post Dialog" ) )
		{
			// parameters are optional. See Facebook's documentation for all the dialogs and paramters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" }
			};
			FacebookAndroid.showDialog( "stream.publish", parameters );
		}


		if( GUILayout.Button( "Show Apprequests Dialog" ) )
		{
			// See Facebook's documentation for all the dialogs and paramters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "message", "Come play my awesome game!" }
			};
			FacebookAndroid.showDialog( "apprequests", parameters );
		}


		if( GUILayout.Button( "Get Friends" ) )
		{
			Facebook.instance.getFriends( completionHandler );
		}


		if( GUILayout.Button( "Log App Event" ) )
		{
			var parameters = new Dictionary<string,object>
			{
				{ "someKey", 55 },
				{ "anotherKey", "string value" }
			};
			FacebookAndroid.logEvent( "fb_mobile_add_to_cart", parameters );
		}


		endColumn();


		if( bottomLeftButton( "Twitter Scene" ) )
		{
			Application.LoadLevel( "TwitterTestScene" );
		}
	}

#endif
}
