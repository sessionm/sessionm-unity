package com.sessionm.unity;

import android.util.Log;
import android.view.Menu;

import com.sessionm.api.AchievementActivity;
import com.sessionm.api.AchievementActivity.AchievementDismissType;
import com.sessionm.api.AchievementActivityIllegalStateException;
import com.sessionm.api.AchievementData;
import com.sessionm.api.Activity;
import com.sessionm.api.SessionM.ActivityType;
import com.sessionm.api.SessionM;
import com.sessionm.api.User;
import com.unity3d.player.UnityPlayerActivity;

public class BaseNativeActivity extends UnityPlayerActivity {

    private final static String TAG = "SessionM.Unity";
    private final static String VERSION_NUM = "2.0.1";
    
    private final SessionM sessionM = SessionM.getInstance();
    
    public static final void setCallbackGameObjectName(String name) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, "Callback game object name: " + name);
        }
        SessionMListener.getInstance().setCallbackGameObjectName(name);
    }
   
    // Convenience Unity/Android bridge methods
    
    public boolean presentActivity(ActivityType type) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, "Present activity: " + type);
        }

        return sessionM.presentActivity(type);
    }
    
    public boolean isActivityAvailable(ActivityType type) {
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
    
    public int getUnclaimedAchievementCount() {
        return sessionM.getUser().getUnclaimedAchievementCount();
    }

    public String getUnclaimedAchievementJSON() {
        String json = "";
        AchievementData achievement = sessionM.getUnclaimedAchievement();
        if(achievement != null) {
            json = SessionMListener.getAchievementJSON(achievement);
        }
        return json;
    }

    public String getUser() {
        String json = "";
        User user = sessionM.getUser();
        if(user != null) {
            json = SessionMListener.getUserJSON(user);
        }
        return json;
    }

    public void setUserOptOutStatus(boolean status){
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".setUserOptOutStatus()");
        }
        sessionM.getUser().setOptedOut(this, status);
    }

    public String getRewardsJSON(){
        return SessionMListener.getRewardsJSON();
    }

    public void updateAchievementsList(){
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".updateAchievementsList");
        }
        sessionM.updateAchievementsList();
    }

    public void setShouldAutoUpdateAchievementsList(boolean auto){
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".updateAchievementsList");
        }
        sessionM.setShouldAutoUpdateAchievementsList(auto);
    }

    public void setServiceRegion(int serviceRegion) {
        sessionM.setServerType(SessionM.SERVER_TYPE_CUSTOM, "https://api.tour-sessionm.com");
        //if (serviceRegion == 0)
            //sessionM.setServerType(SessionM.SERVER_TYPE_PRODUCTION);
    }

    public boolean notifyCustomAchievementPresented() {
        AchievementData achievement = sessionM.getUnclaimedAchievement();
        if(achievement == null) {
            // this cannot happen
            Log.e(TAG, this + ".notifyCustomAchievementPresented(): Null achievement");
            return false;
        }

        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".notifyCustomAchievementPresented(), achievement: " + achievement);
        }

        AchievementActivity activity = new AchievementActivity(achievement);
        try {
            activity.notifyPresented();
            return true;
        } catch (AchievementActivityIllegalStateException e) {
            Log.e(TAG, this + ".notifyCustomAchievementPresented()", e);
            return false;
        }
    }
    
    public void notifyCustomAchievementCancelled() {
        Activity activity = sessionM.getCurrentActivity();
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".notifyCustomAchievementCancelled(), activity: " + activity);
        }

        if(activity instanceof AchievementActivity) {
            AchievementActivity a = (AchievementActivity)activity;
            try {
                a.notifyDismissed(AchievementDismissType.CANCELLED);
            } catch (AchievementActivityIllegalStateException e) {
                Log.e(TAG, this + ".notifyCustomAchievementCancelled()", e);
            }
        }
    }

    public void notifyCustomAchievementClaimed() {
        Activity activity = sessionM.getCurrentActivity();
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".notifyCustomAchievementClaimed(), activity: " + activity);
        }

        if(activity instanceof AchievementActivity) {
            AchievementActivity a = (AchievementActivity)activity;
            try {
                a.notifyDismissed(AchievementDismissType.CLAIMED);
            } catch (AchievementActivityIllegalStateException e) {
                Log.e(TAG, this + ".notifyCustomAchievementClaimed()", e);
            }
        }
    }

    // Activity 
    
    @Override
    protected void onStart() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onStart()");
        }

        super.onStart();
        sessionM.setPluginSDK("Unity", VERSION_NUM);
        sessionM.onActivityStart(this);
    }

    @Override
    protected void onRestart() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onRestart()");
        }       
        super.onRestart();
    }

    @Override
    protected void onResume() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onResume()");
        }

        super.onResume();
        sessionM.onActivityResume(this);
    }

    @Override
    protected void onPause() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onPause()");
        }

        sessionM.onActivityPause(this);
        super.onPause();
    }

    @Override
    protected void onStop() {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onStop()");
        }

        super.onStop();
        sessionM.onActivityStop(this);
    }

    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        sessionM.dismissActivity();
        return super.onPrepareOptionsMenu(menu);
    }
}
