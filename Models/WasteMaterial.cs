using CommunityToolkit.Mvvm.ComponentModel;


namespace TalaGrid.Models
{
    public partial class WasteMaterial : ObservableObject
    {
        public WasteMaterial()
        {

        }

        [ObservableProperty]
        int wasteMaterialId;

        [ObservableProperty]
        string materialName;

        [ObservableProperty]
        double size;

        [ObservableProperty]
        double price;
    }
}
