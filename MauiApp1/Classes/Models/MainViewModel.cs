using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.Classes.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Firebase firebase;
        private string _loginButtonText;

        public string LoginButtonText
        {
            get => _loginButtonText;
            set
            {
                _loginButtonText = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginLogoutCommand { get; }

        public MainViewModel(Firebase firebaseParam)
        {
            firebase = firebaseParam;
            LoginLogoutCommand = new Command(async () => await HandleLoginLogout());
            Initialize();
        }

        public async void Initialize()
        {
            bool isSignedIn = await firebase.IsUserSignedInAsync();
            LoginButtonText = isSignedIn ? "Logout" : "Login";
        }

        private async Task HandleLoginLogout()
        {
            bool isSignedIn = await firebase.IsUserSignedInAsync();

            if (isSignedIn)
            {
                firebase.Logout();
                LoginButtonText = "Login";
                await Application.Current.MainPage.DisplayAlert("Logged out!", "You have been successfully logged out of your account.", "Ok");
            }
            else
            {
                await Shell.Current.GoToAsync("/FirebaseLoginPage");
                bool userNowSignedIn = await firebase.IsUserSignedInAsync();
                if (userNowSignedIn)
                {
                    Console.WriteLine("User is signedin");
                    LoginButtonText = "Logout";
                } else
                {
                    Console.WriteLine("User is NOT signed in");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
