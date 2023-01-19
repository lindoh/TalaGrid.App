using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class LogoutView : ContentPage
{
    public LogoutView()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}