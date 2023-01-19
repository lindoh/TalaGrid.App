using CommunityToolkit.Mvvm.ComponentModel;


namespace TalaGrid.Models
{
    public partial class OtherWaste : ObservableObject
    {
        [ObservableProperty]
        string materialName;

        [ObservableProperty]
        double size;

        [ObservableProperty]
        double price;

        [ObservableProperty]
        int collectorId;

        [ObservableProperty]
        int bBCId;

        [ObservableProperty]
        double amount;

        [ObservableProperty]
        static int adminId;
    }
}
