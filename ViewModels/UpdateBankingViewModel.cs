
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using System.Collections.ObjectModel;

namespace TalaGrid.ViewModels
{
    public partial class UpdateBankingViewModel : ObservableObject
    {
        #region Default Constructor
        public UpdateBankingViewModel()
        {
            user = new Users();
            banker = new Banking();
            dataService = new DatabaseService();
            alerts = new AlertService();
            searchService = new SearchService();
            updateSaveBtnText = "Update";
            selectedUser = "Collector";
        }
        #endregion

        #region Class Properties


        DatabaseService dataService;
        AlertService alerts;
        SearchService searchService;

        [ObservableProperty]
        Users user;

        [ObservableProperty]
        Banking banker;

        //To store the list of users
        [ObservableProperty]
        ObservableCollection<Users> usersList;

        [ObservableProperty]
        string selectedUser;

        [ObservableProperty]
        string updateSaveBtnText;

        #endregion

        #region ViewModel Buttons
        /// <summary>
        /// Search method calls the Database service method
        /// to search for the current user in the database
        /// </summary>
        [RelayCommand]
        void Search(string name)
        {
            UsersList = searchService.FindUser(name, selectedUser);
        }

        /// <summary>
        /// The selectedItem method updates the User object
        /// with the selected user from the ListView
        /// </summary>
        public async void selectedItem(object sender, SelectedItemChangedEventArgs args)
        {
            User = args.SelectedItem as Users;

            //If an existing user is found and is selected from the list
            if (user.Id != 0)
            {
                Banker = dataService.SearchBanking(user.Id);

                //If the user have existing banking details
                if (banker.BankDetailsId != 0)
                {
                    //Alert the Admin that user has already existing banking details
                    await alerts.ShowConfirmationAsync("Banking Details Found", "The User Has Existing Banking Details, Update Instead?");
                    updateSaveBtnText = "Update";
                }
                else
                {
                    //Alert the Admin that the user doesn't have existing banking details
                    await alerts.ShowConfirmationAsync("No Existing Banking Details", "User Doesn't Have Existing Banking Details, Add New Banking Details");
                    updateSaveBtnText = "Save";
                }
            }
        }

        [RelayCommand]
        async void UpdateSave()
        {
            if (updateSaveBtnText == "Update")
            {
                bool isUpdated = dataService.UpdateBankingDetails(banker);

                if (isUpdated)
                    await alerts.ShowAlertAsync("Update Operation Successful", "User Banking Details Updated Successfully");
                else
                    await alerts.ShowAlertAsync("Update Operation Failed", "User banking Details Could Not Be Updated");
            }
            else if (updateSaveBtnText == "Save")
            {

                bool isSaved = dataService.NewBankingDetails(banker, user);

                if (isSaved)
                    await alerts.ShowAlertAsync("Save Operation Successful", "User Banking Details Were Saved Successfully");
                else
                    await alerts.ShowAlertAsync("Save Operation Failed", "User banking Details Could Not Be Saved");
            }

            Clear();
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Clear all text fields
        /// </summary>
        private void Clear()
        {
            Banker.BankName = "";
            Banker.BranchName = "";
            Banker.BranchCode = "";
            Banker.AccountType = "";
            Banker.AccountNumber = "";
        }
        #endregion
    }
}
