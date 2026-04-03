using Android.App;
using Android.Runtime;
using System;
using System.IO;

namespace MauiTest;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        // Earliest possible hook — before any Activity or MAUI code runs
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            WriteCrashFile(ex);
        };

        AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            WriteCrashFile(args.Exception);
            args.Handled = false;
        };
    }

    protected override MauiApp CreateMauiApp()
    {
        try
        {
            return MauiProgram.CreateMauiApp();
        }
        catch (Exception ex)
        {
            WriteCrashFile(ex);
            throw;
        }
    }

    private static void WriteCrashFile(Exception? ex)
    {
        try
        {
            // Write to app-private cache — no permissions needed
            var path = Path.Combine(
                global::Android.App.Application.Context.CacheDir!.AbsolutePath,
                "crash.txt");
            var text = $"Type: {ex?.GetType()?.FullName}\nMessage: {ex?.Message}\nInner: {ex?.InnerException?.GetType()?.FullName}: {ex?.InnerException?.Message}\nInnerInner: {ex?.InnerException?.InnerException?.Message}\n\nStack:\n{ex?.StackTrace}\n\nInner Stack:\n{ex?.InnerException?.StackTrace}";
            File.WriteAllText(path, text);
        }
        catch { }
    }
}
