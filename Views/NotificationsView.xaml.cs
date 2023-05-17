using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class NotificationsView : ContentPage
{
	public NotificationsView()
	{
		InitializeComponent();

		BindingContext = new NotificationsViewModel();
	}
}