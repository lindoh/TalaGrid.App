using TalaGrid.Views;

namespace TalaGrid;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(CaptureNewBottlesView), typeof(CaptureNewBottlesView));
        Routing.RegisterRoute(nameof(CreateLoginsView), typeof(CreateLoginsView));
        Routing.RegisterRoute(nameof(CreateUserAccountView), typeof(CreateUserAccountView));
        Routing.RegisterRoute(nameof(DeleteUserAccView), typeof(DeleteUserAccView));
        Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
        Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
        Routing.RegisterRoute(nameof(LogoutView), typeof(LogoutView));
        Routing.RegisterRoute(nameof(ManagePasswordView), typeof(ManagePasswordView));
        Routing.RegisterRoute(nameof(RegistrationView), typeof(RegistrationView));
        Routing.RegisterRoute(nameof(ResetPasswordView), typeof(ResetPasswordView));
        Routing.RegisterRoute(nameof(UpdateBankingView), typeof(UpdateBankingView));
        Routing.RegisterRoute(nameof(UpdateUserAccountView), typeof(UpdateUserAccountView));
    }
}
