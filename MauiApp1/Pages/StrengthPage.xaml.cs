using MauiApp1.Classes;
using Syncfusion.Maui.Sliders;

namespace MauiApp1.Pages;

public partial class StrengthPage : ContentPage
{
	readonly double sliderIncrement = 1;
    int _internalStrength = -1;
	public StrengthPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Globals.DeviceStrength != -1)
        {
            _internalStrength = Globals.DeviceStrength;
            StrengthSlider.Value = Globals.DeviceStrength;
        }
    }

    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        SfSlider slider = (SfSlider)sender;
        double value = Math.Round(e.NewValue / sliderIncrement) * sliderIncrement;
        slider.Value = value;
        _internalStrength = (int)value;
    }
    private void OnStrengthButtonPressed(object sender, EventArgs e)
    {
        if (Globals.DeviceStrength != _internalStrength)
        {
            Globals.DeviceStrength = _internalStrength;
            DisplayAlert("Strength Set", $"Strength level set at {_internalStrength}", "OK");
        }
    }

    private void Button_Pressed(object sender, EventArgs e)
    {

    }
}