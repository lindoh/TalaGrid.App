using TalaGrid.Services;
using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class UpdateBankingView : ContentPage
{
    public UpdateBankingView()
    {
        InitializeComponent();
        viewModel = new UpdateBankingViewModel();
        BindingContext = viewModel;
        searchService = new SearchService();
    }

    SearchService searchService;
    UpdateBankingViewModel viewModel;


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