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
void SMSetServerURL (const char *url);
void SMSetPortalURL(const char *url);
void SMSetAdURL(const char *url);
void SMStopSession(void);

@interface SessionM_Unity : NSObject

+ (SessionM_Unity *)sharedInstance;

@end
