using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();

    }
}