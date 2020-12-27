using SmartCycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartCycle.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            // Assign styling to view elements
            BackgroundColor = Constants.BackgroundColor;
            lbl_username.TextColor = Constants.MainTextColor;
            lbl_password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            // When username entry is complete, move user to password entry
            entry_username.Completed += (s, e) => entry_password.Focus();
            // When password entry is complete, activate 'SignInProcedure'
            entry_password.Completed += (s, e) => SignInProcedureAsync(s, e);
        }

        async Task SignInProcedureAsync(object sender, EventArgs e)
        {
            // Creates user object from entered data
            User user = new User(entry_username.Text, entry_password.Text);

            // Check user credentials
            if(user.CheckInformation())
            {
                DisplayAlert("Login", "Succesfully Logged In!", "OK");
                var result = await App.RestService.Login(user);
                if (result.access_token != null)
                {
                    App.UserDatabase.SaveUser(user);
                }
            }
            else
            {
                DisplayAlert("Login", "Please enter your details", "OK");
            }
        }

    }
}