using System.Security.AccessControl;
using MauiApp1.Classes;
namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private System.Timers.Timer _timer;
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Globals.DeviceStrength != -1)
            {
                StrengthPointer.Value = Globals.DeviceStrength;
                StrengthValue.Text = Globals.DeviceStrength.ToString();
            }
            if (Globals.DeviceMinutes != -1 && Globals.DeviceSeconds != -1)
            {
                TimeLabel.Text = Globals.DeviceMinutes + " hr " + Globals.DeviceSeconds + " min";
            }
            if (Globals.DeviceStrength >= 10)
            {
                StrengthValue.FontSize = 11;
            }
            else
            {
                StrengthValue.FontSize = 13;
            }
        }
        private async void OnBluetoothPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/BluetoothConnectionPage");
        }

        private async void OnStrengthPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/StrengthPage");
        }
        private async void OnTimerPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/TimerPage");
        }
    }

}
