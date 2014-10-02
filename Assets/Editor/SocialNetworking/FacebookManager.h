//
//  FacebookManager.h
//  Facebook
//
//  Created by Mike on 9/13/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <FacebookSDK/FacebookSDK.h>
#import "P31SharedTools.h"



extern NSString *const kFacebookAppIdKey;


@interface FacebookManager : NSObject
@property (nonatomic, copy) NSString *urlSchemeSuffix;
@property (nonatomic, copy) NSString *appLaunchUrl;
@property (nonatomic) FBSessionLoginBehavior loginBehavior;
@property (nonatomic, retain) FBFrictionlessRecipientCache *frictionlessRecipientCache;


+ (FacebookManager*)sharedManager;


- (void)enableFrictionlessRequests;

- (void)renewCredentialsForAllFacebookAccounts;

- (void)startSessionQuietly;


// Composer and share dialog
+ (BOOL)userCanUseFacebookComposer;

- (void)showFacebookComposerWithMessage:(NSString*)message image:(UIImage*)image link:(NSString*)link;

- (void)showShareDialogWithParams:(FBLinkShareParams*)dialogParams;


- (BOOL)isLoggedIn;

- (NSString*)accessToken;

- (NSArray*)sessionPermissions;

- (void)loginUsingDeprecatedAuthorizationFlowWithRequestedPermissions:(NSMutableArray*)permissions urlSchemeSuffix:(NSString*)urlSchemeSuffix;

- (void)loginWithRequestedPermissions:(NSMutableArray*)permissions urlSchemeSuffix:(NSString*)urlSchemeSuffix;

- (void)reauthorizeWithReadPermissions:(NSArray*)permissions;

- (void)reauthorizeWithPublishPermissions:(NSArray*)permissions defaultAudience:(FBSessionDefaultAudience)audience;

- (void)logout;

- (void)showDialog:(NSString*)dialogType withParms:(NSMutableDictionary*)dict;

- (void)requestWithGraphPath:(NSString*)path httpMethod:(NSString*)method params:(NSDictionary*)params;

- (void)requestWithRestMethod:(NSString*)restMethod httpMethod:(NSString*)method params:(NSMutableDictionary*)params;

@end
