using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Classes.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace MauiApp1.Classes
{
    public class BluetoothManager
    {
        private static BluetoothManager instance;

        public IBluetoothLE BluetoothLE { get; private set; }
        public IAdapter Adapter { get; private set; }
        public DeviceViewModel ConnectedDevice { get; set; }

        private BluetoothManager()
        {
            BluetoothLE = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
        }

        public static BluetoothManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BluetoothManager();
                }
                return instance;
            }
        }
    }
}
