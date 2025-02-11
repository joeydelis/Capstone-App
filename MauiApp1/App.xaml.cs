namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
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