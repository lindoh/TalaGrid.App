using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class RegistrationView : ContentPage
{
    public RegistrationView()
    {
        InitializeComponent();
        BindingContext = new RegistrationViewModel();
    }
}