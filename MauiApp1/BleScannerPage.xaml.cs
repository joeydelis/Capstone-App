using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class BleScannerPage : ContentPage
    {
        private readonly BleViewModel viewModel = new();

        public BleScannerPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }

}
