using Bugsnag;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnConnectClicked(object sender, EventArgs e)
    {
        var connectService = App.Current.MainPage.Handler.MauiContext.Services.GetRequiredService<IMasterCardConnect>();
        var result = await connectService.LaunchConnectAsync(ConnectUrl.Text);
        resultLabel.Text = $"Result: {result.Status}";
    }
}