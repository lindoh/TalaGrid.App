using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class Login : ObservableObject
    {
        public Login()
        {

        }

        //Username can be an Email or any combination of text
        [ObservableProperty]
        static string username;

        [ObservableProperty]
        static string password;

        [ObservableProperty]
        static bool isLoggedIn;

        [ObservableProperty]
        static bool isBBCUpdated;

        [ObservableProperty]
        static int adminId;

        [ObservableProperty]
        static int bBCId;
    }


}
