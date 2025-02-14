using MauiApp1.Classes;
using Syncfusion.Maui.Sliders;

namespace MauiApp1.Pages;

public partial class StrengthPage : ContentPage
{
	readonly double sliderIncrement = 1;
	double sliderCorrectValue;
	public StrengthPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Globals.DeviceStrength != -1)
        {
            StrengthSlider.Value = Globals.DeviceStrength;
        }
    }

    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        SfSlider slider = (SfSlider)sender;
        double value = Math.Round(e.NewValue / sliderIncrement) * sliderIncrement;
        slider.Value = value;
        Globals.DeviceStrength = (int)value;
    }
}