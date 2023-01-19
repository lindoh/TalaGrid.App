using TalaGrid.ViewModels;
using TalaGrid.Views;

namespace TalaGrid;

public partial class App : Application
{
    public App (LoginViewModel viewModel)
    {
        InitializeComponent();
        viewModel = new LoginViewModel();

        if (!viewModel.UserLogin.IsLoggedIn)
            MainPage = new LoginView();
        else
            MainPage = new AppShell();

    }
}
