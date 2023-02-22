using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;

//TO DO!!
//ADD EMAIL VERIFICATION

namespace TalaGrid.ViewModels
{
    public partial class CreateUserAccViewModel : ObservableObject
    {
        #region Default Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CreateUserAccViewModel()
        {
            User = new Users();
            dataService = new DatabaseService();
            alerts = new AlertService();

            //Default Values
            User.Email = " ";
            user.Suburb = " ";
            User.CellNumber = " ";
            User.Country = "South Africa";

        }
        #endregion

        #region Class Members
        DatabaseService dataService;

        [ObservableProperty]
        private Users user;

        AlertService alerts;

        #endregion

        #region ViewModel Buttons
        /// <summary>
        /// The Save Method calls databaseService method to
        /// Save the user data in the database
        /// </summary>
        [RelayCommand]
        async void Save()
        {
            if (user.IdNumber == null || user.IdNumber.Length != 13)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Id Number must be 13 digits long");
            }
            else if (CheckTextFields(this.user))
            {
                await alerts.ShowAlertAsync("Operation Failed", "One or more empty text fields found");  
            }
            else
            {
                dataService.SaveData(user);
                await alerts.ShowAlertAsync("Success", "User Account Created Successfully");
                Clear(this.user);    //Clear text fields
            }
            

        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Check if any text fields are empty
        /// </summary>
        /// <returns>Return true if empty else return false</returns>
        public bool CheckTextFields(Users user)
        {
            bool emptyFields = false;

            if (user.FirstName == null || user.LastName == null || user.IdNumber == null ||
                user.Gender == null || user.HighestQlfn == null || user.IncomeRange == null ||
                user.StreetAddress == null || user.City == null || user.Province == null || user.Country == null)
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
        }

        #endregion
    }
}
