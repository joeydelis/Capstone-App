using MauiApp1.Classes;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Syncfusion.Maui.Sliders;

namespace MauiApp1.Pages;

public partial class StrengthPage : ContentPage
{
    private readonly BluetoothManager bluetoothManager;
    private IService service;
    private ICharacteristic characteristic;

	public StrengthPage()
	{
		InitializeComponent();
        bluetoothManager = BluetoothManager.Instance;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (bluetoothManager.ConnectedDevice?.Device != null)
        {
            bluetoothManager.DeviceDisconnected += OnDeviceDisconnected;

            try
            {
                service = await bluetoothManager.ConnectedDevice.Device.GetServiceAsync(Guid.Parse("12345678-1234-5678-1234-56789abcdef0"));
                characteristic = await service.GetCharacteristicAsync(Guid.Parse("abcd1234-5678-1234-5678-abcdef123456"));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load service or characteristic: {ex.Message}", "OK");
            }
        }
    }

    private async void OnDeviceDisconnected(object sender, IDevice device)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await DisplayAlert("Disconnected", "Bluetooth device has been disconnected.", "OK");

            await Shell.Current.Navigation.PopToRootAsync();
        });
    }
    private async void OnUpClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Up", "", "OK");
    }

    private async void OnDownClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Down", "", "OK");
    }

    private async void OnCustomClicked(object sender, EventArgs e)
    {
         string input = await Application.Current.MainPage.DisplayPromptAsync("New Preset", "Enter command", "OK", "Cancel", "Ex: ON_0");
         await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes(input));
         var received = await characteristic.ReadAsync();
         await DisplayAlert("Message Received", $"{System.Text.Encoding.UTF8.GetString(received.data)}", "OK");
    }
}