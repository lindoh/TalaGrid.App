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
            ControlLabel = new();
            currentAdmin = new();

            LoadNotifications();

            alerts = new();

            emailService = new();

            admin = new();
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

        [ObservableProperty]
        Users admin;

        string ReadNotification;

        [ObservableProperty]
        LabelControl controlLabel;

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

            if (notifications.Count > 0)
            {
                BtnVisible = true;
                ControlLabel.ShowLabel = false;
            }
            else
            {
                BtnVisible = false;
                ControlLabel.Color = Colors.Red;
                ControlLabel.Message = ControlLabel.messages["Notification"];
                ControlLabel.ShowLabel = true;
            }

            foreach(Notification notification in notifications)
            {
                if (notification.Read)
                    notification.BackColor = Colors.LightBlue;
            }
        }

        #endregion

        #region Class Buttons

        [RelayCommand]
        async void Approve()
        {
            if (!CurrentNotification.Read)
            {
                // Confirm the decision
                bool confirmation = await alerts.ShowConfirmationAsync("Confirmation", "Are you sure you would like to Approve this User?", "Yes", "No");

                if (confirmation)
                {
                    Admin = dataService.SearchAndVerifyAdmin(currentAdmin.UserLogin.AdminId);


                    //Update Notifications Read flag 
                    bool isUpdated = dataService.UpdateNotification(currentNotification.NotificationsId);
                    CurrentNotification.Read = isUpdated;

                    //Update User's Verified Admin property in the database
                    bool isUpdated1 = dataService.UpdateAdmin(currentNotification.User.IdNumber);

                    if (isUpdated && isUpdated1)
                    {
                        //Update notification viewcell color
                        CurrentNotification.BackColor = Colors.LightBlue;

                        
                        Users user = new();
                        user = dataService.SearchAdmin(currentNotification.User.IdNumber);

                        Approved = true;

                        //Send a response email to the user to alert them, account has been approved
                        // Check Admin Role, If Developer?
                        if (admin.AdminRole == admin.AdminRoleValue[0])
                            emailService.Send_GW_Ver_Response(user.Email, user.FirstName, user.LastName, Approved);
                        else if (admin.AdminRole == admin.AdminRoleValue[1])
                            emailService.Send_BBC_Ver_Response(user.Email, user.FirstName, user.LastName, Approved); 
                    }
                    else
                        await alerts.ShowAlertAsync("Failure", "Something went wrong, process terminated with an error");
                }
            }
            else
                await alerts.ShowAlertAsync("Acknowledged", "Notification already acknowledged, no need to do anything further");
        }

        [RelayCommand]
        async void Reject()
        {
            if (!CurrentNotification.Read)
            {
                // Confirm the decision
                bool confirmation = await alerts.ShowConfirmationAsync("Confirmation", "Are you sure you would like to Reject this User?", "Yes", "No");

                if (confirmation)
                {
                    //Update notification viewcell color
                    CurrentNotification.BackColor = Colors.LightBlue;

                    // Update the Notification Read flag
                    CurrentNotification.Read = true;

                    Users user = new();
                    user = dataService.SearchAdmin(currentNotification.User.IdNumber);

                    Approved = false;

                    //Send a response email to the user to alert them, account was Rejected
                    if (admin.AdminRole == admin.AdminRoleValue[0])
                        emailService.Send_GW_Ver_Response(user.Email, user.FirstName, user.LastName, Approved);
                    else if (admin.AdminRole == admin.AdminRoleValue[1])
                        emailService.Send_BBC_Ver_Response(user.Email, user.FirstName, user.LastName, Approved);

                    // Delete the user information from the database
                    dataService.Delete(user.Id, "Admin");
                    dataService.DeleteLogins(user.Id);
                }
            }
            else
                await alerts.ShowAlertAsync("Acknowledged", "Notification already acknowledged, no need to do anything further");
        }

        #endregion
    }
}
