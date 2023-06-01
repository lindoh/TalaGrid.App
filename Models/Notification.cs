using CommunityToolkit.Mvvm.ComponentModel;


namespace TalaGrid.Models
{
    public partial class Notification : ObservableObject
    {
        public Notification()
        {
            user = new();
            admin =  new();
            BackColor = new();
            BackColor = Color.FromRgba("#dce0e6");
        }

        [ObservableProperty]
        private int notificationsId;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private bool btnVisible;

        [ObservableProperty]
        private Users user;

        [ObservableProperty]
        private Users admin;

        // Read flag is true when the notification has been read
        [ObservableProperty]
        private bool read;

        [ObservableProperty]
        private Color backColor;

    }
}
