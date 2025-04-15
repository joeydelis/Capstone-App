using System.Diagnostics;
using MauiApp1.Classes;
using Syncfusion.Maui.Picker;

namespace MauiApp1.Pages;

public partial class TimerPage : ContentPage
{
    private int _internalHour = 1;
    private int _internalMinute = 0;
	public TimerPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        for (int i = 0; i <= 60; i++)
        {
            //MinutePicker.Items.Add(i.ToString());
        }
        if (Globals.DeviceMinutes != -1 && Globals.DeviceSeconds != -1)
        {
            //TimePicker.SelectedTime = new TimeSpan(Globals.DeviceMinutes, Globals.DeviceSeconds, 0);
        }
    }
    private void OnPickerChanged(object sender, TimePickerSelectionChangedEventArgs e)
    {
        TimeSpan time = e.NewValue.Value;
        _internalHour = time.Hours * 60 + time.Minutes;
        _internalMinute = time.Seconds;
    }
    private void OnSetTimerClicked(object sender, EventArgs e)
    {
        if (_internalHour != -1 && _internalMinute != -1)
        {
            Globals.DeviceMinutes = _internalHour;
            Globals.DeviceSeconds = _internalMinute;
            DisplayAlert("Timer Set", $"Timer set for {_internalHour} minute(s) and {_internalMinute} second(s)", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please select both minutes and seconds.", "OK");
        }
    }
}