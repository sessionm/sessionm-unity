//
//  FlurryBinding.m
//  FlurryTest
//
//  Created by Mike on 5/22/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "FlurryAds.h"
#import "Flurry.h"
#import "FlurryManager.h"
#import "P31SharedTools.h"



// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil

void UnitySendMessage( const char * className, const char * methodName, const char * param );



void _flurryAdsInitialize( BOOL enableTestAds )
{
	[FlurryAds initialize:[P31 unityViewController]];
	
	if( enableTestAds )
		[FlurryAds enableTestAds:enableTestAds];
	[FlurryAds setAdDelegate:[FlurryManager sharedManager]];
}


void _flurryAdsSetUserCookies( const char * cookies )
{
	NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:GetStringParam( cookies )];
	[FlurryAds setUserCookies:dict];
}


void _flurryAdsClearUserCookies()
{
	[FlurryAds clearUserCookies];
}


void _flurryAdsSetKeywords( const char * keywords )
{
	NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:GetStringParam( keywords )];
	[FlurryAds setKeywordsForTargeting:dict];
}


void _flurryAdsClearKeywords()
{
	[FlurryAds clearKeywords];
}


void _flurryAdsFetchAdForSpace( const char * space, int adSize )
{
	[FlurryAds fetchAdForSpace:GetStringParam( space ) frame:[P31 unityViewController].view.frame size:(FlurryAdSize)adSize];
}


void _flurryAdsFetchAndDisplayAdForSpace( const char * space, int adSize )
{
	[FlurryAds fetchAndDisplayAdForSpace:GetStringParam( space ) view:[P31 unityViewController].view viewController:[P31 unityViewController] size:(FlurryAdSize)adSize];
}


BOOL _flurryAdsIsAdAvailableForSpace( const char * space, int adSize )
{
	return [FlurryAds adReadyForSpace:GetStringParam( space )];
}


void _flurryAdsDisplayAdForSpace( const char * space, int adSize )
{
	[FlurryAds displayAdForSpace:GetStringParam( space ) onView:[P31 unityViewController].view viewControllerForPresentation:[P31 unityViewController]];
}


void _flurryAdsRemoveAdFromSpace( const char * space )
{
	[FlurryAds removeAdFromSpace:GetStringParam( space )];
}



