using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class BottleDataSource : ObservableObject
    {
        public BottleDataSource()
        {

        }

        [ObservableProperty]
        int bottleDataSourceId;

        [ObservableProperty]
        string bottleName;

        [ObservableProperty]
        string size;
    }
}
