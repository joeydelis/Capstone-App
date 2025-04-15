using System.Diagnostics;
using System.Security.AccessControl;
using MauiApp1.Classes;
using MauiApp1.Classes.Models;
namespace MauiApp1.Pages
{
    public partial class MainPage : ContentPage
    {
        private System.Timers.Timer _timer;
        int count = 0;
        private readonly BluetoothManager bluetoothManager;
        private readonly Firebase firebase;
        private MainViewModel viewModel;

        public MainPage(Firebase firebaseParam)
        {
            InitializeComponent();
            bluetoothManager = BluetoothManager.Instance;
            firebase = firebaseParam;
            viewModel = new MainViewModel(firebaseParam);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Reloads viewmodel so that login/logout button can be updated to correct text.
            viewModel.Initialize();


            if (Globals.DeviceStrength != -1)
            {
                //StrengthPointer.Value = Globals.DeviceStrength;
                //StrengthValue.Text = Globals.DeviceStrength.ToString();
            }
            if (Globals.DeviceMinutes != -1 && Globals.DeviceSeconds != -1)
            {
                TimeLabel.Text = Globals.DeviceMinutes.ToString("D2") + ":" + Globals.DeviceSeconds.ToString("D2");
            }
            if (Globals.DeviceStrength >= 10)
            {
                //StrengthValue.FontSize = 11;
            }
            else
            {
                //StrengthValue.FontSize = 13;
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

        private async void OnEnableDeviceClicked(object sender, EventArgs e)
        {
            if (bluetoothManager.ConnectedDevice == null)
            {
                await DisplayAlert("No Device Connected", "Please connect a device first.", "OK");
                return;
            }
            else
            {
                var service = await bluetoothManager.ConnectedDevice.Device.GetServiceAsync(Guid.Parse("12345678-1234-5678-1234-56789abcdef0"));
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("abcd1234-5678-1234-5678-abcdef123456"));
                await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes("ON_0"));
                await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes($"BRIGHTNESS_0_{Globals.DeviceStrength * 10}"));
                var received = await characteristic.ReadAsync();
                await DisplayAlert("Message Received", $"{System.Text.Encoding.UTF8.GetString(received.data)}", "OK");
            }

        }

        private async void OnPresetsClicked(object sender, EventArgs e)
        {
            bool isSignedIn = await firebase.IsUserSignedInAsync();
            if (!isSignedIn)
            {
                await DisplayAlert("Unable to open presets page", "User not signed in", "OK");
            }
            else
            {
                await Shell.Current.GoToAsync("/PresetsPage");
            }
        }
    }

}
