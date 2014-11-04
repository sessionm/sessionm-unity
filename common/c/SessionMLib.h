//
//  SessionMLib.h
//
//  Copyright (c) 2014 SessionM. All rights reserved.
//

#ifndef SessionMLib_h
#define SessionMLib_h

#include "SessionM.h"

void SMStartSession(char *appId);
void SMLogAction(const char *action);
void SMLogActions(const char *action, int count);
BOOL SMPresentActivity(SMActivityType type);
void SMDismissActivity(void);
BOOL SMIsActivityAvailable(SMActivityType type);
BOOL SMIsActivityPresented(void);
SMLogLevel SMGetLogLevel(void);
void SMSetLogLevel(SMLogLevel level);
const char *SMGetSDKVersion(void);
void SMSetMetaData(const char *data, const char *key);
SessionMState SMGetSessionState(void);
void SMPlayerDataSetUserOptOutStatus(BOOL optOut);
long SMPlayerDataGetUnclaimedAchievementCount(void);

const char *SMGetUnclaimedAchievementJSON(void);
const char *SMGetUserJSON(void);
void SMNotifyCustomAchievementPresented(const char *achievementId);
void SMNotifyCustomAchievementDismissed(void);
void SMNotifyCustomAchievementClaimed(void);

#endif
