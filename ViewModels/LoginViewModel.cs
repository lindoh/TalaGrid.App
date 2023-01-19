using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.Views;
using Microsoft.IdentityModel.Tokens;

namespace TalaGrid.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public LoginViewModel()
        {
            dataService = new DatabaseService();
            alerts = new AlertService();
            userLogin = new Login();
            ControlLabel = new LabelControl();
        }

        DatabaseService dataService;
        AlertService alerts;

        [ObservableProperty]
        public static Login userLogin;

        [ObservableProperty]
        BuyBackCentre buyBackCentre;

        [ObservableProperty]
        LabelControl controlLabel;


        #region Class Buttons

        [RelayCommand]
        async void Login()
        {

            //Check if text field are empty first
            if (!CheckFields())
            {
                dataService.SearchLogins(UserLogin);

                if (UserLogin.IsLoggedIn)
                {
                    ControlLabel.Color = Colors.Green;
                    ControlLabel.Message = ControlLabel.messages["Access Granted"];
                    ControlLabel.ShowLabel = true;

                    buyBackCentre = dataService.SearchBBC(UserLogin.AdminId);

                    //An Old User should hopefully have an existing BBBCId
                    if (buyBackCentre.BBCId != 0)
                    {
                        UserLogin.IsBBCUpdated = true;
                        UserLogin.BBCId = buyBackCentre.BBCId;
                    }
                    else
                    {
                        await alerts.ShowAlertAsync("Missing Information Detected", "The user is required to Update BuyBackCentre Details under Update User Account Tab and ReLogin");

                        //await Shell.Current.GoToAsync("../../../");
                    }

                    //Navigate to the Home Page
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    await alerts.ShowAlertAsync("Login Failed", "The user could not be found, Register a new account instead");
                }
            }
            else
            {
                // await alerts.ShowAlertAsync("Operation Failed", "Username and/or Password text fields are empty");
                ControlLabel.Color = Colors.Red;
                ControlLabel.Message = ControlLabel.messages["Operation Failed"];
                ControlLabel.ShowLabel = true;
            }


            //Clear Text fields
            //Clear();
        }

        [RelayCommand]
        async void Logout()
        {
            bool confirmation = await alerts.ShowConfirmationAsync("Confirm Logout", "Are you sure you want to Logout?", "Yes", "No");



            if (confirmation)
            {
                Clear();
                App.Current.MainPage = new LoginView();

            }
        }

        [RelayCommand]
        void ResetPassword()
        {
            App.Current.MainPage = new ResetPasswordView();
        }

        [RelayCommand]
        void ToRegister()
        {
            App.Current.MainPage = new RegistrationView();
        }
        #endregion

        #region Helper Methods
        private bool CheckFields()
        {
            if (userLogin.Username.IsNullOrEmpty() && userLogin.Password.IsNullOrEmpty())
                return true;
            else
                return false;

        }

        /// <summary>
        /// Clear Text Fields
        /// </summary>
        private void Clear()
        {
            UserLogin.Username = String.Empty;
            UserLogin.Password = String.Empty;
        }



        #endregion
    }
}
