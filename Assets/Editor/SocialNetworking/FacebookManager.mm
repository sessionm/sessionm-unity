//
//  FacebookManager.m
//  Facebook
//
//  Created by Mike on 9/13/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "FacebookManager.h"
#import <objc/runtime.h>
#import <Accounts/Accounts.h>
#import <Social/Social.h>
#if UNITY_VERSION >= 430
#include "AppDelegateListener.h"
#endif


NSString* const kFacebookUrlSchemeSuffixKey = @"kFacebookUrlSchemeKey";


void UnitySendMessage( const char * className, const char * methodName, const char * param );
void UnityPause( bool pause );
UIViewController *UnityGetGLViewController();


@implementation FacebookManager

@synthesize urlSchemeSuffix, appLaunchUrl, loginBehavior;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (void)load
{
#if UNITY_VERSION >= 430
	UnityRegisterAppDelegateListener( (id)[self sharedManager] );
#endif
	[[NSNotificationCenter defaultCenter] addObserver:[self sharedManager]
											 selector:@selector(applicationDidFinishLaunching:)
												 name:UIApplicationDidFinishLaunchingNotification
											   object:nil];

	[[NSNotificationCenter defaultCenter] addObserver:[self sharedManager]
											 selector:@selector(applicationDidBecomeActive:)
												 name:UIApplicationDidBecomeActiveNotification
											   object:nil];
}


+ (FacebookManager*)sharedManager
{
	static dispatch_once_t pred;
	static FacebookManager *_sharedInstance = nil;

	dispatch_once( &pred, ^{ _sharedInstance = [[self alloc] init]; } );
	return _sharedInstance;
}


+ (BOOL)userCanUseFacebookComposer
{
	Class slComposer = NSClassFromString( @"SLComposeViewController" );
	if( slComposer && [SLComposeViewController isAvailableForServiceType:SLServiceTypeFacebook] )
		return YES;
	return NO;
}


- (id)init
{
	if( ( self = [super init] ) )
	{
		self.loginBehavior = FBSessionLoginBehaviorUseSystemAccountIfPresent;

		// if we have an appId or urlSchemeSuffix tucked away, set it now
		if( [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookUrlSchemeSuffixKey] )
			self.urlSchemeSuffix = [[NSUserDefaults standardUserDefaults] objectForKey:kFacebookUrlSchemeSuffixKey];

		// check for FB App ID
		NSDictionary *dict = [[NSBundle mainBundle] infoDictionary];
		if( ![[dict allKeys] containsObject:@"FacebookAppID"] )
		{
			NSLog( @"ERROR: You have not setup your Facebook app ID in the Info.plist file. Not having it in the Info.plist will cause your application to crash so the plugin is disabling itself." );
			return nil;
		}
		
		// check for a URL scheme
		if( ![[dict allKeys] containsObject:@"CFBundleURLTypes"] )
			NSLog( @"ERROR: You have not setup a URL scheme. Authentication via Safari or the Facebook.app will not work" );

		[self performSelector:@selector(publishPluginUsage) withObject:nil afterDelay:10];
		[self renewCredentialsForAllFacebookAccounts];
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - NSNotifications

- (void)applicationDidFinishLaunching:(NSNotification*)note
{
	// did we get launched with a userInfo dict?
	if( note.userInfo )
	{
		NSURL *url = [note.userInfo objectForKey:UIApplicationLaunchOptionsURLKey];
		if( url )
		{
			NSLog( @"recovered URL from jettisoned app. going to attempt login" );
			[self handleOpenURL:url sourceApplication:nil];
		}
	}
}


- (void)applicationDidBecomeActive:(NSNotification*)note
{
	[FBSession.activeSession handleDidBecomeActive];
}


- (void)onOpenURL:(NSNotification*)notification
{
	[self handleOpenURL:notification.userInfo[@"url"] sourceApplication:notification.userInfo[@"sourceApplication"]];
}


+ (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
{
	return [[FacebookManager sharedManager] handleOpenURL:url sourceApplication:sourceApplication];
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private/Internal

- (BOOL)handleOpenURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication
{
	NSLog( @"url used to open app: %@", url );
	self.appLaunchUrl = url.absoluteString;

	return [FBAppCall handleOpenURL:url sourceApplication:sourceApplication];
}


- (NSString*)getAppId
{
	return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"FacebookAppID"];
}


- (NSString*)urlEncodeValue:(NSString*)str
{
	NSString *result = (NSString*)CFURLCreateStringByAddingPercentEscapes( kCFAllocatorDefault, (CFStringRef)str, NULL, CFSTR( "?=&+" ), kCFStringEncodingUTF8 );
	return [result autorelease];
}


- (void)publishPluginUsage
{
	dispatch_async( dispatch_get_global_queue( DISPATCH_QUEUE_PRIORITY_LOW, 0 ), ^
	{
		NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:[self getAppId], @"appid",
							  @"prime31_socialnetworking", @"resource",
							  @"1.0.0", @"version", nil];

		// prep the post data
		NSString *post = [NSString stringWithFormat:@"plugin=featured_resources&payload=%@", [self urlEncodeValue:[P31 jsonStringFromObject:dict]]];
		NSData *postData = [post dataUsingEncoding:NSASCIIStringEncoding allowLossyConversion:YES];
		NSString *postLength = [NSString stringWithFormat:@"%d", postData.length];

		// prep the request
		NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
		[request setURL:[NSURL URLWithString:@"https://www.facebook.com/impression.php"]];
		[request setHTTPMethod:@"POST"];
		[request setValue:postLength forHTTPHeaderField:@"Content-Length"];
		[request setValue:@"application/x-www-form-urlencoded" forHTTPHeaderField:@"Content-Type"];
		[request setHTTPBody:postData];

		// send the request
		NSURLResponse *response = nil;
		[NSURLConnection sendSynchronousRequest:request returningResponse:&response error:NULL];
	});
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - SLComposer

- (void)showFacebookComposerWithMessage:(NSString*)message image:(UIImage*)image link:(NSString*)link
{
	if( ![FacebookManager userCanUseFacebookComposer] )
		return;

	Class slComposerClass = NSClassFromString( @"SLComposeViewController" );
	UIViewController *slComposer = [slComposerClass performSelector:@selector(composeViewControllerForServiceType:) withObject:@"com.apple.social.facebook"];

	if( !slComposer )
		return;

	// Add a message
	[slComposer performSelector:@selector(setInitialText:) withObject:message];

	// add an image
	if( image )
		[slComposer performSelector:@selector(addImage:) withObject:image];

	// add a link
	if( link )
		[slComposer performSelector:@selector(addURL:) withObject:[NSURL URLWithString:link]];

	// set a blocking handler for the tweet sheet
	[slComposer performSelector:@selector(setCompletionHandler:) withObject:^( NSInteger result )
	{
		UnityPause( false );
		[UnityGetGLViewController() dismissModalViewControllerAnimated:YES];

		if( result == 1 )
			UnitySendMessage( "FacebookManager", "facebookComposerCompleted", "1" );
		else if( result == 0 )
			UnitySendMessage( "FacebookManager", "facebookComposerCompleted", "0" );
	}];

	// Show the tweet sheet
	UnityPause( true );
	[UnityGetGLViewController() presentModalViewController:slComposer animated:YES];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Facebook Share Dialog

- (void)showShareDialogWithParams:(FBLinkShareParams*)dialogParams
{
	[FBDialogs presentShareDialogWithParams:dialogParams clientState:nil handler:^( FBAppCall *call, NSDictionary *results, NSError *error )
	 {
		 if( error )
		 {
			 NSLog( @"Share Dialog error: %@", error );
			 UnitySendMessage( "FacebookManager", "shareDialogFailed", [P31 jsonFromError:error] );
		 }
		 else
		 {
			 UnitySendMessage( "FacebookManager", "shareDialogSucceeded", [P31 jsonStringFromObject:results].UTF8String );
		 }
	 }];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)enableFrictionlessRequests
{
	if( [FBSession activeSession] == nil )
	{
		NSLog( @"error: there is no active session. You cannot enable frictionless requests until you have an active session" );
		return;
	}

	self.frictionlessRecipientCache = [[[FBFrictionlessRecipientCache alloc] init] autorelease];
	[self.frictionlessRecipientCache prefetchAndCacheForSession:[FBSession activeSession] completionHandler:^( FBRequestConnection *conn, id result, NSError *error )
	{
		if( error )
			NSLog( @"error pre-fetching frictionless recipient cache: %@", error );
	}];
}


- (void)renewCredentialsForAllFacebookAccounts
{
	if( !NSClassFromString( @"ACAccountStore" ) )
		return;

	if( &ACAccountTypeIdentifierFacebook == NULL )
		return;

	ACAccountStore *accountStore = [[ACAccountStore alloc] init];

	if( !accountStore )
		return;

    ACAccountType *accountTypeFB = [accountStore accountTypeWithAccountTypeIdentifier:ACAccountTypeIdentifierFacebook];
	NSArray *facebookAccounts = [accountStore accountsWithAccountType:accountTypeFB];

	if( facebookAccounts.count == 0 )
		return;

	for( ACAccount *fbAccount in facebookAccounts )
	{
		[accountStore renewCredentialsForAccount:fbAccount completion:^( ACAccountCredentialRenewResult renewResult, NSError *error )
		{
			/*
			if( error )
				NSLog( @"account %@ failed to renew: %@", fbAccount, error );
			else
				NSLog( @"account %@ renewed successfully", fbAccount );
			*/
		}];
	}
}


- (void)startSessionQuietly
{
	[FBAppEvents activateApp];
	[FBSettings setDefaultAppID:[self getAppId]];

	// create a session manually in case we have a url scheme suffix
	FBSession *facebookSession = [[FBSession alloc] initWithAppID:[self getAppId] permissions:nil urlSchemeSuffix:self.urlSchemeSuffix tokenCacheStrategy:nil];
	[FBSession setActiveSession:facebookSession];

	if( [FBSession openActiveSessionWithAllowLoginUI:NO] )
	{
		UnitySendMessage( "FacebookManager", "sessionOpened", FBSession.activeSession.accessTokenData.accessToken.UTF8String );
	}
}


- (BOOL)isLoggedIn
{
	if( !FBSession.activeSession )
		return NO;

	return FBSession.activeSession.isOpen;
}


- (NSString*)accessToken
{
    return FBSession.activeSession.accessTokenData.accessToken;
}


- (NSArray*)sessionPermissions
{
	return FBSession.activeSession.permissions;
}


- (void)loginUsingDeprecatedAuthorizationFlowWithRequestedPermissions:(NSMutableArray*)permissions urlSchemeSuffix:(NSString*)aUrlSchemeSuffix
{
	// store the url scheme suffix for later use
	self.urlSchemeSuffix = aUrlSchemeSuffix;
	[[NSUserDefaults standardUserDefaults] setObject:self.urlSchemeSuffix forKey:kFacebookUrlSchemeSuffixKey];

	if( [self isLoggedIn] )
	{
		UnitySendMessage( "FacebookManager", "sessionOpened", FBSession.activeSession.accessTokenData.accessToken.UTF8String );
		return;
	}

	// we must have email, user_birthday, or user_location to authorize!
	if( ![permissions containsObject:@"email"] && ![permissions containsObject:@"user_birthday"] && ![permissions containsObject:@"user_location"] )
		[permissions addObject:@"email"];


	FBSession *facebookSession = [[FBSession alloc] initWithAppID:[self getAppId] permissions:permissions urlSchemeSuffix:aUrlSchemeSuffix tokenCacheStrategy:nil];
	[FBSession setActiveSession:facebookSession];
	[FBSession openActiveSessionWithPermissions:permissions allowLoginUI:YES completionHandler:^( FBSession *sess, FBSessionState status, NSError *error )
	 {
		 if( FB_ISSESSIONOPENWITHSTATE( status ) )
		 {
			 UnitySendMessage( "FacebookManager", "sessionOpened", FBSession.activeSession.accessTokenData.accessToken.UTF8String );
		 }
		 else
		 {
			 if( status == FBSessionStateClosed )
			 {
				 NSLog( @"session closed" );
				 //UnitySendMessage( "FacebookManager", "facebookDidLogout", "" );
			 }
			 else
			 {
				 UnitySendMessage( "FacebookManager", "loginFailed", [P31 jsonFromError:error] );
			 }
		 }
	 }];
}


- (void)loginWithRequestedPermissions:(NSMutableArray*)permissions urlSchemeSuffix:(NSString*)aUrlSchemeSuffix
{
	// store the url scheme suffix for later use
	self.urlSchemeSuffix = aUrlSchemeSuffix;
	[[NSUserDefaults standardUserDefaults] setObject:self.urlSchemeSuffix forKey:kFacebookUrlSchemeSuffixKey];

	if( [self isLoggedIn] )
	{
		UnitySendMessage( "FacebookManager", "sessionOpened", FBSession.activeSession.accessTokenData.accessToken.UTF8String );
		return;
	}


	FBSession *facebookSession = [[FBSession alloc] initWithAppID:[self getAppId] permissions:permissions urlSchemeSuffix:self.urlSchemeSuffix tokenCacheStrategy:nil];
	[FBSession setActiveSession:facebookSession];

	// change the behavior here to force different types. the available tyeps are: FBSessionLoginBehaviorUseSystemAccountIfPresent, FBSessionLoginBehaviorWithFallbackToWebView,
	// FBSessionLoginBehaviorWithNoFallbackToWebView, FBSessionLoginBehaviorForcingWebView
	[facebookSession openWithBehavior:self.loginBehavior completionHandler:^( FBSession *sess, FBSessionState status, NSError *error )
	{
		 if( FB_ISSESSIONOPENWITHSTATE( status ) )
		 {
			 UnitySendMessage( "FacebookManager", "sessionOpened", FBSession.activeSession.accessTokenData.accessToken.UTF8String );
		 }
		 else
		 {
			 if( status == FBSessionStateClosed )
			 {
				 NSLog( @"session closed" );
				 //UnitySendMessage( "FacebookManager", "facebookDidLogout", "" );
			 }
			 else if( error )
			 {
				 NSLog( @"session creation error: %@ userInfo: %@", error, error.userInfo ? error.userInfo : @"no userInfo" );
				 UnitySendMessage( "FacebookManager", "loginFailed", [P31 jsonFromError:error] );
			 }
			 else
			 {
				 NSLog( @"session creation failed with no error. Session: %@, status: %i", sess, status );
				 UnitySendMessage( "FacebookManager", "loginFailed", "Unknown Error" );
			 }
		 }
	 }];
}


- (void)reauthorizeWithReadPermissions:(NSArray*)permissions
{
	if( ![FBSession activeSession] )
		return;
	
	[[FBSession activeSession] requestNewReadPermissions:permissions completionHandler:^( FBSession *session, NSError *error )
	{
		if( error )
			UnitySendMessage( "FacebookManager", "reauthorizationFailed", [P31 jsonFromError:error] );
		else
			UnitySendMessage( "FacebookManager", "reauthorizationSucceeded", "" );
	}];
}


- (void)reauthorizeWithPublishPermissions:(NSArray*)permissions defaultAudience:(FBSessionDefaultAudience)audience
{
	if( ![FBSession activeSession] )
		return;
	
	[[FBSession activeSession] requestNewPublishPermissions:permissions defaultAudience:audience completionHandler:^( FBSession *session, NSError *error )
	{
		if( error )
			UnitySendMessage( "FacebookManager", "reauthorizationFailed", [P31 jsonFromError:error] );
		else
			UnitySendMessage( "FacebookManager", "reauthorizationSucceeded", "" );
	}];
}


- (void)logout
{
	[FBSession.activeSession closeAndClearTokenInformation];
	//[FBSession.activeSession close];
}


- (void)showDialog:(NSString*)dialogType withParms:(NSMutableDictionary*)dict
{
	id dialogHandler = ^( FBWebDialogResult result, NSURL *resultURL, NSError *error )
	{
		if( result == FBWebDialogResultDialogCompleted )
		{
			UnitySendMessage( "FacebookManager", "dialogCompletedWithUrl", resultURL ? resultURL.absoluteString.UTF8String : "" );
		}
		else
		{
			UnitySendMessage( "FacebookManager", "dialogFailedWithError", [P31 jsonFromError:error] );
		}
	};

	if( [dialogType isEqualToString:@"apprequests"] )
	{
		NSArray *allKeys = [dict allKeys];
		NSString *message = @"You forgot to pass in a message parameter";
		NSString *title = @"You forgot to pass in a title parameter";

		if( [allKeys containsObject:@"message"] )
			message = [dict objectForKey:@"message"];

		if( [allKeys containsObject:@"title"] )
			title = [dict objectForKey:@"title"];

		if( [message isEqualToString:@""] || [title isEqualToString:@""] )
			NSLog( @"Note that the apprequests dialog requires both a title and message parameter. The plugin just saved you from an error by adding them for you" );

		[FBWebDialogs presentRequestsDialogModallyWithSession:nil
													  message:message
														title:title
												   parameters:dict
													  handler:dialogHandler
												  friendCache:self.frictionlessRecipientCache];
		return;
	}


	[FBWebDialogs presentDialogModallyWithSession:nil dialog:dialogType parameters:dict handler:dialogHandler];
}


- (void)requestWithGraphPath:(NSString*)path httpMethod:(NSString*)method params:(NSDictionary*)params
{
	[FBRequestConnection startWithGraphPath:path parameters:params HTTPMethod:method completionHandler:^( FBRequestConnection *conn, id result, NSError *error )
	{
		if( error )
		{
			UnitySendMessage( "FacebookManager", "graphRequestFailed", [P31 jsonFromError:error] );
		}
		else
		{
			UnitySendMessage( "FacebookManager", "graphRequestCompleted", [P31 jsonStringFromObject:result].UTF8String );
		}
	}];
}


- (void)requestWithRestMethod:(NSString*)restMethod httpMethod:(NSString*)method params:(NSMutableDictionary*)params
{
	NSLog( @"Facebook has officially removed REST API calls for the Graph API version 2.1" );
}

@end




// we only want the old category hack in Unity versions before 4.3 where the AppDelegateListener was introduced
#if UNITY_VERSION < 430

#if UNITY_VERSION < 420

#import "AppController.h"
@implementation AppController(FacebookURLHandler)

#else

#import "UnityAppController.h"
@implementation UnityAppController(FacebookURLHandler)

#endif

- (BOOL)application:(UIApplication*)application
			openURL:(NSURL*)url
  sourceApplication:(NSString*)sourceApplication
		 annotation:(id)annotation
{
	// Any classes added here should have a class method with the signature application:openURL:sourceApplication:annotation:
	NSArray *classesThatNeedToHandleOpenUrl = @[ @"GPlayManager", @"FacebookManager" ];
	
	for( NSString *className in classesThatNeedToHandleOpenUrl )
	{
		Class klass = NSClassFromString( className );
		if( [klass respondsToSelector:@selector(application:openURL:sourceApplication:annotation:)] )
			[klass application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
	}
	
	return YES;
}

@end

#endif

