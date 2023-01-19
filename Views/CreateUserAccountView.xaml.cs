using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class CreateUserAccountView : ContentPage
{

    public CreateUserAccountView()
    {
        InitializeComponent();
        BindingContext = new CreateUserAccViewModel();
    }

}