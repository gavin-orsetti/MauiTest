namespace MauiTest;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new ContentPage
        {
            Content = new Label
            {
                Text = "Hello from MAUI on .NET 9!",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 24
            }
        });
    }
}
