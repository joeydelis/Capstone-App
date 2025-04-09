using System.Xml.Linq;
using MauiApp1.Classes;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Syncfusion.Maui.Sliders;

namespace MauiApp1.Pages;

public partial class ControllerPage : ContentPage
{
    private readonly BluetoothManager bluetoothManager;
    private IService service;
    private ICharacteristic characteristic;
    private bool isCooldown = false;
    private readonly TimeSpan CommandCooldown = TimeSpan.FromMilliseconds(250);

    public ControllerPage()
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

    private async Task ExecuteWithCooldown(Func<Task> action)
    {
        if (isCooldown) {
            return;
        }

        isCooldown = true;

        try
        {
            await action();
        }
        finally
        {
            await Task.Delay(CommandCooldown);
            isCooldown = false;
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
        _ = ExecuteWithCooldown(async () =>
        {
            await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes("UP"));
            var received = await characteristic.ReadAsync();
            await DisplayAlert("Message Received", $"{System.Text.Encoding.UTF8.GetString(received.data)}", "OK");
        });
    }

    private async void OnDownClicked(object sender, EventArgs e)
    {
        _ = ExecuteWithCooldown(async () =>
        {
            await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes("DOWN"));
            var received = await characteristic.ReadAsync();
            await DisplayAlert("Message Received", $"{System.Text.Encoding.UTF8.GetString(received.data)}", "OK");
        });
    }

    private async void OnCustomClicked(object sender, EventArgs e)
    {
        string input = await Application.Current.MainPage.DisplayPromptAsync("New Preset", "Enter command", "OK", "Cancel", "Ex: ON_0");
        if (string.IsNullOrWhiteSpace(input))
            return;
        await characteristic.WriteAsync(System.Text.Encoding.UTF8.GetBytes(input));
        var received = await characteristic.ReadAsync();
        await DisplayAlert("Message Received", $"{System.Text.Encoding.UTF8.GetString(received.data)}", "OK");
    }
}
