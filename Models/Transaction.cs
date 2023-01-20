using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.Models
{
    public partial class Transaction : ObservableObject
    {
        public Transaction()
        {
            signature = new byte[0];
        }

        [ObservableProperty]
        string transactionType;

        // LocalDate to store the Local computer Date and Time
        [ObservableProperty]
        DateTime localDate;

        [ObservableProperty]
        int wasteMaterialId;

        [ObservableProperty]
        int bankDetailsId;

        //Signature image
        [ObservableProperty]
        byte[] signature;
    }
}
