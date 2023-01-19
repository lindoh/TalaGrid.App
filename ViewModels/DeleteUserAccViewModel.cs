using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using System.Collections.ObjectModel;

//      TO DO
//Allow the user to see their info before deletion

namespace TalaGrid.ViewModels
{
    public partial class DeleteUserAccViewModel : ObservableObject
    {
        public DeleteUserAccViewModel()
        {
            dataService = new DatabaseService();
            alerts = new AlertService();
            User = new Users();
            UsersList = new ObservableCollection<Users>();
            searchService = new SearchService();
            ControlLabel = new LabelControl();
        }

        #region Class Properties
        //Database service object
        DatabaseService dataService;

        SearchService searchService;

        //Alert service object
        AlertService alerts;

        //The current user
        [ObservableProperty]
        Users user;

        //To store the list of users
        [ObservableProperty]
        ObservableCollection<Users> usersList;

        [ObservableProperty]
        string selectedUser;

        [ObservableProperty]
        LabelControl controlLabel;

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

        [RelayCommand]
        void Delete()
        {
            bool isDeleted = dataService.Delete(user.Id, selectedUser);

            if (isDeleted)
            {
                ControlLabel.Color = Colors.Green;
                ControlLabel.Message = ControlLabel.messages["Delete Operation Successful"];
                ControlLabel.ShowLabel = true;
            }
            else
            {
                ControlLabel.Color = Colors.Red;
                ControlLabel.Message = ControlLabel.messages["Delete Operation Failed"];
                ControlLabel.ShowLabel = true;
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// The selectedItem method updates the User object
        /// with the selected user from the ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void selectedItem(object sender, SelectedItemChangedEventArgs args)
        {
            User = args.SelectedItem as Users;
        }
        #endregion
    }
}
