
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

        Notification notification;

        Users admin;

        string Subject;
        string Message;
        string Message1;

        #endregion

        #region ViewModel Buttons
        /// <summary>
        /// The Save Method calls databaseService method to
        /// Save the user data in the database
        /// </summary>
        [RelayCommand]
        async void Register()
        {
            bool isSaved = false;
            bool isSaved1 = false;

            if(dataService.SearchAdmin(User.IdNumber).Id > 0)
                 await alerts.ShowAlertAsync("Operation Failed", "Id Number or User already exists");

            else if (user.IdNumber == null || user.IdNumber.Length != 13)
                await alerts.ShowAlertAsync("Operation Failed", "Id Number must be 13 digits long");

            else if (user.CellNumber.Length != 10)
                await alerts.ShowAlertAsync("Operation Failed", "Cellphone Number must be 10 digits long");

            else if (!CheckTextFields(user))
            {
                User.BBCId = 0;

                notification = new Notification();

                if (user.AdminRole == user.AdminRoleValue[0]) // Admin
                    user.VerifiedAdmin = true;  //By default this user should be verified

                // If GreenWay Africa Admin Registration
                else if (user.AdminRole == user.AdminRoleValue[1])
                {
                    User.VerifiedAdmin = false;           //Reset verification flag

                    Subject = "Admin Verification";

                    // Message to be sent to the Admin via email
                    Message = $"Hi Admin, \n\nA verification for a new registered GreenWay Africa Admin, {user.FirstName} {user.LastName} " +
                        $"with Id Number: {user.IdNumber} is required. Please Login into the App and action the request. \n\n\nRegards, \nTalagrid";

                    // Message for the Admin In-App notification
                    Message1 = $"Hi Admin, \n\nA verification for a new registered GreenWay Africa Admin, {user.FirstName} {user.LastName} " +
                        $"with Id Number: {user.IdNumber} is required. Please Action the request";

                    notification.Title = Subject;
                    notification.Message = Message1;
                    notification.User = user;
                    notification.Admin = AdminToAction(user);
                    notification.Read = false;                  //By default the notification has not been read

                    //Send verification email to the Application Admin (Developer)
                    emailService.Send_GW_Verification(notification.Admin.Email, user.FirstName, user.LastName, user.IdNumber, Subject, Message);

                }
                else if (user.AdminRole == user.AdminRoleValue[2]) // BBC_Admin
                {
                    User.VerifiedAdmin = false;           //Reset verification flag

                    Subject = "Admin Verification";

                    // Message to be sent to the Admin via email
                    Message = $"Hi Admin, \n\nA verification for a new registered Buy-Back-Center Admin, {user.FirstName} {user.LastName} " +
                        $"with Id Number: {user.IdNumber} is required. Please Login into the App and action the request. \n\n\nRegards, \nTalagrid";

                    // Message for the Admin In-App notification
                    Message1 = $"Hi Admin, \n\nA verification for a new registered Buy-Back-Center Admin, {user.FirstName} {user.LastName} " +
                        $"with Id Number: {user.IdNumber} is required. Please Action the request";

                    notification.Title = Subject;
                    notification.Message = Message1;
                    notification.User = user;
                    notification.Admin = AdminToAction(user);
                    notification.Read = false;                  //By default the notification has not been read

                    //Send verification email to the Application Admin (Developer)
                    emailService.Send_BBC_Verification(notification.Admin.Email, user.FirstName, user.LastName, user.IdNumber);
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "Something went wrong, account could not be created!");


                // Save the admin details in the database
                isSaved = dataService.SaveAdminData(user);
                // Save the notification in the database
                isSaved1 = dataService.SaveNotification(notification);

                if (isSaved && isSaved1) 
                {
                    await alerts.ShowAlertAsync("Success", "User Account Created Successfully, pending verification");

                    //Save the user's Id number before class properties are cleared 
                    Logins.IdNumber = user.IdNumber;

                    //Navigate to the Create Login Details Page
                    App.Current.MainPage = new CreateLoginsView();
                    //await Shell.Current.GoToAsync(nameof(CreateLoginsView));
                }
                else
                {
                    // Delete incomplete records from DB
                    DeleteFromDB(isSaved, isSaved1, user.IdNumber);
                        
                    await alerts.ShowAlertAsync("Operation Failed", "Something went wrong, account could not be created!");
                }

                Clear(User);    //Clear text fields

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

        Users AdminToAction(Users user)
        {
            Users admin = new();

            // If GreenWay admin registration, admin (developer) to action
            if (user.AdminRole == user.AdminRoleValue[1])
            {
                // Lindokuhle Gamede is the current admin
                string IdNumber = "9409215626080";
                admin = dataService.SearchAdmin(IdNumber);
            }
            else if (user.AdminRole == user.AdminRoleValue[2])
            {
                // Zee is the current admin
                string IdNumber = "9703155626084";
                admin = dataService.SearchAdmin(IdNumber);
            }

            return admin;
        }

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

        private void DeleteFromDB(bool isSaved, bool isSaved1, string IdNumber)
        {
            if (isSaved)
                dataService.DeleteAdminWithId(IdNumber);

            if (isSaved1)
                dataService.DeleteNotification(IdNumber);
        }
        #endregion
    }
}
