using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TalaGrid.Models;

namespace TalaGrid.ViewModels
{
    public partial class NotificationsViewModel : ObservableObject
    {
        public NotificationsViewModel()
        {
            NewIndex = OldIndex = 0;

            // Initialize your notifications collection with some sample data
            Notifications = new ObservableCollection<Notification>
            {
                new Notification { Title = "Notification 1", Message = "This is the first notification." },
                new Notification { Title = "Notification 2", Message = "This is the second notification." },
                // Add more notifications as needed
                
            };
        }

        [ObservableProperty]
        private ObservableCollection<Notification> notifications;

        [ObservableProperty]
        private Notification notificationObject;

        [ObservableProperty]
        private int newIndex;

        [ObservableProperty]
        private int oldIndex;



        public void SelectedItem(object sender, SelectedItemChangedEventArgs args)
        {
            //The collector selected from the Listview
            NotificationObject = args.SelectedItem as Notification;
            NewIndex = args.SelectedItemIndex;

            if (newIndex != oldIndex) 
            { 
                NotificationObject.BtnVisible = true;
                OldIndex = newIndex;
            }
            else 
            { 
                NotificationObject.BtnVisible = false;
            }
        }
    }
}
