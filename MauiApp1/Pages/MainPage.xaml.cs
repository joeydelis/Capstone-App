using System.Diagnostics;
using System.Security.AccessControl;
using MauiApp1.Classes;
namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private System.Timers.Timer _timer;
        int count = 0;
        private readonly BluetoothManager bluetoothManager;
        private readonly Firebase firebase;

        public MainPage()
        {
            InitializeComponent();
            bluetoothManager = BluetoothManager.Instance;
            firebase = Firebase.Instance;
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

        private async void OnEnableDeviceClicked(object sender, EventArgs e)
        {
            if (bluetoothManager.ConnectedDevice == null)
            {
                await DisplayAlert("No Device Connected", "Please connect a device first.", "OK");
                return;
            } else
            {
                var service = await bluetoothManager.ConnectedDevice.Device.GetServiceAsync(Guid.Parse("12345678-1234-5678-1234-56789abcdef0"));
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("abcd1234-5678-1234-5678-abcdef123456"));
                await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes("ON_0"));
                await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes($"BRIGHTNESS_0_{Globals.DeviceStrength * 10}"));
                var received = await characteristic.ReadAsync();
                await DisplayAlert("Message Received", $"{System.Text.Encoding.UTF8.GetString(received.data)}", "OK");
            }
            
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            bool isSignedIn = await firebase.IsUserSignedInAsync();
            if (isSignedIn)
            {
                await DisplayAlert("Unable to sign in", "User already signed in", "OK");
            } else
            {
                await Shell.Current.GoToAsync("/FirebaseLoginPage");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool isSignedIn = await firebase.IsUserSignedInAsync();
            if (isSignedIn) 
            {
                Firebase.Logout();
            } else
            {
                await DisplayAlert("Unable to logout", "No user signed in", "OK");
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            bool dataAdded = await firebase.AddUserSetting();

            if (dataAdded)
            {
                await DisplayAlert("Success", "User setting added", "OK");
            }
            else
            {
                await DisplayAlert("Failure", "User not added", "OK");
            }
        }

        private async void OnTestClicked(object sender, EventArgs e)
        {
            bool isSignedIn = await firebase.IsUserSignedInAsync();
            if (isSignedIn)
            {
                var presets = await Firebase.Instance.GetUserPresetsAsync();

                if (presets != null)
                {
                    await DisplayAlert("Success", $"Retrieved {presets.Count} presets!", "OK");
                    foreach (var preset in presets)
                    {
                        Console.WriteLine(preset.Time);
                    }
                }
                else
                {
                    await DisplayAlert("Unable get presets", "Error retrieving presets", "OK");
                }


            }
            else
            {
                await DisplayAlert("Unable get presets", "No user signed in", "OK");
            }
        }
    }

}
