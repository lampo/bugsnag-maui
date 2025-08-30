# AndroidRedirectStdToLogCat – Rationale & Usage

**Goal:** capture Mono/.NET for Android native crash output (e.g., `SIGSEGV`) that is printed to `stdout`/`stderr`, extract the **Managed Stacktrace** section, and persist it so it can be attached to a Bugsnag report on the next launch.

## Why this is necessary
Mono emits crash diagnostics to `stdout`/`stderr`, not through managed exception handling. As a result, the managed stack is frequently **not available** to crash reporters if the app dies before the reporter flushes. Redirecting `stdout`/`stderr` gives us a reliable way to detect the `"Managed Stacktrace:"` block and save it to disk.

## How it works (high level)
1. **Redirect** `stdout` (fd 1) and `stderr` (fd 2) to a pipe using `dup2`.
2. A **background thread** reads from the pipe and mirrors each line to Logcat.
3. When it sees **`Managed Stacktrace:`**, it **accumulates frames** until the block ends.
4. The captured frames are written to **`<files>/crashes/crash_<epochMs>.txt`**.
5. On **next launch**, the app can upload the newest crash file to **Bugsnag**.

## Where to call it
Call `AndroidRedirectStdToLogCat.Start("Bugsnag.Maui")` **early** in app startup (e.g., after MAUI has initialized in `MainActivity.OnCreate`).

## Cleanup policy
To avoid unbounded disk usage, the cleaner:
- Keeps **≤ 10** files
- Keeps **≤ ~512 KB** total
- Deletes files **older than 14 days**
- Skips files **younger than 30s** to avoid deleting a fresh report

## Limitations
- This **does not symbolize native addresses**; upload your **AAB/APK symbol files** and proguard/r8 **mapping** to your crash tooling separately.
- It’s **process-wide**; once started, all `stdout`/`stderr` are redirected.