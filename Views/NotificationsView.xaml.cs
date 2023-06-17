using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class NotificationsView : ContentPage
{
	NotificationsViewModel viewModel;

    public NotificationsView()
	{
		InitializeComponent();
		viewModel = new NotificationsViewModel();

		BindingContext = viewModel;

        // Update Flyout Views
        HiddenViews();
	}

	private void notificationsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		viewModel.SelectedItem(sender, e);
    }

    public bool HideView { get; set; }

    private void HiddenViews()
    {
        if (viewModel.Admin.AdminRole == viewModel.Admin.AdminRoleValue[2])
            HideView = false;
        else
            HideView = true;
    }
}