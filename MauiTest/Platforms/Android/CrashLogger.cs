using Android.App;
using Android.OS;
using Android.Runtime;
using System;
using System.IO;

namespace MauiTest.Platforms.Android;

public static class CrashLogger
{
    private static Activity? _activity;

    public static void Register(Activity activity)
    {
        _activity = activity;

        AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            args.Handled = false; // let Android see it too
            ShowCrash(args.Exception);
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            ShowCrash(args.ExceptionObject as Exception);
        };
    }

    private static void ShowCrash(Exception? ex)
    {
        try
        {
            // Write to file first (doesn't need MAUI)
            var path = Path.Combine(
                global::Android.OS.Environment.GetExternalStoragePublicDirectory(
                    global::Android.OS.Environment.DirectoryDocuments)!.AbsolutePath,
                "maui-crash.txt");
            File.WriteAllText(path, $"{ex?.GetType()}\n{ex?.Message}\n\nInner: {ex?.InnerException?.Message}\n\n{ex?.StackTrace}");
        }
        catch { }

        try
        {
            // Show dialog on UI thread
            _activity?.RunOnUiThread(() =>
            {
                var msg = $"{ex?.GetType()?.Name}\n\n{ex?.Message}\n\nInner: {ex?.InnerException?.Message}\n\n{ex?.StackTrace?[..Math.Min(ex.StackTrace?.Length ?? 0, 1000)]}";
                new AlertDialog.Builder(_activity)
                    .SetTitle("Crash Details")!
                    .SetMessage(msg)!
                    .SetPositiveButton("OK", (s, e) => { })!
                    .SetCancelable(false)!
                    .Show();
            });
        }
        catch { }
    }
}
