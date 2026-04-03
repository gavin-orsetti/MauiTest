namespace MauiTest;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new ContentPage
        {
            Content = new Label
            {
                Text = "Hello from MAUI!",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };
    }
}
