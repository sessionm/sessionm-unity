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

void SMSetCallbackGameObjectName(char *gameObjectName);
void SMSetServiceRegion(SMServiceRegion region);

@interface SessionM_Unity : NSObject

+ (SessionM_Unity *)sharedInstance;

@end
