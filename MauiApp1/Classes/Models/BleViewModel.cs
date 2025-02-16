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
        private readonly IAdapter adapter;
        private readonly IBluetoothLE bluetoothLE;
        private DeviceViewModel connectedDevice;
        public ObservableCollection<DeviceViewModel> Devices { get; } = new();

        public Command StartScanCommand { get; }
        public Command<DeviceViewModel> ConnectCommand { get; }

        public BleViewModel()
        {
            bluetoothLE = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            //Used when device is found from StartScanningAsync. Adds discovered device if it meets specified criteria
            adapter.DeviceDiscovered += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Device.Name) && !Devices.Any(d => d.Device.Id == e.Device.Id))
                {
                    Devices.Add(new DeviceViewModel(e.Device));
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
                await adapter.ConnectToDeviceAsync(device.Device);

                device.IsConnected = true;
                connectedDevice = device;

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
                await adapter.DisconnectDeviceAsync(device.Device);
                device.IsConnected = false;
                connectedDevice = null;
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

            /* Should probably be changed to compare to BLE plugin connected devices list instead. Honestly all connection checks probably should be.
             * Will need to test with multiple connections, which shouldn't be possible with app normally anyway.*/
            if (connectedDevice != null && connectedDevice.Device.Id == device.Device.Id)
            {
                /* --<TODO>-- This is a not great way of doing this but if you change this to disconnect from device it just explodes.
                 * Too tired to figure it out right now, will fix it at some point maybe. It technically works. */
                await DisconnectFromDevice(connectedDevice);
                device.IsConnected = false;
            }
            else
            {
                //First disconnect from connected device to prevent multiple connections
                if (connectedDevice != null)
                {
                    await DisconnectFromDevice(connectedDevice);
                }
                await ConnectToDevice(device);
            }

        }
        public async Task StartScanningAsync()
        {
            if (!bluetoothLE.IsAvailable)
            {
                Debug.WriteLine("Bluetooth is not available on this device.");
                return;
            }

            if (!bluetoothLE.IsOn)
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
            //Should probably add check here to only display devices with correct name. Will prevent the need for a bunch of annoying validation stuff.
            var connectedDevices = adapter.GetSystemConnectedOrPairedDevices();
            foreach (var connectedDevice in connectedDevices)
            {
                if (!Devices.Any(d => d.Device.Id == connectedDevice.Id))
                {
                    Devices.Add(new DeviceViewModel(connectedDevice) { IsConnected = true });
                }
            }


            await adapter.StartScanningForDevicesAsync();
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
