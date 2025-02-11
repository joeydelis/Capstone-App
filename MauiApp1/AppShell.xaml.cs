using MauiApp1.Pages;
namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(BluetoothConnectionPage), typeof(BluetoothConnectionPage));
            Routing.RegisterRoute(nameof(StrengthPage), typeof(StrengthPage));
            Routing.RegisterRoute(nameof(TimerPage), typeof(TimerPage));
        }
    }
}
