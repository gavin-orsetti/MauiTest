using System.IO;

namespace MauiTest;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Check if a crash file exists from a previous run
        var crashPath = Path.Combine(FileSystem.CacheDirectory, "crash.txt");
        string pageText = "Hello from MAUI!\n\nNo previous crash detected.";

        if (File.Exists(crashPath))
        {
            pageText = "CRASH FROM PREVIOUS RUN:\n\n" + File.ReadAllText(crashPath);
            File.Delete(crashPath); // clear after reading
        }

        var label = new Label
        {
            Text = pageText,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            LineBreakMode = LineBreakMode.WordWrap,
            Margin = new Thickness(16)
        };

        return new Window(new ContentPage
        {
            Content = new ScrollView { Content = label }
        });
    }
}
