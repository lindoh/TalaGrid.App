using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class CreateLoginsView : ContentPage
{
    public CreateLoginsView()
    {
        InitializeComponent();
        BindingContext = new CreateLoginsViewModel();
    }
}