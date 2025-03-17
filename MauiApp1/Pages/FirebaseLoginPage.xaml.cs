using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Classes;

namespace MauiApp1.Pages
{
    public partial class FirebaseLoginPage : ContentPage
    {
        private readonly Firebase firebase;
        public FirebaseLoginPage(Firebase firebaseParam)
        { 
            InitializeComponent();
            firebase = firebaseParam;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text.Trim();
            string password = PasswordEntry.Text;

            bool responseSuccess = await firebase.SignInAsync(email, password);

            if (responseSuccess)
            {
                await DisplayAlert("Success", "Signed in!", "OK");
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Sign-in failed. Please try again.", "OK");
            }
        }
        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/FirebaseSignUpPage");
        }

    }

 }
