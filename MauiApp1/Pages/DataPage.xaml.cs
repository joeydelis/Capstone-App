using MauiApp1.Classes;

namespace MauiApp1.Pages;

public partial class DataPage : ContentPage
{
	public DataPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        strengthText.Text = $"Last used strength level: {Globals.DeviceStrength}";
        positionText.Text = $"Last used position: {Globals.DevicePosition}";
        timeText.Text = $"Last used time: {Globals.DeviceMinutes} minutes, {Globals.DeviceSeconds} seconds";
    }
}