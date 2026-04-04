using Android.App;
using Android.Content;
using Android.Runtime;
using System;

namespace MauiTest;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            SaveCrash(args.ExceptionObject as Exception);

        AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            SaveCrash(args.Exception);
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
            SaveCrash(ex);
            throw;
        }
    }

    private static void SaveCrash(Exception? ex)
    {
        try
        {
            var ctx = Android.App.Application.Context;
            var prefs = ctx.GetSharedPreferences("crash", FileCreationMode.Private);
            var edit = prefs!.Edit();
            edit!.PutString("crash_info",
                $"Type: {ex?.GetType()?.FullName}\n" +
                $"Message: {ex?.Message}\n\n" +
                $"Inner: {ex?.InnerException?.GetType()?.FullName}\n" +
                $"Inner Message: {ex?.InnerException?.Message}\n\n" +
                $"InnerInner: {ex?.InnerException?.InnerException?.Message}\n\n" +
                $"Stack:\n{ex?.StackTrace}\n\n" +
                $"Inner Stack:\n{ex?.InnerException?.StackTrace}");
            edit.Apply();
        }
        catch { }
    }
}
