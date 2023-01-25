using TalaGrid.Services;
using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class UpdateUserAccountView : ContentPage
{
    UpdateUserAccViewModel viewModel;
    SearchService searchService;

    public UpdateUserAccountView()
    {
        InitializeComponent();
        viewModel = new();
        BindingContext = viewModel;
        searchService = new SearchService();
    }
    private void usersListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.selectedItem(sender, args);
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs args)
    {
        if (AdminRadioBtn.IsChecked)
        {
            viewModel.SelectedUser = "Admin";
            viewModel.ShowBBCSection = true;
        }
        else if (CollectorRadioBtn.IsChecked)
        {
            viewModel.SelectedUser = "Collector";
            viewModel.ShowBBCSection = false;
        }
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        viewModel.UsersList = searchService.FindUser(searchBar.Text, viewModel.SelectedUser);
    }
}