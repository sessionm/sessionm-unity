package com.sessionm.unity;

import android.util.Log;
import android.widget.FrameLayout;

import com.sessionm.api.AchievementData;
import com.sessionm.api.ActivityListener;
import com.sessionm.api.SessionListener;
import com.sessionm.api.SessionM;
import com.sessionm.api.SessionM.ActivityType;
import com.sessionm.api.User;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Map;

public class SessionMListener implements ActivityListener, SessionListener {

    private final static String TAG = "SessionM.Unity";
    private static final SessionM sessionM = SessionM.getInstance();
    private String callbackGameObjectName;
    private static SessionMListener instance;
    private String presentedActivityType;

    public synchronized static SessionMListener getInstance() {
        if (instance == null) {
            instance = new SessionMListener();
            sessionM.setActivityListener(instance);
            sessionM.setSessionListener(instance);
        }
        return instance;
    }

    public final void setCallbackGameObjectName(String name) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, "Callback game object name: " + name);
        }

        callbackGameObjectName = name;
    }

    private String getCurrentUnityActivityType() {
        String type = "0";
        ActivityType activityType = sessionM.getCurrentActivity().getActivityType();
        if (activityType != null) {
            switch (activityType) {
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
        switch (sessionM.getSessionState()) {
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
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onPresented()");
        }

        presentedActivityType = getCurrentUnityActivityType();
        if (callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandlePresentedActivityMessage", presentedActivityType);
        }
    }

    public void onDismissed(com.sessionm.api.SessionM instance) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onDismissed()");
        }

        if (callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleDismissedActivityMessage", presentedActivityType);
        }
        presentedActivityType = null;
    }

    @Override
    public void onSessionStateChanged(com.sessionm.api.SessionM instance, SessionM.State state) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onSessionStateChanged(): " + state);
        }

        if (callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleStateTransitionMessage", getCurrentUnitySessionState());
        }
    }

    @Override
    public void onSessionFailed(com.sessionm.api.SessionM instance, int error) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onSessionFailed(): " + error);
        }

        if (callbackGameObjectName != null) {
            String codeString = String.valueOf(error);
            String errorMessage = "Session error";
            //Packed string format used on the unity side here.
            String message = String.format(Locale.US, "%d:%s%d:%s", codeString.length(), codeString, errorMessage.length(), errorMessage);
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleSessionFailedMessage", message);
        }
    }

    @Override
    public void onUserUpdated(com.sessionm.api.SessionM instance, User user) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onUserUpdated(): " + user);
        }

        if (callbackGameObjectName != null) {
            String jsonData = user.toJSON();
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUserInfoChangedMessage", jsonData);
        }
    }

    @Override
    public void onUnclaimedAchievement(SessionM sessionM, AchievementData achievementData) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onUnclaimedAchievement: " + achievementData);
        }

        if (callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUnclaimedAchievementMessage", achievementData.toString());
        }
    }

    @Override
    public void onUserAction(com.sessionm.api.SessionM instance, UserAction action, Map<String, String> data) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".onUserAction(): " + action.getCode() + ", data: " + data);
        }

        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("userAction", action.getCode());
            if (data != null) {
                JSONObject dataObject = new JSONObject();
                for (Map.Entry<String, String> entry : data.entrySet()) {
                    dataObject.put(entry.getKey(), entry.getValue());
                }
                jsonObject.put("data", dataObject);
            }
        } catch (JSONException e) {
            if (Log.isLoggable(TAG, Log.DEBUG)) {
                Log.d(TAG, "JSONException: " + e);
            }
        }
        if (callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUserActionMessage", jsonObject.toString());
        }
    }

    @Override
    public FrameLayout viewGroupForActivity(com.sessionm.api.SessionM instance) {
        return null;
    }

    public static String getAchievementJSON(AchievementData achievement) {
        JSONObject jsonObject = new JSONObject();
        SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());
        Date earnedDate;
        Date startDate;
        long time = 0;
        try {
            earnedDate = formatter.parse(achievement.lastEarnedDate());
            startDate = formatter.parse("0001-01-01");
            time = (earnedDate.getTime() - startDate.getTime()) * 1000000;
        } catch (ParseException e) {
            e.printStackTrace();
        }
        try {
            jsonObject.put("name", (achievement.getName() == null) ? "" : achievement.getName());
            jsonObject.put("message", (achievement.getMessage() == null) ? "" : achievement.getMessage());
            jsonObject.put("mpointValue", achievement.getMpointValue());
            jsonObject.put("identifier", (achievement.getAchievementId() == null) ? "" : achievement.getAchievementId());
            jsonObject.put("isCustom", achievement.isCustom());
            jsonObject.put("importID", (achievement.getImportId() == null) ? "" : achievement.getImportId());
            jsonObject.put("instructions", (achievement.getInstructions() == null) ? "" : achievement.getInstructions());
            jsonObject.put("achievementIconURL", (achievement.getAchievementIconURL() == null) ? "" : achievement.getAchievementIconURL());
            jsonObject.put("action", (achievement.getAction() == null) ? "" : achievement.getAction());
            jsonObject.put("limitText", (achievement.getLimitTimes() == null) ? "" : achievement.getLimitTimes());
            jsonObject.put("timesEarned", achievement.getTimesEarned());
            jsonObject.put("unclaimedCount", achievement.getUnclaimedCount());
            jsonObject.put("distance", achievement.getDistance());
            jsonObject.put("lastEarnedDate", time);
        } catch (JSONException e) {
            if (Log.isLoggable(TAG, Log.DEBUG)) {
                Log.d(TAG, "JSONException: " + e);
            }
        }
        return jsonObject.toString();
    }

    public static String getUser(User user) {
        JSONObject jsonObject = new JSONObject();
        String userAchievementsJSONString = "";
        String userAchievementsListJSONString = "";
        int size = user.getAchievements().size();
        int listSize = user.getAchievementsList().size();
        for (int i = 0; i < size; i++) {
            userAchievementsJSONString += getAchievementJSON(user.getAchievements().get(i));
            if (i < size - 1)
                userAchievementsJSONString += "__";
        }
        for (int i = 0; i < listSize; i++) {
            userAchievementsListJSONString += getAchievementJSON(user.getAchievementsList().get(i));
            if (i < listSize - 1)
                userAchievementsListJSONString += "__";
        }
        try {
            jsonObject.put("isOptedOut", user.isOptedOut());
            jsonObject.put("isRegistered", user.isRegistered());
            jsonObject.put("isLoggedIn", user.isLoggedIn());
            jsonObject.put("getPointBalance", user.getPointBalance());
            jsonObject.put("getUnclaimedAchievementCount", user.getUnclaimedAchievementCount());
            jsonObject.put("getUnclaimedAchievementValue", user.getUnclaimedAchievementValue());
            jsonObject.put("getAchievementsJSON", userAchievementsJSONString);
            jsonObject.put("getAchievementsListJSON", userAchievementsListJSONString);
        } catch (JSONException e) {
            if (Log.isLoggable(TAG, Log.DEBUG)) {
                Log.d(TAG, "JSONException: " + e);
            }
        }
        return jsonObject.toString();
    }

    @Override
    public boolean shouldPresentAchievement(com.sessionm.api.SessionM instance, AchievementData data) {
        if (Log.isLoggable(TAG, Log.DEBUG)) {
            Log.d(TAG, this + ".shouldAutopresentActivity()");
        }

        if (callbackGameObjectName != null) {
            UnityPlayer.UnitySendMessage(callbackGameObjectName, "_sessionM_HandleUpdatedUnclaimedAchievementMessage", getAchievementJSON(data)); // applies to achievement only
        }
        return false;
    }
}
