//
//  FacebookBinding.m
//  Facebook
//
//  Created by Mike on 9/13/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "FacebookManager.h"
#import <FacebookSDK/FacebookSDK.h>
#import "P31SharedTools.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil



void _facebookInit()
{
	// check for a URL scheme
	NSDictionary *dict = [[NSBundle mainBundle] infoDictionary];
	if( ![[dict allKeys] containsObject:@"CFBundleURLTypes"] )
		NSLog( @"ERROR: You have not setup a URL scheme. Authentication via Safari or the Facebook.app will not work" );
	
	[[FacebookManager sharedManager] startSessionQuietly];
}


const char * _facebookGetAppLaunchUrl()
{
	NSString *url = [FacebookManager sharedManager].appLaunchUrl;
	if( url )
		return MakeStringCopy( url );
	return MakeStringCopy( @"" );
}


void _facebookSetSessionLoginBehavior( int behavior )
{
	FBSessionLoginBehavior loginBehavior = (FBSessionLoginBehavior)behavior;
	[FacebookManager sharedManager].loginBehavior = loginBehavior;
}


void _facebookEnableFrictionlessRequests()
{
	[[FacebookManager sharedManager] enableFrictionlessRequests];
}


void _facebookRenewCredentialsForAllFacebookAccounts()
{
	[[FacebookManager sharedManager] renewCredentialsForAllFacebookAccounts];
}


bool _facebookIsLoggedIn()
{
	return [[FacebookManager sharedManager] isLoggedIn];
}


const char * _facebookGetFacebookAccessToken()
{
	return MakeStringCopy( [[FacebookManager sharedManager] accessToken] );
}


const char * _facebookGetSessionPermissions()
{
	NSString *permissionsString = [P31 jsonStringFromObject:[[FacebookManager sharedManager] sessionPermissions]];
	return MakeStringCopy( permissionsString );
}


void _facebookLoginUsingDeprecatedAuthorizationFlowWithRequestedPermissions( const char * perms, const char * urlSchemeSuffix )
{
	NSString *permsString = GetStringParam( perms );
	NSMutableArray *permissions = nil;
	
	if( permsString.length == 0 )
		permissions = [NSMutableArray array];
	else
		permissions = [[[permsString componentsSeparatedByString:@","] mutableCopy] autorelease];
	
	[[FacebookManager sharedManager] loginUsingDeprecatedAuthorizationFlowWithRequestedPermissions:permissions
												   urlSchemeSuffix:GetStringParam( urlSchemeSuffix )];
}


void _facebookLoginWithRequestedPermissions( const char * perms, const char * urlSchemeSuffix )
{
	NSString *permsString = GetStringParam( perms );
	NSMutableArray *permissions = nil;
	
	if( permsString.length == 0 )
		permissions = [NSMutableArray array];
	else
		permissions = [[[permsString componentsSeparatedByString:@","] mutableCopy] autorelease];
	
	[[FacebookManager sharedManager] loginWithRequestedPermissions:permissions
												   urlSchemeSuffix:GetStringParam( urlSchemeSuffix )];
}


void _facebookReauthorizeWithReadPermissions( const char * perms )
{
	NSString *permsString = GetStringParam( perms );
	NSMutableArray *permissions = [[[permsString componentsSeparatedByString:@","] mutableCopy] autorelease];
	
	[[FacebookManager sharedManager] reauthorizeWithReadPermissions:permissions];
}


void _facebookReauthorizeWithPublishPermissions( const char * perms, int defaultAudience )
{
	NSString *permsString = GetStringParam( perms );
	NSMutableArray *permissions = [[[permsString componentsSeparatedByString:@","] mutableCopy] autorelease];
	
	[[FacebookManager sharedManager] reauthorizeWithPublishPermissions:permissions defaultAudience:(FBSessionDefaultAudience)defaultAudience];
}


void _facebookLogout()
{
	[[FacebookManager sharedManager] logout];
}


void _facebookShowDialog( const char * dialogType, const char * json )
{
	// make sure we have a legit dictionary
	NSString *jsonString = GetStringParamOrNil( json );
	NSMutableDictionary *dict = nil;
	
	if( jsonString && [jsonString isKindOfClass:[NSString class]] && jsonString.length )
		dict = [[P31 objectFromJsonString:jsonString] mutableCopy];
	
	[[FacebookManager sharedManager] showDialog:GetStringParam( dialogType ) withParms:dict];
}


void _facebookGraphRequest( const char * graphPath, const char * httpMethod, const char * jsonDict )
{
	// make sure we have a legit dictionary
	NSString *jsonString = GetStringParam ( jsonDict );
	NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
	
	if( ![dict isKindOfClass:[NSMutableDictionary class]] )
		return;
	
	[[FacebookManager sharedManager] requestWithGraphPath:GetStringParam( graphPath )
											   httpMethod:GetStringParam( httpMethod )
												   params:dict];
}



// Facebook Composer and share dialog methods
bool _facebookCanUserUseFacebookComposer()
{
	return [FacebookManager userCanUseFacebookComposer];
}


void _facebookShowFacebookComposer( const char * message, const char * imagePath, const char * link )
{
	NSString *path = GetStringParamOrNil( imagePath );
	UIImage *image = nil;
	if( [[NSFileManager defaultManager] fileExistsAtPath:path] )
		image = [UIImage imageWithContentsOfFile:path];
	
	[[FacebookManager sharedManager] showFacebookComposerWithMessage:GetStringParam( message ) image:image link:GetStringParamOrNil( link )];
}


void _facebookShowFacebookShareDialog( const char * json )
{
	// make sure we have a legit dictionary
	NSString *jsonString = GetStringParamOrNil( json );
	
	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
		if( [dict isKindOfClass:[NSDictionary class]] )
		{
			// translate the dict to FBShareDialogParams
			FBLinkShareParams *params = [[[FBLinkShareParams alloc] init] autorelease];
			NSArray *allKeys = [dict allKeys];
			
			for( NSString *k in allKeys )
			{
				SEL setter = NSSelectorFromString( [NSString stringWithFormat:@"set%@:", [k capitalizedString]] );
				if( [params respondsToSelector:setter] )
				{
					// handle the two NSURL cases
					if( [k isEqualToString:@"link"] || [k isEqualToString:@"picture"] )
						[params performSelector:setter withObject:[NSURL URLWithString:[dict objectForKey:k]]];
					else
						[params performSelector:setter withObject:[dict objectForKey:k]];
				}
			}
			
			BOOL canShow = [FBDialogs canPresentShareDialogWithParams:params];
			if( canShow )
				[[FacebookManager sharedManager] showShareDialogWithParams:params];
			else
				_facebookShowDialog( "stream.publish", json );
		}
	}
	else
	{
		NSLog( @"failed to show share dialog due to invalid parameters: %@", jsonString );
	}
}



// App Events
void _facebookSetAppVersion( const char * version )
{
	[FBSettings setAppVersion:GetStringParam( version )];
}


void _facebookLogEvent( const char * event )
{
	[FBAppEvents logEvent:GetStringParam( event )];
}


void _facebookLogEventWithParameters( const char * event, const char * json )
{
	NSString *jsonString = GetStringParamOrNil( json );
	
	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
		if( [dict isKindOfClass:[NSDictionary class]] )
		{
			[FBAppEvents logEvent:GetStringParam( event ) parameters:dict];
		}
	}
}


void _facebookLogEventAndValueToSum( const char * event, double valueToSum )
{
	[FBAppEvents logEvent:GetStringParam( event ) valueToSum:valueToSum];
}


void _facebookLogEventAndValueToSumWithParameters( const char * event, double valueToSum, const char * json )
{
	NSString *jsonString = GetStringParamOrNil( json );
	
	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
		if( [dict isKindOfClass:[NSDictionary class]] )
		{
			[FBAppEvents logEvent:GetStringParam( event ) valueToSum:valueToSum parameters:dict];
		}
	}
}

