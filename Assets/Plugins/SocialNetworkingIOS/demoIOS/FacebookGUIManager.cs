using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Prime31;


public class FacebookGUIManager : MonoBehaviourGUI
{
	public GameObject cube;

#if UNITY_IPHONE
	private string _userId;
	private bool _canUserUseFacebookComposer = false;
	private bool _hasPublishPermission = false;
	private bool _hasPublishActions = false;

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

		// when the session opens or a reauth occurs we check the permissions to see if we can publish
		FacebookManager.sessionOpenedEvent += () =>
		{
			_hasPublishPermission = FacebookBinding.getSessionPermissions().Contains( "publish_stream" );
			_hasPublishActions = FacebookBinding.getSessionPermissions().Contains( "publish_actions" );
		};

		FacebookManager.reauthorizationSucceededEvent += () =>
		{
			_hasPublishPermission = FacebookBinding.getSessionPermissions().Contains( "publish_stream" );
			_hasPublishActions = FacebookBinding.getSessionPermissions().Contains( "publish_actions" );
		};

		// grab a screenshot for later use
		Application.CaptureScreenshot( screenshotFilename );

		// this is iOS 6 only!
		_canUserUseFacebookComposer = FacebookBinding.canUserUseFacebookComposer();

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
			FacebookBinding.init();
		}


		if( GUILayout.Button( "Login" ) )
		{
			// Note: requesting publish permissions here will result in a crash. Only read permissions are permitted.
			var permissions = new string[] { "email" };
			FacebookBinding.loginWithReadPermissions( permissions );
		}


		if( GUILayout.Button( "Reauth with Publish Permissions" ) )
		{
			var permissions = new string[] { "publish_actions", "publish_stream" };
			FacebookBinding.reauthorizeWithPublishPermissions( permissions, FacebookSessionDefaultAudience.OnlyMe );
		}


		if( GUILayout.Button( "Enable Frictionless Requests" ) )
		{
			FacebookBinding.enableFrictionlessRequests();
		}


		if( GUILayout.Button( "Logout" ) )
		{
			FacebookBinding.logout();
		}


		if( GUILayout.Button( "Is Session Valid?" ) )
		{
			bool isLoggedIn = FacebookBinding.isSessionValid();
			Debug.Log( "Facebook is session valid: " + isLoggedIn );

			Facebook.instance.checkSessionValidityOnServer( isValid =>
			{
				Debug.Log( "checked session validity on server: " + isValid );
			});
		}


		if( GUILayout.Button( "Get Access Token" ) )
		{
			var token = FacebookBinding.getAccessToken();
			Debug.Log( "access token: " + token );
		}


		if( GUILayout.Button( "Get Granted Permissions" ) )
		{
			// This way of getting permissions uses the Facebook SDK. It is not always accurate since it does not hit the FB servers
			var permissions = FacebookBinding.getSessionPermissions();
			foreach( var perm in permissions )
				Debug.Log( perm );

			// This way of getting the permissions hits Facebook's servers so it is certain to be valid.
			Facebook.instance.getSessionPermissionsOnServer( completionHandler );
		}


		if( GUILayout.Button( "Log App Event" ) )
		{
			var parameters = new Dictionary<string,object>
			{
				{ "someKey", 55 },
				{ "anotherKey", "string value" }
			};
			FacebookBinding.logEvent( "fb_mobile_add_to_cart", parameters );
		}


		endColumn( true );


		// toggle to show two different sets of buttons
		if( toggleButtonState( "Show OG Buttons" ) )
			secondColumnButtonsGUI();
		else
			secondColumnAdditionalButtonsGUI();
		toggleButton( "Show OG Buttons", "Toggle Buttons" );

		endColumn( false );


		if( bottomRightButton( "Twitter..." ) )
		{
			Application.LoadLevel( "TwitterTestScene" );
		}
	}


	private void secondColumnButtonsGUI()
	{
		// only show posting actions if we have permission to do them
		if( _hasPublishPermission )
		{
			if( GUILayout.Button( "Post Image" ) )
			{
				var pathToImage = Application.persistentDataPath + "/" + FacebookGUIManager.screenshotFilename;
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
		}
		else
		{
			GUILayout.Label( "Reauthorize with publish_stream permissions to show posting buttons" );
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


		if( GUILayout.Button( "Show stream.publish Dialog" ) )
		{
			// parameters are optional. See Facebook's documentation for all the dialogs and parameters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" }
			};
			FacebookBinding.showDialog( "stream.publish", parameters );
		}


		if( GUILayout.Button( "Show apprequests Dialog" ) )
		{
			// see Facebook's documentation for all the dialogs and parameters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "title", "This Is The Title" },
				{ "message", "message goes here" }
			};
			FacebookBinding.showDialog( "apprequests", parameters );
		}


		if( GUILayout.Button( "Get Friends" ) )
		{
			Facebook.instance.getFriends( completionHandler );
		}


		if( _canUserUseFacebookComposer )
		{
			if( GUILayout.Button( "Show Facebook Composer" ) )
			{
				// ensure the image exists before attempting to add it!
				var pathToImage = Application.persistentDataPath + "/" + FacebookGUIManager.screenshotFilename;
				if( !System.IO.File.Exists( pathToImage ) )
					pathToImage = null;

				FacebookBinding.showFacebookComposer( "I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage, "http://prime31.com" );
			}
		}


		if( GUILayout.Button( "Show Facebook Share Dialog" ) )
		{
			var parameters = new Dictionary<string,object>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" },
				{ "description", "description of what this share dialog is all about" }
			};
			FacebookBinding.showFacebookShareDialog( parameters );
		}
	}


	private void secondColumnAdditionalButtonsGUI()
	{
		if( _hasPublishActions )
		{
			if( GUILayout.Button( "Post Score" ) )
			{
				// note that posting a score requires publish_actions permissions!
				var parameters = new Dictionary<string,object>()
				{ { "score", "5677" } };
				Facebook.instance.graphRequest( "me/scores", HTTPVerb.POST, parameters, completionHandler );
			}


			if( GUILayout.Button( "Post Achievement via Open Graph" ) )
			{
				// post an achievement. Note that posting achievements requires properly setting up your Open Graph accordingly:
				// https://developers.facebook.com/docs/concepts/opengraph/
				var parameters = new Dictionary<string,object>();
				parameters.Add( "achievement", "http://www.friendsmash.com/opengraph/achievement_50.html" );
				Facebook.instance.graphRequest( "me/achievements", HTTPVerb.POST, parameters, completionHandler );
			}


			if( GUILayout.Button( "Post Open Graph Action" ) )
			{
				// Note that you must properly setup your Open Graph actions and objects for it to work!
				// https://developers.facebook.com/docs/concepts/opengraph/

				var parameters = new Dictionary<string,object>()
				{ { "profile", "http://samples.ogp.me/390580850990722" } };
				Facebook.instance.graphRequest( "me/testiostestapp:smash", HTTPVerb.POST, parameters, completionHandler );
			}
		}
		else
		{
			GUILayout.Label( "Reauthorize with publish_actions permissions to show action buttons" );
		}


		if( GUILayout.Button( "Custom Graph Request: platform/posts" ) )
		{
			Facebook.instance.graphRequest( "platform/posts", HTTPVerb.GET, completionHandler );
		}


		if( GUILayout.Button( "Get Scores for me" ) )
		{
			Facebook.instance.getScores( "me", completionHandler );
		}


		if( _userId != null )
		{
			if( GUILayout.Button( "Show Profile Image" ) )
			{
				Facebook.instance.fetchProfileImageForUserId( _userId, ( tex ) =>
				{
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
