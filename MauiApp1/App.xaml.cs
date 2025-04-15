using MauiApp1.Classes;
using MauiApp1.Pages;
using MauiApp1.Services;
using Microsoft.Maui;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF1cXmhLYVF2WmFZfVtgdVVMYVlbR3ZPMyBoS35Rc0VhWHxfcHVQRWBbWEFx");
            ApplyTheme();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
        private void ApplyTheme()
        {
            if (AppInfo.RequestedTheme == AppTheme.Dark)
            {
                Current.Resources["PageStyle"] = Current.Resources["DarkPageStyle"];
            }
            else
            {
                Current.Resources["PageStyle"] = Current.Resources["LightPageStyle"];
            }
        }
    }
}