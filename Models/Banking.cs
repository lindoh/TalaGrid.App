using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class Banking : ObservableObject
    {
        #region Default Constructor
        public Banking()
        {

        }
        #endregion

        #region Class Properties
        //User's BankingDetailsId
        [ObservableProperty]
        private int bankDetailsId;

        //User's Bank name
        [ObservableProperty]
        private string bankName;

        //User's Branch name
        [ObservableProperty]
        private string branchName;

        //User's Branch code
        [ObservableProperty]
        private string branchCode;

        //User's Account Type
        [ObservableProperty]
        private string accountType;

        //User's Account number
        [ObservableProperty]
        private string accountNumber;

        #endregion
    }
}
