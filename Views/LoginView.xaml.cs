using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }



}