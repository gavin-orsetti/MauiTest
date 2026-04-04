using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System;
using System.Threading;

namespace MauiTest;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership) { }

    protected override MauiApp CreateMauiApp()
    {
        try
        {
            return MauiProgram.CreateMauiApp();
        }
        catch (Exception ex)
        {
            // Build full message including all inner exceptions
            var msg = BuildMessage(ex);

            // Save to SharedPreferences synchronously
            try
            {
                var prefs = Android.App.Application.Context
                    .GetSharedPreferences("crash", FileCreationMode.Private)!
                    .Edit()!;
                prefs.PutString("crash_info", msg);
                prefs.Commit(); // synchronous — not Apply()
            }
            catch { }

            // Show a blocking native Toast on the main thread
            try
            {
                var handler = new Handler(Looper.MainLooper!);
                handler.Post(() =>
                {
                    Toast.MakeText(Android.App.Application.Context, 
                        "CRASH: " + ex.GetType().Name + ": " + ex.Message, 
                        ToastLength.Long)?.Show();
                });
                Thread.Sleep(4000); // give toast time to show
            }
            catch { }

            throw;
        }
    }

    private static string BuildMessage(Exception? ex)
    {
        var sb = new System.Text.StringBuilder();
        var current = ex;
        var depth = 0;
        while (current != null && depth < 5)
        {
            sb.AppendLine($"[Level {depth}] {current.GetType().FullName}");
            sb.AppendLine($"Message: {current.Message}");
            sb.AppendLine($"Stack: {current.StackTrace}");
            sb.AppendLine();
            current = current.InnerException;
            depth++;
        }
        return sb.ToString();
    }
}
