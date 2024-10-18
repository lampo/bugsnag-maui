package com.bugsnag.maui;

import androidx.annotation.Nullable;

import com.bugsnag.android.BreadcrumbType;
import com.bugsnag.android.Telemetry;

import org.json.JSONArray;

import java.util.Collections;
import java.util.EnumSet;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

/**
 * Utility class containing mappings between Dart <-> Android enums
 */
class EnumHelper {
    private static final Map<String, BreadcrumbType> mauiBreadcrumbTypes = new HashMap<>();
    private static final Map<String, Telemetry> mauiTelemetry = new HashMap<>();

    static {
        mauiBreadcrumbTypes.put("navigation", BreadcrumbType.NAVIGATION);
        mauiBreadcrumbTypes.put("request", BreadcrumbType.REQUEST);
        mauiBreadcrumbTypes.put("process", BreadcrumbType.PROCESS);
        mauiBreadcrumbTypes.put("log", BreadcrumbType.LOG);
        mauiBreadcrumbTypes.put("user", BreadcrumbType.USER);
        mauiBreadcrumbTypes.put("state", BreadcrumbType.STATE);
        mauiBreadcrumbTypes.put("error", BreadcrumbType.ERROR);
        mauiBreadcrumbTypes.put("manual", BreadcrumbType.MANUAL);

        mauiTelemetry.put("internalErrors", Telemetry.INTERNAL_ERRORS);
        mauiTelemetry.put("usage", Telemetry.USAGE);
    }

    private EnumHelper() {
    }

    static Set<BreadcrumbType> unwrapBreadcrumbTypes(@Nullable JSONArray breadcrumbTypes) {
        if (breadcrumbTypes == null) {
            return Collections.emptySet();
        }

        Set<BreadcrumbType> set = EnumSet.noneOf(BreadcrumbType.class);

        int enabledTypeCount = breadcrumbTypes.length();
        for (int index = 0; index < enabledTypeCount; index++) {
            set.add(mauiBreadcrumbTypes.get(breadcrumbTypes.optString(index)));
        }

        return set;
    }

    static Set<Telemetry> unwrapTelemetry(@Nullable JSONArray telemetry) {
        if (telemetry == null) {
            return Collections.emptySet();
        }

        Set<Telemetry> set = EnumSet.noneOf(Telemetry.class);

        int count = telemetry.length();
        for (int i = 0; i < count; i++) {
            set.add(mauiTelemetry.get(telemetry.optString(i)));
        }

        return set;
    }
}
