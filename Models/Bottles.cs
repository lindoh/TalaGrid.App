using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class Bottles : ObservableObject
    {
        [ObservableProperty]
        string bottleName;

        [ObservableProperty]
        int quantity;

        [ObservableProperty]
        int bottleDataSourceId;

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
