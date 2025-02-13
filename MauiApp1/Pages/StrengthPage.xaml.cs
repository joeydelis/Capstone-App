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

    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        SfSlider slider = (SfSlider)sender;
        double value = Math.Round(e.NewValue / sliderIncrement) * sliderIncrement;
        slider.Value = value;
    }
}