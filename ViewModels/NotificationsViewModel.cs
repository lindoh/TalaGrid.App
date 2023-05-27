using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TalaGrid.Models;

namespace TalaGrid.ViewModels
{
    public partial class NotificationsViewModel : ObservableObject
    {
        public NotificationsViewModel()
        {
            // Initialize your notifications collection with some sample data
            Notifications = new ObservableCollection<Notification>
            {
                new Notification { Title = "Notification 1", Message = "This is the first notification." },
                new Notification { Title = "Notification 2", Message = "This is the second notification." },
                new Notification { Title = "Notification 1", Message = "This is the first notification." },
                new Notification { Title = "Notification 2", Message = "This is the second notification." }
                // Add more notifications as needed
                
            };

            OldNotification = new Notification();
        }

        [ObservableProperty]
        private ObservableCollection<Notification> notifications;

        [ObservableProperty]
        private Notification currentNotification;

        [ObservableProperty]
        static private Notification oldNotification;


        public void SelectedItem(object sender, SelectedItemChangedEventArgs args)
        {

            // Reset the old notification object button visible property
            OldNotification.BtnVisible = false;
            // Initiate a new notification object
            CurrentNotification = args.SelectedItem as Notification;
            // Set the current notification button visibility property to true
            CurrentNotification.BtnVisible = true;
            // The old notification set to the current notification
            OldNotification = currentNotification;
        }
    }
}
