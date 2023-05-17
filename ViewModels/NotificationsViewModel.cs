using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TalaGrid.Models;

namespace TalaGrid.ViewModels
{
    public partial class NotificationsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Notification> notifications;
        public NotificationsViewModel() 
        {
            // Initialize your notifications collection with some sample data
            Notifications = new ObservableCollection<Notification>
            {
                new Notification { Title = "Notification 1", Message = "This is the first notification." },
                new Notification { Title = "Notification 2", Message = "This is the second notification." },
                // Add more notifications as needed
            };
        }
    }
}
