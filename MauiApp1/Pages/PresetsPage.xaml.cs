using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Classes.Models;

namespace MauiApp1.Pages
{
   public partial class PresetsPage : ContentPage
    {
        private readonly PresetsViewModel viewModel = new();
        public PresetsPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
