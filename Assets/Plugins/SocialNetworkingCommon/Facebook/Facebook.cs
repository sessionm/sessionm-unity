using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System.Reflection;


public class Facebook : P31RestKit
{
	public string accessToken;
	public string appAccessToken;
	public bool useSessionBabysitter = true;


	private static Facebook _instance = null;
	public static Facebook instance
	{
		get
		{
			if( _instance == null )
				_instance = new Facebook();

			return _instance;
		}
	}


	public Facebook()
	{
		_baseUrl = "https://graph.facebook.com/";
		forceJsonResponse = true;
	}


	#region Private

	protected override IEnumerator send( string path, HTTPVerb httpVerb, Dictionary<string,object> parameters, Action<string, object> onComplete )
	{
		if( parameters == null )
			parameters = new Dictionary<string, object>();

		// add the access token if we dont have one in the dictionary
		if( !parameters.ContainsKey( "access_token" ) )
			parameters.Add( "access_token", accessToken );

		if( httpVerb == HTTPVerb.PUT || httpVerb == HTTPVerb.DELETE )
			parameters.Add( "method", httpVerb.ToString() );

		return base.send( path, httpVerb, parameters, onComplete );
	}


	protected bool shouldSendRequest()
	{
		// if the session babysitter is not enabled always return true
		if( !useSessionBabysitter )
			return true;

		// if we dont have the babysitter available then always send the request. This also protects against Windows Store/Phone Type class issues
		try
		{
#if UNITY_IPHONE
			var type = typeof( Facebook ).Assembly.GetType( "FacebookBinding" );
#elif UNITY_ANDROID
			var type = typeof( Facebook ).Assembly.GetType( "FacebookAndroid" );
#endif

#if UNITY_IPHONE || UNITY_ANDROID
			// this will pass for iOS and Android when the SocialNetworking Plugin is present
			if( type != null )
			{
		    	var method = type.GetMethod( "isSessionValid", BindingFlags.Static | BindingFlags.Public );
		    	var result = method.Invoke( null, null );
				return (bool)result;
			}
#endif
		}
		catch( Exception )
		{}

		return true;
	}


	#endregion


	public void prepareForMetroUse( GameObject go, MonoBehaviour mb )
	{
		GameObject.DontDestroyOnLoad( go );
		surrogateGameObject = go;
		surrogateMonobehaviour = mb;
	}


	#region Public

	// Sends off a graph request. The completion handler will return a Dictionary<string,object> or List<object> if successful depending on the path called.
	// See Facebook's documentation for the returned data and parameters
	public void graphRequest( string path, Action<string, object> completionHandler )
	{
		graphRequest( path, HTTPVerb.GET, completionHandler );
	}


	public void graphRequest( string path, HTTPVerb verb, Action<string, object> completionHandler )
	{
		graphRequest( path, verb, null, completionHandler );
	}


	public void graphRequest( string path, HTTPVerb verb, Dictionary<string, object> parameters, Action<string, object> completionHandler )
	{
		// in the editor, we allow requests to be sent as long as we have an access token
#if UNITY_EDITOR
		if( ( accessToken != null && accessToken.Length > 0 ) || shouldSendRequest() )
#else
		if( shouldSendRequest() )
#endif
		{
			surrogateMonobehaviour.StartCoroutine( send( path, verb, parameters, completionHandler ) );
		}
		else
		{
			// if we have an auth helper we will use it because we are not logged in
			// auth helpers are classes in the FacebookBinding and FacebookAndroid files
			try
			{
#if UNITY_IPHONE
				var type = typeof( Facebook ).Assembly.GetType( "FacebookBinding" );
#elif UNITY_ANDROID
				var type = typeof( Facebook ).Assembly.GetType( "FacebookAndroid" );
#endif

#if UNITY_IPHONE || UNITY_ANDROID
				// this will pass for iOS and Android when the SocialNetworking Plugin is present
				if( type != null )
				{
			    	var method = type.GetMethod( "babysitRequest", BindingFlags.Static | BindingFlags.NonPublic );
			    	if( method != null )
					{
						Action action = () => { surrogateMonobehaviour.StartCoroutine( send( path, verb, parameters, completionHandler ) ); };
						method.Invoke( null, new object[] { verb == HTTPVerb.POST, action } );
					}
				}
#endif
			}
			catch( Exception )
			{}
		}
	}


	public void graphRequestBatch( IEnumerable<FacebookBatchRequest> requests, Action<string, object> completionHandler )
	{
		var parameters = new Dictionary<string,object>();
		var requestList = new List<Dictionary<string,object>>();

		foreach( var r in requests )
			requestList.Add( r.requestDictionary() );

		parameters.Add( "batch", Json.encode( requestList ) );

		surrogateMonobehaviour.StartCoroutine( send( string.Empty, HTTPVerb.POST, parameters, completionHandler ) );
	}


	// Fetches a profile image for the user with userId. completionHandler will fire with the Texture2D or null
	public void fetchProfileImageForUserId( string userId, Action<Texture2D> completionHandler )
	{
		var url = "http://graph.facebook.com/" + userId + "/picture?type=large";
		surrogateMonobehaviour.StartCoroutine( fetchImageAtUrl( url, completionHandler ) );
	}


	// Fetches an image for the url. completionHandler will fire with the Texture2D or null
	public IEnumerator fetchImageAtUrl( string url, Action<Texture2D> completionHandler )
	{
		var www = new WWW( url );

		yield return www;

		if( !string.IsNullOrEmpty( www.error ) )
		{
			Debug.Log( "Error attempting to load profile image: " + www.error );
			if( completionHandler != null )
				completionHandler( null );
		}
		else
		{
			if( completionHandler != null )
				completionHandler( www.texture );
		}
	}

	#endregion


	#region Graph API Examples

	// Posts the message to the user's wall
    public void postMessage( string message, Action<string, object> completionHandler )
    {
		var parameters = new Dictionary<string,object>
		{
			{ "message", message }
		};
		graphRequest( "me/feed", HTTPVerb.POST, parameters, completionHandler );
    }


	// Posts the message to the user's wall with a link and a name for the link
    public void postMessageWithLink( string message, string link, string linkName, Action<string, object> completionHandler )
    {
		var parameters = new Dictionary<string,object>
		{
			{ "message", message },
			{ "link", link },
			{ "name", linkName }
		};
		graphRequest( "me/feed", HTTPVerb.POST, parameters, completionHandler );
    }


	// Posts the message to the user's wall with a link, a name for the link, a link to an image and a caption for the image
    public void postMessageWithLinkAndLinkToImage( string message, string link, string linkName, string linkToImage, string caption, Action<string, object> completionHandler )
    {
		var parameters = new Dictionary<string,object>
		{
			{ "message", message },
			{ "link", link },
			{ "name", linkName },
			{ "picture", linkToImage },
			{ "caption", caption }
		};
		graphRequest( "me/feed", HTTPVerb.POST, parameters, completionHandler );
    }


	// Posts an image on the user's wall along with a caption.
    public void postImage( byte[] image, string message, Action<string, object> completionHandler )
    {
		var parameters = new Dictionary<string,object>()
		{
			{ "picture", image },
			{ "message", message  }
		};
		graphRequest( "me/photos", HTTPVerb.POST, parameters, completionHandler );
    }


	// Posts an image to a specific album along with a caption.
    public void postImageToAlbum( byte[] image, string caption, string albumId, Action<string, object> completionHandler )
    {
		var parameters = new Dictionary<string,object>()
		{
			{ "picture", image },
			{ "message", caption  }
		};
		graphRequest( albumId, HTTPVerb.POST, parameters, completionHandler );
    }


	// Sends a request to fetch the currently logged in users details
    public void getMe( Action<string,FacebookMeResult> completionHandler )
    {
		graphRequest( "me", ( error, obj ) =>
		{
			if( completionHandler == null )
				return;

			if( error != null )
				completionHandler( error, null );
			else
				completionHandler( null, Json.decodeObject<FacebookMeResult>( obj ) );
		});
    }


	// Sends a request to fetch the currently logged in users friends
    public void getFriends( Action<string,FacebookFriendsResult> completionHandler )
    {
		graphRequest( "me/friends", ( error, obj ) =>
		{
			if( completionHandler == null )
				return;

			if( error != null )
				completionHandler( error, null );
			else
				completionHandler( null, Json.decodeObject<FacebookFriendsResult>( obj ) );
		});
    }


	// Extends a short lived access token. Completion handler returns either the expiry date or null if unsuccessful. Note that it is highly recommended to
	// only call this from a server. Your app secret should not be included with your application
	public void extendAccessToken( string appId, string appSecret, Action<DateTime?> completionHandler )
	{
		if( Facebook.instance.accessToken == null )
		{
			Debug.LogError( "There is no access token to extend. The user must be autenticated before attempting to extend their access token" );
			return;
		}

		var parameters = new Dictionary<string,object>()
		{
			{ "client_id", appId },
			{ "client_secret", appSecret },
			{ "grant_type", "fb_exchange_token" },
			{ "fb_exchange_token", Facebook.instance.accessToken }
		};

		get( "oauth/access_token", parameters, ( error, obj ) =>
		{
			if( obj is string )
			{
				var text = obj as string;
				if( text.StartsWith( "access_token=" ) )
				{
					var paramDict = text.parseQueryString();
					Facebook.instance.accessToken = paramDict["access_token"];

					var expires = double.Parse( paramDict["expires"] );
					completionHandler( DateTime.Now.AddSeconds( expires ) );
				}
				else
				{
					Debug.LogError( "error extending access token: " + text );
					completionHandler( null );
				}
			}
			else
			{
				Debug.LogError( "error extending access token: " + error );
				completionHandler( null );
			}
		});
	}


	// Checks the validity of a session on Facebook's servers. This is the authoritative way to check a session's validity.
	public void checkSessionValidityOnServer( Action<bool> completionHandler )
	{
		/*
		// first see if we have an access token
		if( ( accessToken == null || ( accessToken != null && accessToken.Length == 0 ) ) || !shouldSendRequest() )
		{
			Debug.Log( "no access token locally so aborting request" );
			completionHandler( false );
			return;
		}
		*/

		get( "me", ( error, obj ) =>
		{
			if( error == null && obj != null && obj is IDictionary )
				completionHandler( true );
			else
				completionHandler( false );
		});
	}


	// Fetches the session permissions directly from Facebook's servers. This is the authoritative way to get a session's granted permissions.
	public void getSessionPermissionsOnServer( Action<string, List<string>> completionHandler )
	{
		get( "me/permissions", ( error, obj ) =>
		{
			if( error == null && obj != null && obj is IDictionary )
			{
				var resultDict = obj as IDictionary;
				var dataDict = resultDict["data"] as IList;
				var rawPermissionsList = dataDict[0] as IDictionary;

				var grantedPermissions = new List<string>();
				foreach( DictionaryEntry kv in rawPermissionsList )
				{
					if( kv.Value.ToString() == "1" )
						grantedPermissions.Add( kv.Key.ToString() );
				}
				completionHandler( null, grantedPermissions );
			}
			else
			{
				completionHandler( error, null );
			}
		});
	}

	#endregion


	#region App Access Token

	// Fetches the app access token. Note that it is highly recommended to only call this from a server. Your app secret should not be included
	// with your application
	public void getAppAccessToken( string appId, string appSecret, Action<string> completionHandler )
	{
		var parameters = new Dictionary<string,object>()
		{
			{ "client_id", appId },
			{ "client_secret", appSecret },
			{ "grant_type", "client_credentials" }
		};

		get( "oauth/access_token", parameters, ( error, obj ) =>
		{
			if( obj is string )
			{
				var text = obj as string;
				if( text.StartsWith( "access_token=" ) )
				{
					appAccessToken = text.Replace( "access_token=", string.Empty );
					completionHandler( appAccessToken );
				}
				else
				{
					completionHandler( null );
				}
			}
			else
			{
				completionHandler( null );
			}
		});
	}


	// Posts a score for the current user
	public void postScore( int score, Action<bool> completionHandler )
	{
		// post the score to the proper path
		var parameters = new Dictionary<string,object>()
		{
			{ "score", score.ToString() }
		};

		post( "me/scores", parameters, ( error, obj ) =>
		{
			if( error == null && obj is string )
			{
				completionHandler( ((string)obj).ToLower() == "true" );
			}
			else
			{
				completionHandler( false );
			}
		});
	}


	// Retrieves the scores for your app
	public void getScores( string userId, Action<string, object> onComplete )
	{
		var path = userId + "/scores";
		graphRequest( path, onComplete );
	}

	#endregion

}