using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;

namespace MauiApp1.Classes.Models
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public IDevice Device { get; }
        private bool isConnected;

        public string Name => Device.Name;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                    OnPropertyChanged(nameof(ButtonText));
                }
            }
        }

        public string ButtonText => IsConnected ? "Disconnect" : "Connect";

        public DeviceViewModel(IDevice device)
        {
            Device = device;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
