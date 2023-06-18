using CommunityToolkit.Mvvm.ComponentModel;

// Create different users
// 1. Collector (No App Access)
// 2. Admin (Developer)
// 3. GW_Admin (Green Way Africa Admin)
// 4. BBC_Admin (Buy Back Center Admin)


namespace TalaGrid.Models
{
    public partial class Users : ObservableObject
    {
        public Users()
        {
            Email = "";
            CellNumber = "";
            Suburb = "";

            AdminRoleValue = new string[] { "Admin (Developer)", "GW_Admin (GreenWay Africa Admin)", "BBC_Admin (Buy Back Center Admin)" };

        }

        #region Class Properties
        //User's Database Id
        [ObservableProperty]
        private int id;

        //User's First Name
        [ObservableProperty]
        private string firstName;

        //User's Last name
        [ObservableProperty]
        private string lastName;

        //User's Id Number
        [ObservableProperty]
        private string idNumber;

        //User's Gender
        [ObservableProperty]
        private string gender;

        //User's Highest Qualification
        [ObservableProperty]
        private string highestQlfn;

        //User's Income Range
        [ObservableProperty]
        private string incomeRange;

        //User's Email Address
        [ObservableProperty]
        private string email;

        //User's Cell Number
        [ObservableProperty]
        private string cellNumber;

        //User's Street Address
        [ObservableProperty]
        private string streetAddress;

        //User's Suburb Name
        [ObservableProperty]
        private string suburb;

        //User's City Name
        [ObservableProperty]
        private string city;

        //User's Province Name
        [ObservableProperty]
        private string province;

        //User's Country Name
        [ObservableProperty]
        private string country;

        //Admin Role Value
        // 1. Admin (Developer)
        // 2. GW_Admin (GreenWay Africa Admin)
        // 3. BBC_Admin (Buy Back Center Admin)
        public string[] AdminRoleValue { get; }

        //Admin Role property
        [ObservableProperty]
        private string adminRole;

        //Verirfied Admin: GW_Admin must be verified by the Admin,
        //BBC_Admin must be verified by GW_Admin
        [ObservableProperty]
        private bool verifiedAdmin;

        //BuyBackCanter at which the user/collector account is created
        [ObservableProperty]
        private int bBCId;

        #endregion

    }
}
