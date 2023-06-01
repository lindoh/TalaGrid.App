using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TalaGrid.Models;
using TalaGrid.Services;

namespace TalaGrid.ViewModels
{
    public partial class NotificationsViewModel : ObservableObject
    {
        #region Constructor
        public NotificationsViewModel()
        {
            // Initialize your notifications collection with some sample data
            OldNotification = new();
            CurrentNotification = new();
            dataService = new();

            currentAdmin = new();

            LoadNotifications();

            alerts = new();

            emailService = new();
        }
        #endregion

        #region Class Properties

        [ObservableProperty]
        private ObservableCollection<Notification> notifications;

        [ObservableProperty]
        private Notification currentNotification;

        [ObservableProperty]
        static private Notification oldNotification;

        [ObservableProperty]
        LoginViewModel currentAdmin;

        DatabaseService dataService;

        AlertService alerts;

        EmailService emailService;

        bool Approved;

        [ObservableProperty]
        private bool btnVisible;

        #endregion

        #region Class Methods
        public void SelectedItem(object sender, SelectedItemChangedEventArgs args)
        {
            // The old notification set to the current notification
            OldNotification = currentNotification; 
            // Initiate a new notification object with the current selected notification
            CurrentNotification = args.SelectedItem as Notification;

        }

        private void LoadNotifications() 
        {
            notifications = new ObservableCollection<Notification>(dataService.LoadNotifications(currentAdmin.UserLogin.AdminId));
        }

        #endregion

        #region Class Buttons

        [RelayCommand]
        async void Approve()
        {
            // Confirm the decision
            bool confirmation = await alerts.ShowConfirmationAsync("Confirmation", "Are you sure you would like to Approve this User?", "Yes", "No");
            
            if (confirmation)
            {
                //Update Notifications Read flag and disable the item selection mode
                bool isUpdated = dataService.UpdateNotification(currentNotification.NotificationsId);

                //Update User's Verified Admin property in the database
                bool isUpdated1 = dataService.UpdateAdmin(currentNotification.User.IdNumber);

                if (isUpdated && isUpdated1)
                {
                    CurrentNotification.BackColor = Color.FromRgba("#989a9e");

                    //Send a response email to the user to alert them, account has been approved
                    Users user = new();
                    user = dataService.SearchAdmin(currentNotification.User.IdNumber);

                    Approved = true;

                    emailService.Send_GW_Ver_Response(user.Email, user.FirstName, user.LastName, Approved);
                }
                else
                    await alerts.ShowAlertAsync("Failure", "Something went wrong, process terminated with an error");
            }
        }

        [RelayCommand]
        async void Reject()
        {
            // Confirm the decision
            bool confirmation = await alerts.ShowConfirmationAsync("Confirmation", "Are you sure you would like to Reject this User?", "Yes", "No");

            if (confirmation) 
            {
                CurrentNotification.Read = !CurrentNotification.Read; //Disable the notification viewcell
                CurrentNotification.BackColor = Color.FromRgba("#989a9e");

                //Send a response email to the user to alert them, account has been approved
                Users user = new();
                user = dataService.SearchAdmin(currentNotification.User.IdNumber);

                Approved = false;

                emailService.Send_GW_Ver_Response(user.Email, user.FirstName, user.LastName, Approved);
            }
        }

        #endregion
    }
}
