using System.Diagnostics;
using MauiApp1.Classes;

namespace MauiApp1.Pages;

public partial class TimerPage : ContentPage
{
	public TimerPage()
	{
		InitializeComponent();
        InitializePickers();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Globals.DeviceMinutes != -1 && Globals.DeviceSeconds != -1)
        {
            Debug.WriteLine("Writing new time.");
            SecondsPicker.SelectedItem = Globals.DeviceSeconds.ToString();
            MinutesPicker.SelectedItem = Globals.DeviceMinutes.ToString();
        }
    }
    private void InitializePickers()
    {
        List<string> minutes = new List<string>();
        for (int i = 0; i < 60; i++)
        {
            minutes.Add(i.ToString("D2"));
        }
        MinutesPicker.ItemsSource = minutes;

        List<string> seconds = new List<string>();
        for (int i = 0; i < 60; i++)
        {
            seconds.Add(i.ToString("D2"));
        }
        SecondsPicker.ItemsSource = seconds;
    }

    private void OnSetTimerClicked(object sender, EventArgs e)
    {
        string selectedMinutes = MinutesPicker.SelectedItem?.ToString();
        string selectedSeconds = SecondsPicker.SelectedItem?.ToString();

        if (selectedMinutes != null && selectedSeconds != null)
        {
            Globals.DeviceMinutes = int.Parse(selectedMinutes);
            Globals.DeviceSeconds = int.Parse(selectedSeconds);
            DisplayAlert("Timer Set", $"Timer set for {selectedMinutes} minutes and {selectedSeconds} seconds", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please select both minutes and seconds.", "OK");
        }
    }
}