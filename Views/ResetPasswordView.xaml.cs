using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class ResetPasswordView : ContentPage
{
    public ResetPasswordView()
    {
        InitializeComponent();
        BindingContext = new ManagePasswordViewModel();
    }
}