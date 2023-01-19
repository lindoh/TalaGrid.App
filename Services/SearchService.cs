using TalaGrid.Models;
using System.Collections.ObjectModel;


namespace TalaGrid.Services
{
    public partial class SearchService
    {
        public SearchService()
        {
            dataService = new();
            alerts = new();
        }

        readonly DatabaseService dataService;
        readonly AlertService alerts;

        public ObservableCollection<Users> FindUser(string name, string selectedUser)
        {
            ObservableCollection<Users> UsersList = new();

            if (selectedUser != null)
            {
                //Get the list of users that match the given name
                UsersList = new ObservableCollection<Users>(dataService.Search(name, selectedUser));

                //If the user is not found, notify the user
                if (UsersList.Count == 0)
                    alerts.ShowAlert("Search Operation Failed", "User not found");
            }
            else
                alerts.ShowAlert("Search Operation Failed", "The User Category Must be Selected First");

            return UsersList;
        }
    }
}
