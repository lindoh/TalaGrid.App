using Microsoft.Maui.Controls;
using System.Drawing;
using TalaGrid.ViewModels;
using Color = Microsoft.Maui.Graphics.Color;

namespace TalaGrid.Views;

public partial class NotificationsView : ContentPage
{
	NotificationsViewModel viewModel;
	public NotificationsView()
	{
		InitializeComponent();
		viewModel = new NotificationsViewModel();

		BindingContext = viewModel;
	}

	private void notificationsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		viewModel.SelectedItem(sender, e);
    }
}