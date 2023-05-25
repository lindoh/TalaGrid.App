using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using System.Collections.ObjectModel;

namespace TalaGrid.ViewModels
{
    public partial class UpdateUserAccViewModel : ObservableObject
    {
        #region Default Constructor
        public UpdateUserAccViewModel()
        {
            dataService = new DatabaseService();
            User = new Users();
            bBC = new BuyBackCentre();
            usersList = new ObservableCollection<Users>();
            alerts = new AlertService();
            createUserAccViewModel = new CreateUserAccViewModel();
            searchService = new SearchService();
            ShowBBCSection = false;
            User.Country = "South Africa";
            BBC.Country = "South Africa";
            BBC.Suburb = " ";
        }

        #endregion

        #region Class Properties
        //Database service object
        DatabaseService dataService;

        SearchService searchService;

        //CreateUserAccViewModel
        //To assist with common methods needed by this
        //ViewModel Class
        CreateUserAccViewModel createUserAccViewModel;

        //Alert service object
        AlertService alerts;

        [ObservableProperty]
        Users user;

        //Buy Back Centre 
        [ObservableProperty]
        BuyBackCentre bBC;

        //To store the list of users
        [ObservableProperty]
        ObservableCollection<Users> usersList;

        //Which user details are being updated
        [ObservableProperty]
        string selectedUser;

        [ObservableProperty]
        bool showBBCSection;

        [ObservableProperty]
        bool newBBCUser;

        #endregion

        #region ViewModel Buttons

        /// <summary>
        /// Update Method calls the Database service Update Method
        /// to update the User's details
        /// </summary>
        [RelayCommand]
        async void Update()
        {
            if (user.IdNumber == null || user.IdNumber.Length != 13)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Id Number must be 13 digits long");
            }
            if (User.CellNumber.Length > 10)
                await alerts.ShowAlertAsync("Operation Failed", "Cell Number must be 10 digits long");
            else if (!CheckTextFields())
            {
                bool isUpdated = dataService.Update(user, selectedUser);

                if (selectedUser == "Admin" && (user.AdminRole == user.AdminRoleValue[1] || user.AdminRole == user.AdminRoleValue[2]))
                    UpdateBBC();

                if (isUpdated)
                {
                    await alerts.ShowAlertAsync("Success", "User Account Updated Successfully");
                    Clear();    //Clear text fields
                }
            }
            else
            {
                await alerts.ShowAlertAsync("Operation Failed", "One or more empty text fields found");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// The selectedItem method updates the User object
        /// with the selected user from the ListView
        /// The BuyBackCentre object is updated here
        /// </summary>
        public void selectedItem(object sender, SelectedItemChangedEventArgs args)
        {
            User = args.SelectedItem as Users;

        }

        private void LoadBBCData()
        {
            if (user.Id != 0)
            {
                BBC = dataService.SearchBBC(user.Id);

                if (BBC.BBCId == 0)
                {
                    //alerts.ShowAlertAsync("Missing Information", "No assocciated BuyBackCenter data found, please Update under Update User Account tab");
                    NewBBCUser = true;
                    BBC.Country = "South Africa";
                    BBC.Suburb = "";
                }
                else
                    NewBBCUser = false;
            }
        }

        private bool UpdateBBC()
        {
            bool isUpdated = false;

            // If a BBC admin
            if (user.AdminRole == user.AdminRoleValue[2])
                LoadBBCData();

            //If this is a new BuyBackCentre User, save data in the database
            //Otherwise, update existing data
            if (newBBCUser)
                isUpdated = dataService.SaveBBCData(bBC, user);
            else
            {
                isUpdated = dataService.UpdateBBC(bBC);
            }

            return isUpdated;
        }

        /// <summary>
        /// Check if any text fields are empty
        /// </summary>
        /// <returns>Return false if empty else return True</returns>
        private bool CheckTextFields()
        {
            bool emptyFields = false;

            if (selectedUser == "Admin" && (user.AdminRole == user.AdminRoleValue[1] || user.AdminRole == user.AdminRoleValue[2]))
            {
                if (user.FirstName == null || user.LastName == null || user.IdNumber == null ||
                user.Gender == null || user.HighestQlfn == null || user.IncomeRange == null ||
                user.StreetAddress == null || user.City == null || user.Province == null || user.Country == null
                || bBC.BuyBackCentreName == null || bBC.StreetAddress == null ||
               bBC.City == null || bBC.Province == null || bBC.Country == null)
                {
                    emptyFields = true;
                }
            }
            else if (selectedUser == "Collector" || user.AdminRole == user.AdminRoleValue[0])
            {
                if (user.FirstName == null || user.LastName == null || user.IdNumber == null ||
                user.Gender == null || user.HighestQlfn == null || user.IncomeRange == null ||
                user.StreetAddress == null || user.City == null || user.Province == null || user.Country == null)
                {
                    emptyFields = true;

                }
            }
           

            return emptyFields;
        }

        /// <summary>
        /// Clear all text fields
        /// </summary>
        private void Clear()
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
            bBC.BuyBackCentreName = "";
            bBC.StreetAddress = "";
            bBC.Suburb = "";
            bBC.City = "";
            bBC.Province = "";
        }

        #endregion
    }
}
