package com.sessionm.unity;

import android.util.Log;

import com.sessionm.api.AchievementActivity;
import com.sessionm.api.AchievementActivity.AchievementDismissType;
import com.sessionm.api.AchievementActivityIllegalStateException;
import com.sessionm.api.AchievementData;
import com.sessionm.api.Activity;
import com.sessionm.api.SessionM.ActivityType;
import com.sessionm.api.SessionM;

public class SessionMPlugin{

    private final static String TAG = "SessionM.Unity";
    
    private final static SessionM sessionM = SessionM.getInstance();
    private static android.app.Activity ac = com.unity3d.player.UnityPlayer.currentActivity;
    private final static String VERSION_NUM = "2.0";

    public static final void setCallbackGameObjectName(String name) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, "Callback game object name: " + name);
        }
        SessionMListener.getInstance().setCallbackGameObjectName(name);
    }
    
    public static boolean isActivityAvailable(ActivityType type) {
        boolean available = false;
        if(sessionM.getSessionState() == SessionM.State.STARTED_ONLINE) {
            if(type == ActivityType.ACHIEVEMENT) {
                available = sessionM.getUnclaimedAchievement() != null;
            } else {
                available = true;
            }
        }
        return available;
    }
    
    public static int getUnclaimedAchievementCount() {
        return sessionM.getUser().getUnclaimedAchievementCount();
    }

    public static String getUnclaimedAchievementJSON() {
        String json = null;
        AchievementData achievement = sessionM.getUnclaimedAchievement();
        if(achievement != null) {
            json = SessionMListener.getAchievementJSON(achievement);
        }
        return json;
    }

    public static void notifyCustomAchievementPresented() {
        AchievementData achievement = sessionM.getUnclaimedAchievement();
        if(achievement == null) {
            // this cannot happen 
            Log.e(TAG, ac + ".notifyCustomAchievementPresented(): Null achievement");
            return;
        }
        
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + ".notifyCustomAchievementPresented(), achievement: " + achievement);
        }
        
        AchievementActivity activity = new AchievementActivity(achievement);
        try {
            activity.notifyPresented();
        } catch (AchievementActivityIllegalStateException e) {
            Log.e(TAG, ac + ".notifyCustomAchievementPresented()", e);
        }
    }
    
    public static void notifyCustomAchievementCancelled() {
        Activity activity = sessionM.getCurrentActivity();
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + ".notifyCustomAchievementCancelled(), activity: " + activity);
        }

        if(activity instanceof AchievementActivity) {
            AchievementActivity a = (AchievementActivity)activity;
            try {
                a.notifyDismissed(AchievementDismissType.CANCELLED);
            } catch (AchievementActivityIllegalStateException e) {
                Log.e(TAG, ac + ".notifyCustomAchievementCancelled()", e);
            }
        }
    }

    public static void notifyCustomAchievementClaimed() {
        Activity activity = sessionM.getCurrentActivity();
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + ".notifyCustomAchievementClaimed(), activity: " + activity);
        }

        if(activity instanceof AchievementActivity) {
            AchievementActivity a = (AchievementActivity)activity;
            try {
                a.notifyDismissed(AchievementDismissType.CLAIMED);
            } catch (AchievementActivityIllegalStateException e) {
                Log.e(TAG, ac + ".notifyCustomAchievementClaimed()", e);
            }
        }
    }
  
    // Prime31 Shared Activity 
    public static void onStart() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + "SessionM.onStart()");
        }
        sessionM.setPluginSDK("Unity", VERSION_NUM);
        sessionM.onActivityStart(ac);
    }

    public static void onRestart() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + "SessionM.onRestart()");
        }
    }

    public static void onResume() {
        
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + "SessionM.onResume()");
        }
        setCallbackGameObjectName(ac.toString());
        sessionM.onActivityResume(ac);
    }

    public static void onPause() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + "SessionM.onPause()");
        }
        sessionM.onActivityPause(ac);
    }

    public static void onStop() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, ac + "SessionM.onStop()");
        }
        sessionM.onActivityStop(ac);
    }
}
