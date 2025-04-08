using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Classes;

namespace MauiApp1.Pages
{
    public partial class ControllerPage : ContentPage 
    {
        private readonly BluetoothManager bluetoothManager;
        public ControllerPage()
        {
            InitializeComponent();
            bluetoothManager = BluetoothManager.Instance;
        }

        private async void OnUpClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Up", "", "OK");
        }

        private async void OnDownClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Down", "", "OK");
        }

    }
}
