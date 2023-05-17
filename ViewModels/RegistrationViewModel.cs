
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.Views;


namespace TalaGrid.ViewModels
{
    public partial class RegistrationViewModel : ObservableObject
    {
        #region Default Constructor
        public RegistrationViewModel()
        {
            dataService = new DatabaseService();
            user = new Users();
            alerts = new AlertService();
            emailService = new EmailService();
            logins = new CreateLoginsViewModel();

            //Default Values
            User.Email = " ";
            user.Suburb = " ";
            User.Country = "South Africa";
        }

        #endregion

        #region Class Members
        DatabaseService dataService;

        [ObservableProperty]
        private Users user;

        [ObservableProperty]
        Login userLogins;

        [ObservableProperty]
        CreateLoginsViewModel logins;


        [ObservableProperty]
        string confirmPassword;

        AlertService alerts;

        EmailService emailService;

        #endregion

        #region ViewModel Buttons
        /// <summary>
        /// The Save Method calls databaseService method to
        /// Save the user data in the database
        /// </summary>
        [RelayCommand]
        async void Register()
        {
            if(dataService.SearchAdmin(User.IdNumber).Id > 0)
                 await alerts.ShowAlertAsync("Operation Failed", "Id Number or User already exists");

            else if (user.IdNumber == null || user.IdNumber.Length != 13)
                await alerts.ShowAlertAsync("Operation Failed", "Id Number must be 13 digits long");

            else if (!CheckTextFields(user))
            {
                User.BBCId = 0;
                dataService.SaveAdminData(user);

                if (user.AdminRole == user.AdminRoleValue[0]) // Admin
                    await alerts.ShowAlertAsync("Success", "User Account Created Successfully");

                else if (user.AdminRole == user.AdminRoleValue[1]) // GW_Admin
                {
                    await alerts.ShowAlertAsync("Success", "User Account Created Successfully, pending verification");

                    //Reset verification flag
                    User.VerifiedAdmin = false;

                    //Send verification email to the Application Admin (Developer)
                    emailService.Send_GW_Verification(user.Email, user.FirstName, user.LastName, user.IdNumber);

                    
                }
                else if (user.AdminRole == user.AdminRoleValue[2]) // BBC_Admin
                {
                    await alerts.ShowAlertAsync("Success", "User Account Created Successfully, pending verification");
                }


                //Save the user's Id number before class properties are cleared 
                Logins.IdNumber = user.IdNumber;

                Clear(User);    //Clear text fields

                //Navigate to the Create Login Details Page
                App.Current.MainPage = new CreateLoginsView();
                //await Shell.Current.GoToAsync(nameof(CreateLoginsView));
            }
            else
            {
                await alerts.ShowAlertAsync("Operation Failed", "One or more empty text fields found");
            }
        }

        /// <summary>
        /// Go back to the Home Page View
        /// </summary>
        [RelayCommand]
        void ToLogin()
        {
            App.Current.MainPage = new LoginView();
        }

        #endregion

        #region Helper Methods



        /// <summary>
        /// Check if any text fields are empty
        /// </summary>
        /// <returns>Return false if empty else return True</returns>
        private bool CheckTextFields(Users user)
        {
            bool emptyFields = false;

            if (user.FirstName == "" || user.LastName == "" || user.IdNumber == "" ||
                user.Gender == "" || user.HighestQlfn == "" || user.IncomeRange == "" ||
                user.CellNumber == "" || user.StreetAddress == "" ||
                user.City == "" || user.Province == "" || user.Country == "" || user.AdminRole == "")
            {
                emptyFields = true;

            }

            return emptyFields;
        }

        /// <summary>
        /// Clear all text fields
        /// </summary>
        public void Clear(Users user)
        {
            user.FirstName = "";
            user.LastName = "";
            user.IdNumber = "";
            user.Gender = "";
            user.HighestQlfn = "";
            user.IncomeRange = "";
            user.Email = "";
            user.CellNumber = "";
            user.StreetAddress = "";
            user.Suburb = "";
            user.City = "";
            user.Province = "";
            user.Country = "";
            user.AdminRole = "";
        }

        #endregion
    }
}
