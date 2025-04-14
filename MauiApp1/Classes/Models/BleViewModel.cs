using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using System.Diagnostics;
using System.ComponentModel;

namespace MauiApp1.Classes.Models
{
    public class BleViewModel : INotifyPropertyChanged
    {
        private readonly BluetoothManager bluetoothManager;
        public ObservableCollection<DeviceViewModel> Devices { get; } = new();

        public Command StartScanCommand { get; }
        public Command<DeviceViewModel> ConnectCommand { get; }

        public BleViewModel()
        {
            bluetoothManager = BluetoothManager.Instance;

            //Used when device is found from StartScanningAsync. Adds discovered device if it meets specified criteria
            bluetoothManager.Adapter.DeviceDiscovered += (s, e) =>
            {
                //Make sure to update name if it is changed in Arduino code
                if (!string.IsNullOrEmpty(e.Device.Name) && (e.Device.Name) == "ESP32_BLE_Server" && !Devices.Any(d => d.Device.Id == e.Device.Id))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Devices.Add(new DeviceViewModel(e.Device));
                    });
                }
            };

            StartScanCommand = new Command(async () => await StartScanningAsync());
            ConnectCommand = new Command<DeviceViewModel>(async (device) => await ToggleConnection(device));
        }

        private async Task ConnectToDevice(DeviceViewModel device)
        {
            if (device == null) return;

            try
            {
                await bluetoothManager.Adapter.ConnectToDeviceAsync(device.Device);

                device.IsConnected = true;
                bluetoothManager.ConnectedDevice = device;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Connection Failed", $"Could not connect to {device.Name}. Error: {ex.Message}", "OK");
            }
        }

        private async Task DisconnectFromDevice(DeviceViewModel device)
        {
            try
            {
                await bluetoothManager.Adapter.DisconnectDeviceAsync(device.Device);
                device.IsConnected = false;
                bluetoothManager.ConnectedDevice = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Disconnection Failed", $"Could not disconnect from {device.Name}. Error: {ex.Message}", "OK");
            }
        }

        //Called when connect/disconnect button is pressed. Decides between disconnect or connect based on if device matches id of connected device.
        private async Task ToggleConnection(DeviceViewModel device)
        {
            if (device == null) return;

            if (bluetoothManager.ConnectedDevice != null && bluetoothManager.ConnectedDevice.Device.Id == device.Device.Id)
            {
                await DisconnectFromDevice(bluetoothManager.ConnectedDevice);
                device.IsConnected = false;
            }
            else
            {
                //First disconnect from connected device to prevent multiple connections
                if (bluetoothManager.ConnectedDevice != null)
                {
                    await DisconnectFromDevice(bluetoothManager.ConnectedDevice);
                }
                await ConnectToDevice(device);
            }

        }
        public async Task StartScanningAsync()
        {
            if (!bluetoothManager.BluetoothLE.IsAvailable)
            {
                Debug.WriteLine("Bluetooth is not available on this device.");
                return;
            }

            if (!bluetoothManager.BluetoothLE.IsOn)
            {
                Debug.WriteLine("Bluetooth is not on.");
                return;
            }

            if (!await HasCorrectPermissions())
            {
                Debug.WriteLine("Does not have correct permissions");
                return;
            }

            Devices.Clear();

            //After clearing list, check to see if device is already connected from previous scans.
            if (bluetoothManager.ConnectedDevice != null)
            {
                var ConnectedDevices = bluetoothManager.Adapter.GetSystemConnectedOrPairedDevices();
                foreach (var device in ConnectedDevices)
                {
                    if (!Devices.Any(d => d.Device.Id == device.Id) && device.Id == bluetoothManager.ConnectedDevice.Device.Id)
                    {
                        Devices.Add(bluetoothManager.ConnectedDevice);
                    }
                }
            }


            await bluetoothManager.Adapter.StartScanningForDevicesAsync();
        }

        private async Task<bool> HasCorrectPermissions()
        {
            var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            }

            if (permissionResult != PermissionStatus.Granted)
            {
                AppInfo.ShowSettingsUI();
                return false;
            }

            return true;
        }

        //Used to update button text between connect and disconnect
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
