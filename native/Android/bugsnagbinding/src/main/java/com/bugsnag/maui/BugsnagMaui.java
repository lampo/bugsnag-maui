package com.bugsnag.maui;

import static com.bugsnag.maui.JsonHelper.unwrap;

import android.content.Context;
import android.util.Log;

import androidx.annotation.Nullable;

import com.bugsnag.android.Bugsnag;
import com.bugsnag.android.BugsnagExitInfoPlugin;
import com.bugsnag.android.Configuration;
import com.bugsnag.android.Event;
import com.bugsnag.android.MauiInternalHooks;

import org.json.JSONException;
import org.json.JSONObject;

public class BugsnagMaui {
    private MauiInternalHooks client;

    private static boolean isAttached = false;

    private static boolean isAnyStarted = false;
    private boolean isStarted = false;

    static final int HEX_LONG_LENGTH = 16;

    /*
     ***********************************************************************************************
     * All methods listed here must also be registered in the BugsnagFlutterPlugin otherwise they
     * won't be callable from the Maui layer.
     ***********************************************************************************************
     */


    public void start(Context context, @Nullable Configuration configuration) throws Exception {
        if (isStarted) {
            Log.w("BugsnagMaui", "bugsnag.start() was called more than once. Ignoring.");
            return;
        }

        if (isAnyStarted) {
            Log.w("BugsnagMaui", "bugsnag.start() was called from a previous Maui context. Reusing existing client. Config not applied.");
            client = new MauiInternalHooks(MauiInternalHooks.getClient());
            return;
        }

        if (MauiInternalHooks.getClient() != null) {
            throw new IllegalStateException("bugsnag.start() may not be called after starting Bugsnag natively");
        }

        if (configuration == null) {
            configuration = Configuration.load(context);
        }

        // automatically add the Exit Info plugin on Android 11+ if not already added
        if (android.os.Build.VERSION.SDK_INT >= 30) {
            BugsnagExitInfoPlugin bugsnagExitInfoPlugin = new BugsnagExitInfoPlugin();
            configuration.addPlugin(bugsnagExitInfoPlugin);
        }

        client = new MauiInternalHooks(Bugsnag.start(context, configuration));
        isAnyStarted = true;
        isStarted = true;
    }

    @SuppressWarnings("unchecked")
    public JSONObject createEvent(JSONObject error, boolean unhandled, boolean deliver) throws JSONException {
        if (client == null) {
            return null;
        }

        // early exit if we are going to discard this Error, but *only* if we would also deliver
        // immediately - otherwise the Maui layer could modify it and avoid discard
        if (deliver && client.shouldDiscardError(error)) {
            return null;
        }

        Event event = client.createEvent(
                client.createSeverityReason(
                        unhandled ? "unhandledException" : "handledException"
                )
        );

        event.getErrors().add(client.unmapError(unwrap(error)));

//        Object mauiMetadata = unwrap(args.optJSONObject("mauiMetadata"));
//        if (mauiMetadata instanceof Map) {
//            event.addMetadata("maui", (Map<String, Object>) mauiMetadata);
//        }

//        JSONObject correlation = args.optJSONObject("correlation");
//        if (correlation != null) {
//            try {
//                String traceId = getString(correlation, "traceId");
//                String spanId = getString(correlation, "spanId");
//                if (traceId != null &&
//                        traceId.length() == HEX_LONG_LENGTH * 2 &&
//                        spanId != null &&
//                        spanId.length() == HEX_LONG_LENGTH
//                ) {
//                    long traceIdMostSignificantBits = hexToLong(traceId.substring(0, HEX_LONG_LENGTH));
//                    long traceIdLeastSignificantBits = hexToLong(traceId.substring(HEX_LONG_LENGTH));
//                    long spanIdAsLong = hexToLong(spanId);
//                    event.setTraceCorrelation(new UUID(traceIdMostSignificantBits, traceIdLeastSignificantBits), spanIdAsLong);
//                }
//            } catch (Exception e) {
//                // ignore the error, the error correlation will be missing
//            }
//        }

        if (deliver) {
            // maui layer has asked us to deliver the Event immediately
            client.deliverEvent(event);
            return null;
        } else {
            return client.mapEvent(event);
        }
    }

    public JSONObject deliverEvent(@Nullable JSONObject eventJson) throws JSONException {
        if (eventJson == null || client == null) {
            return null;
        }

        if (client.shouldDiscardEvent(eventJson)) {
            return null;
        }

        Event event = client.unmapEvent(unwrap(eventJson));
        client.deliverEvent(event);
        return null;
    }

    @Nullable
    String getString(JSONObject args, String key) {
        Object value = args.opt(key);
        return value instanceof String ? (String) value : null;
    }

    @Nullable
    String getString(JSONObject args, String key, @Nullable String fallback) {
        String value = getString(args, key);
        return value != null ? value : fallback;
    }

    boolean hasString(JSONObject args, String key) {
        return getString(args, key) != null;
    }

    private static long hexToLong(String hex) {
        return Long.parseLong(hex.substring(0, 2), 16) << 56 |
                Long.parseLong(hex.substring(2), 16);
    }
}