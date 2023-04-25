using CommunityToolkit.Mvvm.ComponentModel;


namespace TalaGrid.Models
{
    public partial class OtherWaste : ObservableObject
    {
        [ObservableProperty]
        string materialName;

        [ObservableProperty]
        decimal size;

        [ObservableProperty]
        decimal price;

        [ObservableProperty]
        int collectorId;

        [ObservableProperty]
        int bBCId;

        [ObservableProperty]
        decimal amount;

        [ObservableProperty]
        static int adminId;
    }
}
