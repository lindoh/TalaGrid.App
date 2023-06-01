using TalaGrid.Models;
using System.Collections.ObjectModel;
using TalaGrid.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Services
{
    public partial class SearchService : ObservableObject
    {
        public SearchService()
        {
            dataService = new();
            alerts = new();
            CurrentAdmin = new();
        }

        readonly DatabaseService dataService;
        readonly AlertService alerts;

        [ObservableProperty]
        Login currentAdmin;


        /// <summary>
        /// This method is used to find a collector from the database
        /// </summary>
        /// <param name="name">Collector's name</param>
        /// <param name="selectedUser">Collector</param>
        /// <returns></returns>
        public ObservableCollection<Users> FindUser(string name, string selectedUser)
        {
            ObservableCollection<Users> UsersList = new();
            ObservableCollection<Users> TempList = new();

            if (selectedUser != null)
            {
                //Get the list of collectors that match the given name and are registered under a unique BBC
                if (selectedUser == "Collector")
                    TempList = new ObservableCollection<Users>(dataService.SearchCollector(name, currentAdmin.BBCId));
                else if (selectedUser == "Admin")
                    TempList = new ObservableCollection<Users>(dataService.SearchAdmin(name, currentAdmin.AdminId));

                foreach (Users user in TempList)
                {
                    if (user.BBCId == currentAdmin.BBCId)
                    {
                        UsersList.Add(user);
                        break;
                    }
                    else
                        alerts.ShowAlertAsync("Search Failure", "Admin not logged in properly, relogin or contact system administrator");
                }
            }
            else
                alerts.ShowAlert("Search Failure", "Incorrect User Radio Button is selected");

            return UsersList;
        }

     
    }
}
