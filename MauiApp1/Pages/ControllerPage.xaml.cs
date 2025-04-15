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
                service = await bluetoothManager.ConnectedDevice.Device.GetServiceAsync(Guid.Parse("c93e8091-1b04-4258-8ac2-2588e890e121"));
                characteristic = await service.GetCharacteristicAsync(Guid.Parse("e7467b73-034c-4f44-8afc-4cac0be2db0b"));
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
}
