using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class BuyBackCentre : ObservableObject
    {
        public BuyBackCentre()
        {

        }

        [ObservableProperty]
        private int bBCId;

        [ObservableProperty]
        private string buyBackCentreName;

        [ObservableProperty]
        int adminId;

        //BBC Street Address
        [ObservableProperty]
        private string streetAddress;

        //BBC Suburb Name
        [ObservableProperty]
        private string suburb;

        //BBC City Name
        [ObservableProperty]
        private string city;

        //BBC Province Name
        [ObservableProperty]
        private string province;

        //BBC Country Name
        [ObservableProperty]
        private string country;
    }
}
