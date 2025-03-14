using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Classes;

namespace MauiApp1.Pages
{
    public partial class FirebaseSignUpPage : ContentPage
    {
        private readonly Firebase firebase;
        public FirebaseSignUpPage()
        {
            InitializeComponent();
            firebase = Firebase.Instance;
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text.Trim();
            string password = PasswordEntry.Text;

            bool responseSuccess = await firebase.SignUpAsync(email, password);

            if (responseSuccess)
            {
                await DisplayAlert("Success", "Account created successfully!", "OK");
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Sign-up failed. Please try again.", "OK");
            }
        }


    }

}
