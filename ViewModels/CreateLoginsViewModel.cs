using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.Views;

namespace TalaGrid.ViewModels
{
    public partial class CreateLoginsViewModel : ObservableObject
    {
        public CreateLoginsViewModel()
        {
            userLogins = new Login();
            dataService = new DatabaseService();
            alerts = new AlertService();
            confirmPassword = "";
            userLogins = new Login();
            controlLabel = new LabelControl();
        }

        [ObservableProperty]
        Login userLogins;

        [ObservableProperty]
        static string idNumber;

        [ObservableProperty]
        public string confirmPassword;

        DatabaseService dataService;

        AlertService alerts;

        [ObservableProperty]
        LabelControl controlLabel;

        [RelayCommand]
        async void Continue()
        {
            Users user = new Users();

            //Check if the Username provided is already taken
            int id = dataService.SearchAdminUsername(UserLogins.Username);
            if (id > 0)
            {
                ControlLabel.Color = Colors.Red;
                ControlLabel.Message = ControlLabel.messages["Username Invalid"];
                ControlLabel.ShowLabel = true;
                return;
            }
            else
            {
                ControlLabel.Color = Colors.Green;
                ControlLabel.Message = ControlLabel.messages["Username Valid"];
                ControlLabel.ShowLabel = true;
            }

            user = dataService.SearchAdmin(idNumber);

            if (!ValidatePassword(UserLogins.Password))
                await alerts.ShowAlertAsync("Invalid Password", "Please check Password Guidlines Highlighted in red below!");
            else if (!(UserLogins.Password == confirmPassword))
                await alerts.ShowAlertAsync("Operation Failed", "Passwords do not match");
            else if (!(UserLogins.Username == "" && UserLogins.Password == "" && confirmPassword == ""))
            {
                UserLogins.AdminId = user.Id;
                bool isSaved = dataService.SaveLogins(userLogins);

                if (isSaved)
                {
                    await alerts.ShowAlertAsync("Operation Successful", "Login Details Saved Successfully");
                    //Navigate to the Home Page
                    App.Current.MainPage = new LoginView();
                    //await Shell.Current.GoToAsync(nameof(LoginView));
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "The password could not be updated!");
            }

            ControlLabel.ShowLabel = false;
            Clear();

        }

        public bool ValidatePassword(string password)
        {
            int validConditions = 0;

            //Check the length of Password, if must be >= 8 otherwise it is invalid
            if (password.Length < 8)
                return false;

            //Check if the Password has a LowerCase character
            foreach (char c in password)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            //Check if the Password has an UpperCase character
            foreach (char c in password)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            //Check if the Password has a number
            if (validConditions == 0) return false;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            //If only one of the above is found return false
            if (validConditions == 1)
                return false;
            //If two of the above are found continue checking for special characters
            if (validConditions == 2)
            {
                char[] special = { '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '/', '=' }; // or whatever    
                if (password.IndexOfAny(special) == -1) return false;
            }

            //If all is well, return true
            return true;
        }

        /// <summary>
        /// Clear Text Fields
        /// </summary>
        private void Clear()
        {
            UserLogins.Username = "";
            UserLogins.Password = "";
        }


    }
}
