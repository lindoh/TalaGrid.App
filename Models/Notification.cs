using CommunityToolkit.Mvvm.ComponentModel;


namespace TalaGrid.Models
{
    public partial class Notification : ObservableObject
    {
        [ObservableProperty]
        public string title;

        [ObservableProperty]
        public string message;
    }
}
