using MauiApp1.Classes;
using Syncfusion.Maui.Sliders;

namespace MauiApp1.Pages;

public partial class StrengthPage : ContentPage
{
	readonly double sliderIncrement = 1;
    int _internalStrength = -1;
    int _internalPosition = -1;
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
            StrengthDisplay.Text = Globals.DeviceStrength.ToString();
        }
        if (Globals.DevicePosition != -1)
        {
            _internalPosition = Globals.DevicePosition;
            PositionDisplay.Text = Globals.DevicePosition.ToString();
        }
    }

    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        SfSlider slider = (SfSlider)sender;
        double value = Math.Round(e.NewValue / sliderIncrement) * sliderIncrement;
        slider.Value = value;
        _internalStrength = (int)value;
    }
    private void OnIncreaseClicked1(object sender, EventArgs e)
    {
        if (_internalStrength < 10)
        {
            _internalStrength++;
            StrengthDisplay.Text = _internalStrength.ToString();
        }
    }
    private void OnDecreaseClicked1(object sender, EventArgs e)
    {
        if (_internalStrength > 0)
        {
            _internalStrength--;
            StrengthDisplay.Text = _internalStrength.ToString();
        }
    }
    private void OnIncreaseClicked2(object sender, EventArgs e)
    {
        if (_internalPosition < 10)
        {
            _internalPosition++;
            PositionDisplay.Text = _internalPosition.ToString();
        }
    }
    private void OnDecreaseClicked2(object sender, EventArgs e)
    {
        if (_internalPosition > 0)
        {
            _internalPosition--;
            PositionDisplay.Text = _internalPosition.ToString();
        }
    }
    private void OnStrengthButtonPressed(object sender, EventArgs e)
    {
        if (Globals.DeviceStrength != _internalStrength || Globals.DevicePosition != _internalPosition)
        {
            Globals.DeviceStrength = _internalStrength;
            Globals.DevicePosition = _internalPosition;
            DisplayAlert("Strength Set", $"Strength level set at {_internalStrength}, position set at {_internalPosition}", "OK");
        }
    }

    private void Button_Pressed(object sender, EventArgs e)
    {

    }
}