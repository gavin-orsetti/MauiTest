using Android.Content;

namespace MauiTest;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Read crash from previous run
        var prefs = global::Android.App.Application.Context
            .GetSharedPreferences("crash", FileCreationMode.Private);
        var crashInfo = prefs?.GetString("crash_info", null);

        string displayText;
        if (!string.IsNullOrEmpty(crashInfo))
        {
            displayText = "CRASH FROM PREVIOUS RUN:\n\n" + crashInfo;
            // Clear it
            prefs?.Edit()?.Remove("crash_info")?.Apply();
        }
        else
        {
            displayText = "App launched successfully!\n\nNo crash detected.";
        }

        return new Window(new ContentPage
        {
            Content = new ScrollView
            {
                Content = new Label
                {
                    Text = displayText,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    LineBreakMode = LineBreakMode.WordWrap,
                    Margin = new Thickness(16)
                }
            }
        });
    }
}
