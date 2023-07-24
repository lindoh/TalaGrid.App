using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class Report : ObservableObject
    {
        #region Default Constructor
        public Report()
        {
            // Initialize the startDate and endDate with the 
            // minimal possible and latest possible dates, respectively.
            StartDate = DateOnly.MinValue; EndDate = DateOnly.MaxValue;
            RegStartDate = DateOnly.MinValue; RegEndDate = DateOnly.MaxValue;

            // Initialize all other class properties
            ReportType = string.Empty;
            BottlesWaste = false;
            OtherWaste = false;
            CollectorName = string.Empty;
            BBCName = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            PaymentMethod = string.Empty;
            IsEnabled_Col = IsEnabled_BBC = false;
            IncomeMin = IncomeMax = 0.0;
            PayedOutAmountMin = PayedOutAmountMax = 0.0;
            CollectorsMin = CollectorsMax = 0;
            
        }

        #endregion

        #region Class Properties

        // Applicable to All reports viz, Transaction, Collector, and BBC report
        [ObservableProperty]
        private string reportId;

        [ObservableProperty]
        private string reportType;

        // Report Period start and end date 
        [ObservableProperty]
        private DateOnly startDate;  //Date format is mmddyyyy

        [ObservableProperty]
        private DateOnly endDate;

        [ObservableProperty]
        private bool bottlesWaste;

        [ObservableProperty]
        private bool otherWaste;

        [ObservableProperty]
        private string collectorName;

        [ObservableProperty]
        private string bBCName;

        [ObservableProperty]
        private string province;

        [ObservableProperty]
        private string city;

        [ObservableProperty]
        private string paymentMethod;

        // Only Applicable to Collector Report
        [ObservableProperty]
        private bool isEnabled_Col;

        // Registration start and end date
        [ObservableProperty]
        private DateOnly regStartDate;

        [ObservableProperty]
        private DateOnly regEndDate;

        [ObservableProperty]
        private double incomeMin;

        [ObservableProperty]
        private double incomeMax;

        // Only applicable to BBC report
        [ObservableProperty]
        private bool isEnabled_BBC;

        [ObservableProperty]
        private double payedOutAmountMin;

        [ObservableProperty]
        private double payedOutAmountMax;

        [ObservableProperty]
        private int collectorsMin;

        [ObservableProperty]
        private int collectorsMax;

        #endregion
    }
}
