using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class ManagePasswordView : ContentPage
{
    public ManagePasswordView()
    {
        InitializeComponent();
        BindingContext = new ManagePasswordViewModel();
    }
}