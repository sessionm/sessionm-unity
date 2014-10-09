//
//  SessionM_Unity.h
//
//  Copyright (c) 2014 SessionM. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "SessionMLib.h"

#define __SESSIONM_UNITY_SDK_TAG__ @"unity_sdk"
#define __SESSIONM_UNITY_SDK_VERSION__ @"1.5.1"
#define kSMUnityDefaultSessionM_SetCallbackGameObjectName @"Main Camera"

/*!
 @abstract Sets the name of the Unity game object that will be sent messages to invoke handler methods for SessionM SDK events.
 @discussion Default value will be "Main Camera" if this method is not called. The game object needs to handle the following messages:
    StateTransitionMessage
    SessionFailedMessage
    PresentedActivityMessage
    DismissedActivityMessage
    UpdatedUnclaimedAchievementMessage
    UserInfoChangedMessage
    UserActionMessage
 @param gameObjectName The name of the game object to be set.
 */
void SMSetCallbackGameObjectName(char *gameObjectName);
/*!
 @abstract Sets the SessionM service region.
 @discussion This method should be called before calling SMStartSession.
 @param region The service region.
 */
void SMSetServiceRegion(SMServiceRegion region);

@interface SessionM_Unity : NSObject

+ (SessionM_Unity *)sharedInstance;

@end
