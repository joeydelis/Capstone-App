using System.Diagnostics;
using System.Security.AccessControl;
using MauiApp1.Classes;
using MauiApp1.Classes.Models;
using Plugin.BLE.Abstractions.Contracts;
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
                IService service = await bluetoothManager.ConnectedDevice.Device.GetServiceAsync(Guid.Parse("c93e8091-1b04-4258-8ac2-2588e890e121"));
                ICharacteristic characteristic = await service.GetCharacteristicAsync(Guid.Parse("e7467b73-034c-4f44-8afc-4cac0be2db0b"));

                int time = (((Globals.DeviceMinutes * 60) + Globals.DeviceSeconds) * 60);

                await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes($"Timer_{time}"));
                var received = await characteristic.ReadAsync();

                await Shell.Current.GoToAsync("/ControllerPage");
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
