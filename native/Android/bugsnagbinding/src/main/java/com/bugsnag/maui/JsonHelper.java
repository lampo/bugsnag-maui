package com.bugsnag.maui;


import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.bugsnag.android.BreadcrumbType;
import com.bugsnag.android.FeatureFlag;
import com.bugsnag.android.JsonStream;
import com.bugsnag.android.MetadataAware;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.StringWriter;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

public class JsonHelper {
    private JsonHelper() {
    }

    /**
     * Convenience function to encode a {@code Streamable} as a {@code JSONObject}.
     *
     * @param json the object to encode
     * @return the {@code JSONObject} equivalent of {@code json}
     */
    @Nullable
    public static JSONObject toJson(JsonStream.Streamable json) {
        StringWriter writer = new StringWriter();
        JsonStream stream = new JsonStream(writer);
        try {
            json.toStream(stream);
            return new JSONObject(writer.toString());
        } catch (IOException e) {
            return null;
        } catch (JSONException e) {
            return null;
        }
    }

    @SuppressWarnings("unchecked")
    public static <E, C extends Collection<E>> C unwrap(@Nullable JSONArray array, C outputCollection) throws JSONException {
        if (array != null) {
            int arrayLength = array.length();
            for (int index = 0; index < arrayLength; index++) {
                Object value = array.opt(index);
                if (value instanceof JSONObject) {
                    outputCollection.add((E) unwrap((JSONObject) value));
                } else if (value instanceof JSONArray) {
                    outputCollection.add((E) unwrap((JSONArray) value, outputCollection));
                } else {
                    outputCollection.add((E) value);
                }
            }
        }

        return outputCollection;
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> unwrap(JSONObject object) throws JSONException {
        Map<String, Object> map = new HashMap<>();
        Iterator<String> keys = object.keys();

        while (keys.hasNext()) {
            String key = keys.next();
            Object value = object.get(key);
            if (value instanceof JSONObject) {
                map.put(key, unwrap((JSONObject) value));
            } else if (value instanceof JSONArray) {
                map.put(key, unwrap((JSONArray) value, new ArrayList<>()));
            } else {
                map.put(key, value);
            }
        }

        return map;
    }

    // unpack Metadata with the Configuration.addMetadata public API
    public static void unpackMetadata(JSONObject metadata, MetadataAware target) throws JSONException {
        if (metadata == null || metadata.length() == 0) {
            return;
        }

        Iterator<String> sections = metadata.keys();
        while (sections.hasNext()) {
            String sectionName = sections.next();
            JSONObject section = metadata.optJSONObject(sectionName);

            if (section != null) {
                target.addMetadata(sectionName, unwrap(section));
            }
        }
    }

    @SuppressWarnings("unchecked")
    public static JSONObject wrap(Map<? super String, Object> wrappedJson) throws JSONException {
        JSONObject jsonObject = new JSONObject();
        for (Map.Entry<? super String, Object> entry : wrappedJson.entrySet()) {
            jsonObject.put((String) entry.getKey(), entry.getValue());
        }
        return jsonObject;
    }

    @NonNull
    public static List<FeatureFlag> unpackFeatureFlags(@Nullable JSONArray featureFlags) {
        if (featureFlags == null) {
            return Collections.emptyList();
        }

        List<FeatureFlag> flags = new ArrayList<>(featureFlags.length());
        for (int index = 0; index < featureFlags.length(); index++) {
            JSONObject featureFlag = featureFlags.optJSONObject(index);
            flags.add(new FeatureFlag(
                    featureFlag.optString("featureFlag"),
                    (String) featureFlag.opt("variant")
            ));
        }
        return flags;
    }

    public static BreadcrumbType unpackBreadcrumbType(String type) {
        switch (type) {
            case "error": return BreadcrumbType.ERROR;
            case "log": return BreadcrumbType.LOG;
            case "navigation": return BreadcrumbType.NAVIGATION;
            case "process": return BreadcrumbType.PROCESS;
            case "request": return BreadcrumbType.REQUEST;
            case "state": return BreadcrumbType.STATE;
            case "user": return BreadcrumbType.USER;
            default: return BreadcrumbType.MANUAL;
        }
    }
}
