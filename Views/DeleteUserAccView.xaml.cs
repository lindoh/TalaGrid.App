
using TalaGrid.Services;
using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class DeleteUserAccView : ContentPage
{
    public DeleteUserAccView()
    {
        InitializeComponent();
        viewModel = new DeleteUserAccViewModel();
        BindingContext = viewModel;
        viewModel.SelectedUser = "Collector";
        searchService = new SearchService();
    }

    SearchService searchService;
    DeleteUserAccViewModel viewModel;

    private void usersListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.selectedItem(sender, args);
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        viewModel.UsersList = searchService.FindUser(searchBar.Text, viewModel.SelectedUser);
    }
}