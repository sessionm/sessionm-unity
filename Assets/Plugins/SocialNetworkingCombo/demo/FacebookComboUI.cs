using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class FacebookComboUI : MonoBehaviourGUI
{
	public GameObject cube;

#if UNITY_IPHONE || UNITY_ANDROID
	private string _userId;

	public static string screenshotFilename = "someScreenshot.png";



	// common event handler used for all graph requests that logs the data to the console
	void completionHandler( string error, object result )
	{
		if( error != null )
			Debug.LogError( error );
		else
			Prime31.Utils.logObject( result );
	}


	void Start()
	{
		// dump custom data to log after a request completes
		FacebookManager.graphRequestCompletedEvent += result =>
		{
			Prime31.Utils.logObject( result );
		};

		// grab a screenshot for later use
		Application.CaptureScreenshot( screenshotFilename );

		// optionally enable logging of all requests that go through the Facebook class
		//Facebook.instance.debugRequests = true;
	}


	void OnGUI()
	{
		// center labels
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		beginColumn();

		if( GUILayout.Button( "Initialize Facebook" ) )
		{
			FacebookCombo.init();
		}


		if( GUILayout.Button( "Login" ) )
		{
			// Note: requesting publish permissions here will result in a crash. Only read permissions are permitted.
			var permissions = new string[] { "email" };
			FacebookCombo.loginWithReadPermissions( permissions );
		}


		if( GUILayout.Button( "Reauth with Publish Permissions" ) )
		{
			var permissions = new string[] { "publish_actions", "publish_stream" };
			FacebookCombo.reauthorizeWithPublishPermissions( permissions, FacebookSessionDefaultAudience.OnlyMe );
		}


		if( GUILayout.Button( "Logout" ) )
		{
			FacebookCombo.logout();
		}


		if( GUILayout.Button( "Is Session Valid?" ) )
		{
			// isSessionValid only checks the local session so if a user deauthorizies the application on Facebook's website it can be incorrect
			var isLoggedIn = FacebookCombo.isSessionValid();
			Debug.Log( "Facebook is session valid: " + isLoggedIn );

			// This way of checking a session hits Facebook's servers ensuring the session really is valid
			Facebook.instance.checkSessionValidityOnServer( isValid =>
			{
				Debug.Log( "checked session validity on server. Is it valid? " + isValid );
			});
		}


		if( GUILayout.Button( "Get Access Token" ) )
		{
			var token = FacebookCombo.getAccessToken();
			Debug.Log( "access token: " + token );
		}


		if( GUILayout.Button( "Get Granted Permissions" ) )
		{
			// This way of getting session permissions uses Facebook's SDK which is a local cache. It can be wrong for various reasons.
			var permissions = FacebookCombo.getSessionPermissions();
			foreach( var perm in permissions )
				Debug.Log( perm );

			// This way of getting the permissions hits Facebook's servers so it is certain to be valid.
			Facebook.instance.getSessionPermissionsOnServer( completionHandler );
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
			FacebookCombo.showFacebookShareDialog( parameters );
		}


		endColumn( true );

		secondColumnButtonsGUI();

		endColumn( false );


		if( bottomRightButton( "Twitter..." ) )
		{
			Application.LoadLevel( "TwitterCombo" );
		}
	}


	private void secondColumnButtonsGUI()
	{
		if( GUILayout.Button( "Post Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + FacebookComboUI.screenshotFilename;
			if( !System.IO.File.Exists( pathToImage ) )
			{
				Debug.LogError( "there is no screenshot avaialable at path: " + pathToImage );
				return;
			}

			var bytes = System.IO.File.ReadAllBytes( pathToImage );
			Facebook.instance.postImage( bytes, "im an image posted from iOS", completionHandler );
		}


		if( GUILayout.Button( "Post Message" ) )
		{
			Facebook.instance.postMessage( "im posting this from Unity: " + Time.deltaTime, completionHandler );
		}


		if( GUILayout.Button( "Post Message & Extras" ) )
		{
			Facebook.instance.postMessageWithLinkAndLinkToImage( "link post from Unity: " + Time.deltaTime, "http://prime31.com", "Prime31 Studios", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo", completionHandler );
		}


		if( GUILayout.Button( "Post Score" ) )
		{
			// note that posting a score requires publish_actions permissions!
			Facebook.instance.postScore( 5688, ( didPost ) => { Debug.Log( "score did post: " + didPost ); } );
		}


		if( GUILayout.Button( "Show stream.publish Dialog" ) )
		{
			// parameters are optional. See Facebook's documentation for all the dialogs and paramters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" }
			};
			FacebookCombo.showDialog( "stream.publish", parameters );
		}


		if( GUILayout.Button( "Get Friends" ) )
		{
			Facebook.instance.getFriends( ( error, friends ) =>
			{
				if( error != null )
				{
					Debug.LogError( "error fetching friends: " + error );
					return;
				}

				Debug.Log( friends );
			});
		}


		if( GUILayout.Button( "Graph Request (me)" ) )
		{
			Facebook.instance.getMe( ( error, result ) =>
			{
				// if we have an error we dont proceed any further
				if( error != null )
					return;

				if( result == null )
					return;

				// grab the userId and persist it for later use
				_userId = result.id;

				Debug.Log( "me Graph Request finished: " );
				Debug.Log( result );
			});
		}


		if( _userId != null )
		{
			if( GUILayout.Button( "Show Profile Image" ) )
			{
				Facebook.instance.fetchProfileImageForUserId( _userId, ( tex ) =>
				{
					if( tex != null )
						cube.renderer.material.mainTexture = tex;
				});
			}
		}
		else
		{
			GUILayout.Label( "Call the me Graph request to show user specific buttons" );
		}
	}


#endif
}
