package com.sessionm.unity;

import java.util.Locale;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;
import android.widget.FrameLayout;

import com.sessionm.api.AchievementData;
import com.sessionm.api.ActivityListener;
import com.sessionm.api.SessionListener;
import com.sessionm.api.SessionM.ActivityType;
import com.sessionm.api.User;
import com.sessionm.api.SessionM;
import com.unity3d.player.UnityPlayer;

public class SessionMListener implements ActivityListener, SessionListener {
    
    private final static String TAG = "SessionM.Unity";
    private static final SessionM sessionM = SessionM.getInstance();
    private String callbackGameObjectName;
    private static SessionMListener instance;
    private String presentedActivityType;
    
    public synchronized static SessionMListener getInstance() {
        if(instance == null) {
            instance = new SessionMListener();
            sessionM.setActivityListener(instance);
            sessionM.setSessionListener(instance);
       }
        return instance;
    }
    
    public final void setCallbackGameObjectName(String name) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, "Callback game object name: " + name);
        }
        
        callbackGameObjectName = name;
    }
 
    private String getCurrentUnityActivityType() {
        String type = "0";
        ActivityType activityType = sessionM.getCurrentActivity().getActivityType();
        if(activityType != null) {
            switch(activityType) {
                case ACHIEVEMENT:
                    type = "1";
                    break;
                case PORTAL:
                    type = "2";
                    break;
                case INTERSTITIAL:
                    type = "3";
                    break;
                default:
                    break;
            }
        }
        return type;
    }

    private String getCurrentUnitySessionState() {
        String state = "0";
        switch(sessionM.getSessionState()) {
            case STOPPED:
                state = "0";
                break;
            case STARTED_ONLINE:
                state = "1";
                break;
            case STARTED_OFFLINE:
                state = "2";
                break;
        default:
            break;
        }
        return state;
    }

    @Override
    public void onPresented(com.sessionm.api.SessionM instance) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onPresented()");
        }

        presentedActivityType = getCurrentUnityActivityType();
        if(callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandlePresentedActivityMessage", presentedActivityType);
        }
    }

    public void onDismissed(com.sessionm.api.SessionM instance) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onDismissed()");
        }

        if(callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleDismissedActivityMessage", presentedActivityType);
        }
        presentedActivityType = null;
    }

    @Override
    public void onSessionStateChanged(com.sessionm.api.SessionM instance, SessionM.State state) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onSessionStateChanged(): " + state);
        }

        if(callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleStateTransitionMessage", getCurrentUnitySessionState());
        }
    }
    
    @Override
    public void onSessionFailed(com.sessionm.api.SessionM instance, int error) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onSessionFailed(): " + error);
        }
        
        if(callbackGameObjectName != null) {
            String codeString =  String.valueOf(error);
            String errorMessage = "Session error";
            //Packed string format used on the unity side here.
            String message = String.format(Locale.US, "%d:%s%d:%s", codeString.length(), codeString, errorMessage.length(), errorMessage);
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleSessionFailedMessage", message);
        }
    }

    @Override
    public void onUserUpdated(com.sessionm.api.SessionM instance, User user) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onUserUpdated(): " + user);
        }
        
        if(callbackGameObjectName != null) {
            String jsonData = user.toJSON();
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUserInfoChangedMessage", jsonData);
        }
    }

    @Override
    public void onUserAction(com.sessionm.api.SessionM instance, UserAction action, Map<String, String> data) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onUserAction(): " + action.getCode() + ", data: " + data);
        }

        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("userAction", action.getCode());
            if(data != null) {
            JSONObject dataObject = new JSONObject();
            for(Map.Entry<String, String> entry : data.entrySet()) {
                dataObject.put(entry.getKey(), entry.getValue());
            }
            jsonObject.put("data", dataObject);
        }
        } catch (JSONException e) {
            if(Log.isLoggable(TAG, Log.DEBUG)) {
                Log.d(TAG, "JSONException: " + e);
            }
        }
        if(callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUserActionMessage", jsonObject.toString());
        }
    }

    @Override
    public FrameLayout viewGroupForActivity(com.sessionm.api.SessionM instance) {
        return null;
    }
    
    public static String getAchievementJSON(AchievementData achievement) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("name", achievement.getName());
            jsonObject.put("message", achievement.getMessage());
            jsonObject.put("mpointValue", achievement.getMpointValue());
            jsonObject.put("identifier", achievement.getAchievementId());
            jsonObject.put("isCustom", achievement.isCustom());
        } catch (JSONException e) {
            if(Log.isLoggable(TAG, Log.DEBUG)) {
                Log.d(TAG, "JSONException: " + e);
            }
        }
        return jsonObject.toString();
    }

    @Override
    public boolean shouldPresentAchievement(com.sessionm.api.SessionM instance, AchievementData data) {
        if(Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".shouldAutopresentActivity()");
        }
        
        if(callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUpdatedUnclaimedAchievementMessage", getAchievementJSON(data)); // applies to achievement only
        }
        return false;
    }
}
