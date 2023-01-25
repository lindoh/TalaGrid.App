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
            else if (!CheckTextFields())
            {
                bool isUpdated = dataService.Update(user, selectedUser);

                if (selectedUser == "Admin")
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

            //If the Admin is selected, Load data from the database to
            //update the BuyBackCentre object
            if (selectedUser == "Admin")
                LoadBBCData();
        }

        private void LoadBBCData()
        {
            if (user.Id != 0)
            {
                BBC = dataService.SearchBBC(user.Id);

                if (BBC.BBCId == 0)
                {
                    alerts.ShowAlertAsync("BBC Data Update Required", "No assocciated BuyBackCentre data found, please Update!");
                    NewBBCUser = true;
                    BBC.Country = "South Africa";
                }
                else
                    NewBBCUser = false;
            }
        }

        private bool UpdateBBC()
        {
            bool isUpdated = false;

            //If this is a new BuyBackCentre User, save data in the database
            //Otherwise, update existing data
            if (newBBCUser)
                isUpdated = dataService.SaveBBCData(bBC, user);
            else
                isUpdated = dataService.UpdateBBC(bBC);

            return isUpdated;
        }

        /// <summary>
        /// Check if any text fields are empty
        /// </summary>
        /// <returns>Return false if empty else return True</returns>
        private bool CheckTextFields()
        {
            bool emptyFields = false;

            if (selectedUser == "Admin")
            {
                if (user.FirstName == "" || user.LastName == "" || user.IdNumber == "" ||
               user.Gender == "" || user.HighestQlfn == "" || user.IncomeRange == "" ||
               user.StreetAddress == "" || user.City == "" || user.Province == "" ||
               user.Country == "" || bBC.BuyBackCentreName == "" || bBC.StreetAddress == "" ||
               bBC.City == "" || bBC.Province == "" || bBC.Country == "")
                {
                    emptyFields = true;
                }
            }
            else
            {
                if (user.FirstName == "" || user.LastName == "" || user.IdNumber == "" ||
               user.Gender == "" || user.HighestQlfn == "" || user.IncomeRange == "" ||
               user.StreetAddress == "" || user.City == "" || user.Province == "" ||
               user.Country == "")
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
