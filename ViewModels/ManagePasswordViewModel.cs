using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.Views;
using System.Text;

namespace TalaGrid.ViewModels
{
    public partial class ManagePasswordViewModel : ObservableObject
    {
        public ManagePasswordViewModel()
        {
            dataService = new DatabaseService();
            alerts = new AlertService();
            createLoginsVM = new CreateLoginsViewModel();
            login = new LoginViewModel();
            email = new EmailService();
        }

        [ObservableProperty]
        string oldPassword;

        [ObservableProperty]
        string newPassword;

        [ObservableProperty]
        string confirmPassword;

        [ObservableProperty]
        string oneTimePin;

        [ObservableProperty]
        string userOTP;

        DatabaseService dataService;

        AlertService alerts;

        [ObservableProperty]
        LoginViewModel login;

        CreateLoginsViewModel createLoginsVM;

        EmailService email;

        [ObservableProperty]
        string idNumber;

        //If false it means we are reseting password, else, it's an update operation
        [ObservableProperty]
        bool reset_Update_Password;

        #region Class Buttons

        [RelayCommand]
        async void Update()
        {
            if (reset_Update_Password)
            {
                if (Login.UserLogin.IsLoggedIn)
                {
                    //Confirm Old Password
                    if (oldPassword == null || NewPassword == null || confirmPassword == null)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "One or more empty fields found");
                        return;
                    }

                    if (login.UserLogin.Password != oldPassword)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "The provided Old Password is invalid!");
                        return;
                    }


                    if (newPassword == oldPassword)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "Your new password cannot be similar to your old password");
                        return;
                    }
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "The user is not Logged in");
            }


            //Validate password
            if (!createLoginsVM.ValidatePassword(NewPassword))
                await alerts.ShowAlertAsync("Invalid Password", "Please check Password Guidlines Highlighted in red below!");
            else if (!(NewPassword == confirmPassword))
                await alerts.ShowAlertAsync("Operation Failed", "Passwords do not match");
            else
            {
                bool isUpdated = dataService.UpdatePassword(login.UserLogin.AdminId, newPassword);

                if (isUpdated)
                {
                    await alerts.ShowAlertAsync("Operation Successful", "Password Updated Successfully");
                    Clear();
                    //Navigate to the Home Page
                    //App.Current.MainPage = new AppShell();
                    //await Shell.Current.GoToAsync(nameof(LoginView));
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "The password could not be updated!");
            }

            Clear();
        }

        [RelayCommand]
        async void Continue()
        {
            if (userOTP != null)
            {
                if (userOTP == oneTimePin)
                {
                    await alerts.ShowAlertAsync("Success", "Head to the next page to create a new Password");
                    App.Current.MainPage = new ManagePasswordView();
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "Type in a correct OTP and click Generate New OTP");
            }
            else
                await alerts.ShowAlertAsync("Operation Failed", "Type in a correct OTP and click Generate New OTP");
        }

        [RelayCommand]
        async void GenerateNewOTP()
        {
            //If The Generate OTP Button is pressed it means we are resetting Password
            Reset_Update_Password = false;

            Users user = new Users();

            user = dataService.SearchAdmin(idNumber);

            if (user.Id == 0 || user.Email == null)
                await alerts.ShowAlertAsync("Operation Failed", "User is not recognized, please enter a valid Id number");
            else
            {
                GenerateOTP(user);
                await alerts.ShowAlertAsync("Success", "User found, please check your email address for the OTP");
            }
        }

        [RelayCommand]
        void GoBack()
        {
            App.Current.MainPage = new LoginView();
        }

        #endregion
        /// <summary>
        /// Clear Text Fields
        /// </summary>
        private void Clear()
        {
            //Login.UserLogin.Username = "";
            //Login.UserLogin.Password = "";
            NewPassword = String.Empty;
            ConfirmPassword = String.Empty;
        }

        private void GenerateOTP(Users user)
        {
            Random rand = new Random();
            StringBuilder randomString = new StringBuilder();

            //The length of the generated OTP
            int OTP_Length = 6;

            //Clear previous OTP
            OneTimePin = string.Empty;

            for (int i = 0; i < OTP_Length; i++)
            {
                int number = rand.Next(10);
                randomString.Append(number);
            }

            OneTimePin = randomString.ToString();

            email.SendEmail(user.Email, user.FirstName, user.LastName, oneTimePin);
        }



    }
}
