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
            Routing.RegisterRoute(nameof(FirebaseLoginPage), typeof(FirebaseLoginPage));
            Routing.RegisterRoute(nameof(FirebaseSignUpPage), typeof(FirebaseSignUpPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
