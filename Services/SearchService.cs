﻿using TalaGrid.Models;
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
                //Get the list of users that match the given name and are registered under a unique BBC
                TempList = new ObservableCollection<Users>(dataService.Search(name, selectedUser, currentAdmin.BBCId));

                foreach (Users user in TempList)
                {
                    if (user.BBCId == currentAdmin.BBCId)
                    {
                        UsersList.Add(user);
                        break;
                    }
                }
            }
            else
                alerts.ShowAlert("Search Operation Failed", "Incorrect User Radio Button is selected");

            if (UsersList.Count == 0) 
            {
                alerts.ShowAlertAsync("Search Operation Failed", "User does not exist");
            }

            return UsersList;
        }

     
    }
}
