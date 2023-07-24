using TalaGrid.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TalaGrid.Services;

namespace TalaGrid.ViewModels
{
    public partial class ReportingViewModel : ObservableObject
    {
        #region Default Constructor
        public ReportingViewModel()
        {
            currentReport = new();
            dataService = new();
        }

        #endregion

        #region Class Properties
        DatabaseService dataService;
        

        [ObservableProperty]
        private Report currentReport;

        //[ObservableProperty]
        //private ObservableCollection<Users> collectorsList;

        //[ObservableProperty]
        //private ObservableCollection<BottleDataSource> bottlesList;

        //[ObservableProperty]
        //private ObservableCollection<WasteMaterial> wasteMaterialList;

        //[ObservableProperty]
        //private BottleDataSource bottle;

        //[ObservableProperty]
        //private WasteMaterial wasteMaterial;

        [ObservableProperty]
        private ObservableCollection<string> bBCList;

        [ObservableProperty]
        private ObservableCollection<string> citiesList;

        //[ObservableProperty]
        //private Users currentCollector;

        //[ObservableProperty]
        //private bool showBottles;

        //[ObservableProperty]
        //private bool showOtherWaste;

        #endregion

        #region Helper Methods
        public void GetAllBBC(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            BBCList = new ObservableCollection<string>(dataService.GetAllBBC(searchBar.Text));
        }
        //public void GetBottles(string name)
        //{
        //    BottlesList = new ObservableCollection<BottleDataSource>(dataService.GetBottleList(name));
        //}

        //public void GetOtherWaste(string name)
        //{
        //    WasteMaterialList = new ObservableCollection<WasteMaterial>(dataService.GetOtherWasteList(name));
        //}

        //public void SelectedBottle(object sender, SelectedItemChangedEventArgs args)
        //{
        //    Bottle = args.SelectedItem as BottleDataSource;
        //}

        //public void SelectedWaste(object sender, SelectedItemChangedEventArgs args)
        //{
        //    WasteMaterial = args.SelectedItem as WasteMaterial;
        //}

        #endregion
    }
}
