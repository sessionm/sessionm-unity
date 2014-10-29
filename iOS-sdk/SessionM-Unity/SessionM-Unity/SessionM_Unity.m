//
//  SessionM_Unity.m
//
//  Copyright (c) 2014 SessionM. All rights reserved.
//

#import "SessionM_Unity.h"

#pragma mark - Interface

// An extern function implemented by the SessionM Unity plugin
extern void UnitySendMessage(const char* obj, const char* method, const char* msg);

// Utility functions
static NSString *SMPackStrings(NSArray * strings);
static NSString *SMAchievementDataToJSONString(SMAchievementData *achievementData);
static NSString *SMUserToJSONString(SMUser *user);
SessionM_Unity *__unityClientSharedInstance;

@interface SessionM_Unity()<SessionMDelegate>

@property(nonatomic, copy) NSString *callbackGameObjectName;
@property(nonatomic, strong) SMAchievementActivity *customAchievementActivity;

- (void)invokeUnityGameObjectMethod:(NSString *)methodName message:(NSString *)message;

@end


#pragma mark - Implementation

@implementation SessionM_Unity

@synthesize callbackGameObjectName;

+ (SessionM_Unity *)sharedInstance {
    if (!__unityClientSharedInstance) {
        __unityClientSharedInstance = [[SessionM_Unity alloc] init];
        [[SessionM sharedInstance] setPluginSDK:__SESSIONM_UNITY_SDK_TAG__ version:__SESSIONM_UNITY_SDK_VERSION__];
    }
    return __unityClientSharedInstance;
}


#pragma mark - SessionMDelegate

- (void)sessionM:(SessionM *)sessionM didTransitionToState:(SessionMState)state {
    NSString *stateStr = [NSString stringWithFormat:@"%i", state];
    [self invokeUnityGameObjectMethod:@"_sessionM_HandleStateTransitionMessage" message:stateStr];
}

- (void)sessionM:(SessionM *)sessionM didFailWithError:(NSError *)error {
    NSString *code = [NSString stringWithFormat:@"%ld", (long)error.code];
    NSString *description = error.description ? error.description : @"";
    NSArray *strings = [NSArray arrayWithObjects:code, description, nil];
    NSString *message = SMPackStrings(strings);
    [self invokeUnityGameObjectMethod:@"_sessionM_HandleSessionFailedMessage" message:message];
}

- (BOOL)sessionM:(SessionM *)sessionM shouldAutopresentAchievement:(SMAchievementData *)achievement {
    // Activities are not automatically presented in Unity environment because all Unity callback methods are performed asynchronously (instead the client is expected to use logAction: in conjunction with "activity available" callback or "is activity available" property method)
    
    NSString *jsonString = SMAchievementDataToJSONString(achievement);
    [self invokeUnityGameObjectMethod:@"_sessionM_HandleUpdatedUnclaimedAchievementMessage" message:jsonString];

    return NO;
}

- (void)sessionM:(SessionM *)sessionM didPresentActivity:(SMActivity *)activity {
    NSString *activityType = [NSString stringWithFormat:@"%i", activity.activityType];
    [self invokeUnityGameObjectMethod:@"_sessionM_HandlePresentedActivityMessage" message:activityType];
}

- (void)sessionM:(SessionM *)sessionM didDismissActivity:(SMActivity *)activity {
    NSString *activityType = [NSString stringWithFormat:@"%i", activity.activityType];
    [self invokeUnityGameObjectMethod:@"_sessionM_HandleDismissedActivityMessage" message:activityType];
}

- (void)sessionM:(SessionM *)sessionM didUpdateUser:(SMUser *)user {
    NSString *userJSON = SMUserToJSONString(user);
    [self invokeUnityGameObjectMethod:@"_sessionM_HandleUserInfoChangedMessage" message:userJSON];
}

- (void)sessionM:(SessionM *)sessionM user:(SMUser *)user didPerformAction:(SMActivityUserAction)action forActivity:(SMActivity *)activity withData:(NSDictionary *)data {
    NSDictionary *userActionDict = [NSDictionary dictionaryWithObjectsAndKeys:[NSNumber numberWithInt:action], @"userAction",
                                    data, @"data",
                                    nil];
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:userActionDict
                                                       options:0
                                                         error:&error];
    NSString *jsonString = nil;
    if (jsonData) {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }

    [self invokeUnityGameObjectMethod:@"_sessionM_HandleUserActionMessage" message:jsonString];
}


#pragma mark - Private

// Sends a message to the Unity object specified by callbackGameObjectName to invoke a method that handles a SessionM SDK event
- (void)invokeUnityGameObjectMethod:(NSString *)methodName message:(NSString *)message  {
    NSString *gameObject = self.callbackGameObjectName == nil ? kSMUnityDefaultSessionM_SetCallbackGameObjectName : self.callbackGameObjectName;
    message = (message == nil) ? @"" : message;
    // no need to strdup strings here because UnitySendMessage does it
    const char *gameObjectStr = [gameObject cStringUsingEncoding:NSUTF8StringEncoding];
    const char *methodNameStr = [methodName cStringUsingEncoding:NSUTF8StringEncoding];
    const char *messageStr = [message cStringUsingEncoding:NSUTF8StringEncoding];
    UnitySendMessage(gameObjectStr, methodNameStr, messageStr);
}

@end


#pragma mark - Unity Plugin

// Sets the name of the Unity game object that will be sent messages to invoke handler methods for SessionM SDK events
void SMSetCallbackGameObjectName(char *gameObjectName) {
    NSString *name = [NSString stringWithCString:gameObjectName encoding:NSUTF8StringEncoding];
    SessionM_Unity *client = [SessionM_Unity sharedInstance];
    client.callbackGameObjectName = name;
}

// Starts a session for the specified application ID
void SMStartSession(char *appId) {
    NSString *appIdString = [NSString stringWithCString:appId encoding:NSUTF8StringEncoding];
    SessionM *sessionM = [SessionM sharedInstance];
    [sessionM startSessionWithAppID:appIdString];
    sessionM.delegate = [SessionM_Unity sharedInstance];
}

// Returns the current session state
SessionMState SMGetSessionState(void) {
    return [SessionM sharedInstance].sessionState;
}

// Presents the user portal (if type is SMActivityTypePortal) or the current unclaimed achievement (if type is SMActivityTypeAchievement) to the user
BOOL SMPresentActivity(SMActivityType type) {
    return [[SessionM sharedInstance] presentActivity:type];
}

// Dismisses the user portal or the achievement that is currently being presented
void SMDismissActivity(void) {
    [[SessionM sharedInstance] dismissActivity];
}

// Returns whether the user portal or an achievement is currently being presented to the user
BOOL SMIsActivityPresented(void) {
    return ([SessionM sharedInstance].currentActivity != nil);
}

// Returns whether the user portal or an achievement is available to present to the user
BOOL SMIsActivityAvailable(SMActivityType type) {
    return [[SessionM sharedInstance] isActivityAvailable:type];
}


// Logs a count for the achievement associated with the specified action
void SMLogAction(const char *action) {
    NSString *actionStr = [NSString stringWithCString:action encoding:NSUTF8StringEncoding];
    [[SessionM sharedInstance] logAction:actionStr];
}

// Logs multiple counts for the achievement associated with the specified action
void SMLogActions(const char *action, int count) {
    NSString *actionStr = [NSString stringWithCString:action encoding:NSUTF8StringEncoding];
    [[SessionM sharedInstance] logAction:actionStr withCount:count];
}

// Returns the current debug log level
SMLogLevel SMGetLogLevel(void) {
    return [SessionM sharedInstance].logLevel;
}

// Sets the log level to be used for debugging
void SMSetLogLevel(SMLogLevel level) {
    [SessionM sharedInstance].logLevel = level;
}

// Returns the version number of the SessionM iOS SDK being used
const char *SMGetSDKVersion(void) {
    NSString *str = __SESSIONM_SDK_VERSION__;
    const char *c = [str cStringUsingEncoding:NSUTF8StringEncoding];
    return c ? strdup(c) : NULL;
}

// Sends meta data to SessionM SDK. Please refer to the documentation for more information on common keys. Data should only be supplied in accordance with your application's terms of service and privacy policy.
void SMSetMetaData(const char *data, const char *key) {
    NSString *dataString = [NSString stringWithCString:data encoding:NSUTF8StringEncoding];
    NSString *keyString = [NSString stringWithCString:key encoding:NSUTF8StringEncoding];
    [[SessionM sharedInstance] setMetaData:dataString forKey:keyString];
}

// Sets the SessionM service region
void SMSetServiceRegion(SMServiceRegion region) {
    [[SessionM sharedInstance] setServiceRegion:region];
}

// Returns the user's unclaimed achievement count
long SMPlayerDataGetUnclaimedAchievementCount(void) {
    SMUser *playerData = [SessionM sharedInstance].user;
    return playerData.unclaimedAchievementCount;
}

// Returns a JSON representation of the current unclaimed achievement (returns NULL once achievement is presented)
const char *SMGetUnclaimedAchievementJSON(void) {
    NSString *achievementString = nil;
    SMAchievementData *achievementData = [SessionM sharedInstance].unclaimedAchievement;
    if (achievementData) {
        achievementString = SMAchievementDataToJSONString(achievementData);
    }
    const char *c = [achievementString cStringUsingEncoding:NSUTF8StringEncoding];
    return c ? strdup(c) : NULL;
}

// Notifies SessionM SDK that the current unclaimed custom achievement was presented
void SMNotifyCustomAchievementPresented() {
    SMAchievementData *achievementData = [SessionM sharedInstance].unclaimedAchievement;
    if (achievementData) {
        SessionM_Unity *unityClient = [SessionM_Unity sharedInstance];
        unityClient.customAchievementActivity = [[SMAchievementActivity alloc] initWithAchievmentData:achievementData];
        [unityClient.customAchievementActivity notifyPresented];
    }
}

// Notifies SessionM SDK that the current presented custom achievement was dismissed
void SMNotifyCustomAchievementDismissed(void) {
    SessionM_Unity *unityClient = [SessionM_Unity sharedInstance];
    [unityClient.customAchievementActivity notifyDismissed:SMAchievementDismissTypeCanceled];
}

// Notifies SessionM SDK that the current presented custom achievement was claimed
void SMNotifyCustomAchievementClaimed(void) {
    SessionM_Unity *unityClient = [SessionM_Unity sharedInstance];
    [unityClient.customAchievementActivity notifyDismissed:SMAchievementDismissTypeClaimed];
}


#pragma mark - Utility

static NSString *SMAchievementDataToJSONString(SMAchievementData *achievementData) {
    NSDictionary *achievementDict = @{
                                      @"name": achievementData.name,
                                      @"message": achievementData.message,
                                      @"mpointValue": [NSNumber numberWithUnsignedInteger:achievementData.mpointValue],
                                      @"identifier": @"",
                                      @"isCustom": [NSNumber numberWithBool:achievementData.isCustom]
                                      };
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:achievementDict
                                                       options:0
                                                         error:&error];
    NSString *jsonString = nil;
    if (!error || jsonData) {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }

    return jsonString;
}

static NSString *SMUserToJSONString(SMUser *user) {
    NSDictionary *userDict = @{
                               @"pointBalance": [NSNumber numberWithUnsignedInteger:user.pointBalance],
                               @"optedOut": [NSNumber numberWithBool:user.isOptedOut],
                               @"unclaimedAchievementValue": [NSNumber numberWithUnsignedInteger:user.unclaimedAchievementValue],
                               @"unclaimedAchievementCount": [NSNumber numberWithUnsignedInteger:user.unclaimedAchievementCount]
                               };

    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:userDict
                                                       options:0
                                                         error:&error];
    NSString *jsonString = nil;
    if (!error) {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }

    return jsonString;
}

static NSString *SMPackStrings(NSArray * strings) {
    NSMutableString *packedString = [NSMutableString string];
    for (NSString *string in strings) {
        [packedString appendFormat:@"%ld:%@", (long)string.length, string];
    }

    return packedString;
}