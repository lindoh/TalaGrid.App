
using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class ReportingView : ContentPage
{

	public ReportingView()
	{
		InitializeComponent();
        
		reportingViewModel = new ReportingViewModel();
        BindingContext = reportingViewModel;

        searchService = new SearchService();
	}

    string SelectedUser = "Collector";

    SearchService searchService;
    ReportingViewModel reportingViewModel;

    private void Transaction_RadioBtn_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        reportingViewModel.CurrentReport.IsEnabled_BBC = BBC_RadioBtn.IsChecked;
        reportingViewModel.CurrentReport.IsEnabled_Col = Collectors_RadioBtn.IsChecked;

        if (Transaction_RadioBtn.IsChecked)
		{
			reportingViewModel.CurrentReport.ReportType = "Transaction Report";
		}
		else if (BBC_RadioBtn.IsChecked)
		{
            reportingViewModel.CurrentReport.ReportType = "Buy-Back-Center Report";
        }
		else if (Collectors_RadioBtn.IsChecked)
		{
            reportingViewModel.CurrentReport.ReportType = "Collectors Report";
        }
		else
		{
            reportingViewModel.CurrentReport.ReportType = "System Report";
        }
    }

    private void Bottles_RadioBtn_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        reportingViewModel.CurrentReport.BottlesWaste = Bottles_RadioBtn.IsChecked;
        reportingViewModel.CurrentReport.OtherWaste = OtherWaste_RadioBtn.IsChecked;
    }

    //private void searchBar_Collector_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    SearchBar searchBar = (SearchBar)sender;
    //    reportingViewModel.CollectorsList = searchService.FindUser(searchBar.Text, SelectedUser);
    //}

    /// <summary>
    /// Returns the selected Collector from the List View
    /// </summary>
    //private void collectorsListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    //{
    //    reportingViewModel.CurrentCollector = args.SelectedItem as Users;
    //}

    private void searchBar_BBC_TextChanged(object sender, TextChangedEventArgs e)
    {
        
    }

    private void BBCListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }

    private void searchBar_City_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void CityListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }

    /// <summary>
    /// Search Waster material from the Database
    /// </summary>
    //private void searchBar_Waste_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    SearchBar searchBar = (SearchBar)sender;

    //    if (Bottles_RadioBtn.IsChecked)
    //    {
    //        reportingViewModel.GetBottles(searchBar.Text);
    //    }
    //    else if (OtherWaste_RadioBtn.IsChecked)
    //    {
    //        reportingViewModel.GetOtherWaste(searchBar.Text);
    //    }

    //    reportingViewModel.ShowBottles = Bottles_RadioBtn.IsChecked;
    //    reportingViewModel.ShowOtherWaste = OtherWaste_RadioBtn.IsChecked;
    //}

    //private void bottleListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    //{
    //    reportingViewModel.SelectedBottle(sender, args);
    //}

    //private void wasteListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    //{
    //    reportingViewModel.SelectedWaste(sender, args);
    //}
}