using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using System.Collections.ObjectModel;


namespace TalaGrid.ViewModels
{
    public partial class CaptureBottlesViewModel : ObservableObject
    {
        #region Default Constructor
        public CaptureBottlesViewModel()
        {
            dataService = new DatabaseService();
            searchService = new SearchService();
            alerts = new AlertService();
            user = new Users();
            banker = new Banking();
            currentAdmin = new LoginViewModel();
            bottleData = new BottleDataSource();
            capturedBottles = new ObservableCollection<Bottles>();
            capturedWaste = new();

            buyBackCentre = new BuyBackCentre();

            wasteMaterialList = new();
            wasteMaterialData = new();

            selectedUser = "Collector";

            //GetBottles();
            //GetOtherWaste();

            Amount = 0.0M;
            CaptureBottleDisplay = true;
            PaymentsDisplay = !captureBottleDisplay;
            display_0 = display_1 = false;
            transactions = new Transaction();

            //Show Bottles list or Other Waste Material list
            ShowBottles = true;
            ShowOtherWaste = false;

        }

        #endregion

        #region Class Properties
        readonly DatabaseService dataService;
        readonly SearchService searchService;
        readonly AlertService alerts;

        //Collector
        [ObservableProperty]
        Users user;

        [ObservableProperty]
        Banking banker;

        [ObservableProperty]
        LoginViewModel currentAdmin;

        [ObservableProperty]
        ObservableCollection<Users> usersList;

        [ObservableProperty]
        string selectedUser;

        //Bottles saved in the database are represented with this variable
        [ObservableProperty]
        BottleDataSource bottleData;

        [ObservableProperty]
        WasteMaterial wasteMaterialData;

        [ObservableProperty]
        decimal wasteSize;

        [ObservableProperty]
        decimal wastePrice;


        //List of Other Waste Material
        [ObservableProperty]
        ObservableCollection<WasteMaterial> wasteMaterialList;

        //List of Bottles from the database
        [ObservableProperty]
        ObservableCollection<BottleDataSource> bottlesList;

        [ObservableProperty]
        OtherWaste wasteMaterial;

        //Represents a given bottle
        [ObservableProperty]
        Bottles bottle;

        //List of Bottle Id's
        [ObservableProperty]
        List<int> bottleIdList;

        //List of OtherWaste Id's
        List<int> otherWasteIdList;

        //Store the collected waste material in the list before submition to database
        [ObservableProperty]
        ObservableCollection<OtherWaste> capturedWaste;

        //Selected item from the captured list
        [ObservableProperty]
        OtherWaste capturedOtherWasteItem;

        //The quantity of bottles submitted by the Collector
        [ObservableProperty]
        int quantity;

        //Current Amount
        [ObservableProperty]
        decimal currentAmount;

        //The amount due to the collector
        [ObservableProperty]
        static decimal amount;

        [ObservableProperty]
        string amountString;

        //Store the collected bottles in the list before submition to database
        [ObservableProperty]
        ObservableCollection<Bottles> capturedBottles;

        //Selected item from the captured list
        [ObservableProperty]
        Bottles capturedBottleItem;

        //Switch Display Between capturing bottle data to payment section
        [ObservableProperty]
        bool captureBottleDisplay;

        //Used to select the Payment Method available to the user
        [ObservableProperty]
        bool paymentsDisplay;

        //Switch display between Bank/MobileMoney Payment methods
        [ObservableProperty]
        bool display_0;

        [ObservableProperty]
        bool display_1;

        [ObservableProperty]
        bool display_2;

        //Switch Display between Bottles and Other Waste material items
        [ObservableProperty]
        bool showOtherWaste;

        [ObservableProperty]
        bool showBottles;

        [ObservableProperty]
        Transaction transactions;

        BuyBackCentre buyBackCentre;

        #endregion

        #region Button Methods

        [RelayCommand]
        public async void Add_and_Calculate()
        {
            //Confirm if the Admin has registered a BuyBackCentre
            buyBackCentre = dataService.SearchBBC(currentAdmin.UserLogin.AdminId);

            if (buyBackCentre.BBCId == 0)
            {
                await alerts.ShowAlertAsync("Missing Information Detected", "The user is required to Update BuyBackCentre Details under Update User Account Tab and ReLogin");
                return;
            }

            //Confirm if a User is selected
            if (user.Id == 0)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Please Search and Select User");
                return;
            }

            //If Capturing Bottles
            if (showBottles)
            {
                if (CapturedWaste.Count > 0)
                    await alerts.ShowAlertAsync("Operation Failed", "One type of Waste Material can be captured at a time!");
                else
                    SaveCapturedBottles();

            }
            //If Capturing Other Waste Material
            else if (showOtherWaste)
            {
                if (capturedBottles.Count > 0)
                    await alerts.ShowAlertAsync("Operation Failed", "One type of Waste Material can be captured at a time!");
                else
                {
                    SaveCapturedOtherWaste();
                    WasteMaterialData.Size = 0.0M;
                    WasteMaterialData.Price = 0.0M;
                }
                
            }

            //Reset Bottle size and Quantity
            Clear(false);
        }

        /// <summary>
        /// Submit Captured Bottles to the database as a record and 
        /// alert the user of the outcome.
        /// </summary>
        [RelayCommand]
        public void Submit()
        {
            if (ShowBottles)
                SubmitCapturedBottles();
            else if (showOtherWaste)
                SubmitCapturedOtherWaste();
        }

        /// <summary>
        /// Submit the Transaction file showing the collected bottle information,
        /// Collector Details, and BuyBackCentre Details
        /// </summary>
        [RelayCommand]
        async public void SubmitTransaction()
        {
            //Set the transaction type
            if (display_0)   //If display is set to Banking Display
            {
                transactions.TransactionType = "Bank Payment";

                //Find the Banking details
                Banker = dataService.SearchBanking(user.Id);

                if (Banker != null)
                    transactions.BankDetailsId = banker.BankDetailsId;
                else
                    await alerts.ShowAlertAsync("Operation Failed", "No banking details associated with the collector were found!");
                
            }
            else if (display_1)      //If display is set to MobileMoney Display
            {
                if(user.CellNumber != null)
                    transactions.TransactionType = "Mobile Money Payment";
                else
                    await alerts.ShowAlertAsync("Operation Failed", "No cellphone number associated with the collector was found!");
            }
            else if (display_2)
                transactions.TransactionType = "Cash Payment";

            //Set the Date and Time to the current computer time
            transactions.LocalDate = DateTime.Now;

            //Set Operation Hours
           /* if (transactions.LocalDate.Hour < 6 || transactions.LocalDate.Hour > 18)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Time out of business hours");
                return;
            }
           */

            if(transactions.TransactionType == String.Empty)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Transaction type must be selected");
                return;
            }

            //Save the collectorId and AdminId with the transaction
            transactions.AdminId = buyBackCentre.AdminId;
            transactions.CollectorId = user.Id;

            //Which type of waste material is being captured
            if (showBottles)            //Bottles
            {
                transactions.WasteMaterialType = "Bottles";
                MaterialTransactionId(BottleIdList);
            }
            else if (showOtherWaste)    //Other Waste material
            {
                transactions.WasteMaterialType = "Other Waste Material";
                MaterialTransactionId(otherWasteIdList);
            }

            

        }

        [RelayCommand]
        public async void DeleteItem()
        {
            if(showBottles)
            {
                if (capturedBottleItem != null)
                {
                    Amount -= capturedBottleItem.Amount;
                    AmountString = $"R{Amount}";
                    CapturedBottles.Remove(capturedBottleItem);
                }
                else
                    await alerts.ShowAlertAsync("Error!", "Select an item to delete from the list");
            }
            else if (showOtherWaste)
            {
                if (capturedOtherWasteItem != null)
                {
                    Amount -= capturedOtherWasteItem.Amount;
                    AmountString = $"R{Amount}";
                    CapturedWaste.Remove(capturedOtherWasteItem);
                }
                else
                    await alerts.ShowAlertAsync("Error!", "Select an item to delete from the list");
            }

        }

        [RelayCommand]
        public async void DeleteAllItems()
        {
            bool answer = false;

            if (capturedBottleItem != null || capturedOtherWasteItem != null)
                answer = await alerts.ShowConfirmationAsync("Confirmation", "Are you sure you want to delete all items", "Yes", "No");
            else
                await alerts.ShowAlertAsync("Error!", "Select an item to delete from the list");

            if (answer)
            {

                Amount = 0.0M;
                AmountString = $"R{Amount}";

                if (showBottles)
                    CapturedBottles.Clear();
                else if (showOtherWaste)
                    CapturedWaste.Clear();
            }
            else
                return;
            
        }

        [RelayCommand]
        public void GoBack()
        {
            PaymentsDisplay = false;
            CaptureBottleDisplay = true;
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// The selectedItem method updates the User object
        /// with the selected user from the ListView
        /// </summary>
        public void SelectedItem(object sender, SelectedItemChangedEventArgs args)
        {
            //The collector selected from the Listview
            User = args.SelectedItem as Users;
        }

        /// <summary>
        /// The SelectedBottle method updates the BottleData object with
        /// the chosen bottle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SelectedBottle(object sender, SelectedItemChangedEventArgs args)
        {
            BottleData = args.SelectedItem as BottleDataSource;
        }

        public void SelectedWaste(object sender, SelectedItemChangedEventArgs args)
        {
            WasteMaterialData = args.SelectedItem as WasteMaterial;
        }


        public void GetBottles(string name)
        {
            BottlesList = new ObservableCollection<BottleDataSource>(dataService.GetBottleList(name));
        }

        public void GetOtherWaste(string name)
        {
            WasteMaterialList = new ObservableCollection<WasteMaterial>(dataService.GetOtherWasteList(name));
        }

        private void BottleAmount()
        {
            string bottleSize;

            if (bottleData.Size != null)
            {
                bottleSize = bottleData.Size.Replace("ml", string.Empty);
                bottleSize = bottleSize.Trim();

                int size = Int32.Parse(bottleSize);

                if (size > 0 && size < 2000)
                    CurrentAmount = (decimal)Quantity;
                else if (size >= 2000)
                    CurrentAmount = (decimal)(Quantity * 1.5);

                Amount += Math.Round((decimal)CurrentAmount, 3);
                AmountString = $"R{amount}";

            }
            else
            {
                alerts.ShowAlertAsync("Operation Failed", "Select a bottle name from the list and enter quantity value");
                return;
            }
        }

        private void WasteAmount()
        {
            if (wasteMaterialData.MaterialName != null)
            {
                //Calculate current amount of the selected waste material
                CurrentAmount = wasteSize * wastePrice;

                Amount += Math.Round((decimal)CurrentAmount, 3);
                AmountString = $"R{amount}";
            }
            else
            {
                alerts.ShowAlertAsync("Operation Failed", "Select a waste material name from the list and enter size (in Kg) and Price (per Kg)");
            }
        }

        /// <summary>
        /// Show Banking Details for the current User/Collector for confirmation
        /// </summary>
        public void UpdateBanker()
        {
            if (user.Id != 0)
            {
                //Search the database for the user's banking details
                Banker = dataService.SearchBanking(user.Id);

            }
        }

        private void Clear(bool clearAll)
        {
            if (clearAll)
            {
                Amount = 0.0M;
                AmountString = $"R{amount}";
                CapturedBottles.Clear();
                CapturedWaste.Clear();
                WasteMaterialData.Size = 0.0M;
                WasteMaterialData.Price = 0.0M;
            }

            Quantity = 0;
        }

        public void SwitchDisplay(bool captureBottles)
        {
            //Switch between the CaptureBottle Display and Payment Display in the CaptureNewBottlesView
            CaptureBottleDisplay = captureBottles;
            PaymentsDisplay = !captureBottleDisplay;
        }

        private async void SaveCapturedBottles()
        {
            if (quantity == 0)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Quantity Cannot be Zero");
                return;
            }
            //Update the Bottle Object
            else if (bottleData.BottleDataSourceId != 0)
            {
                bottle = new Bottles();
                //Calculate amount due
                BottleAmount();

                Bottle.BottleName = BottleData.BottleName;
                Bottle.Quantity = quantity;
                Bottle.BottleDataSourceId = BottleData.BottleDataSourceId;
                Bottle.CollectorId = user.Id;
                Bottle.BBCId = buyBackCentre.BBCId;
                Bottle.Amount = currentAmount;
                Bottle.AdminId = buyBackCentre.AdminId;

                //Update and Display Captured Bottles
                capturedBottles.Insert(0, Bottle);

            }
            else
                await alerts.ShowAlertAsync("Operation Failed", "Botlles were not captured correctly.");
        }

        private async void SaveCapturedOtherWaste()
        {
            if (wasteSize == 0)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Size Cannot be Zero");
                return;
            }

            if (wastePrice == 0)
            {
                await alerts.ShowAlertAsync("Operation Failed", "Price Cannot be Zero");
                return;
            }

            if (wasteMaterialData.MaterialName != null)
            {
                wasteMaterial = new();

                //Calculate the amount due
                WasteAmount();

                //Update the Object
                WasteMaterial.MaterialName = wasteMaterialData.MaterialName;
                WasteMaterial.Price = wastePrice;
                WasteMaterial.Size = wasteSize;
                WasteMaterial.CollectorId = user.Id;
                WasteMaterial.BBCId = currentAdmin.UserLogin.BBCId;
                WasteMaterial.Amount = currentAmount;
                WasteMaterial.AdminId = currentAdmin.UserLogin.AdminId;

                capturedWaste.Insert(0, wasteMaterial);

            }
            else
                await alerts.ShowAlertAsync("Operation Failed", "Material name must be selected from the list");
        }

        private async void SubmitCapturedBottles()
        {
            bool isSubmitted = false;

            if (capturedBottles.Count > 0)
            {
                bottleIdList = new List<int>();

                foreach (Bottles bottle in capturedBottles)
                {
                    isSubmitted = dataService.CaptureBottles(bottle);

                    if (!isSubmitted)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "Couldn't save data successfully, something went wrong");
                    }
                    else
                        BottleIdList.Insert(0, dataService.GetBottleId(bottle));
                }

                if (isSubmitted)
                {
                    await alerts.ShowAlertAsync("Operation Successful", "Collected bottle(s) data saved successfully!");

                    //Switch to the Payment Display 
                    SwitchDisplay(false);

                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "Couldn't save data successfully, something went wrong");
            }
            else
                await alerts.ShowAlertAsync("Operation Failed", "Bottle data was not captured succesfully, please try again!!");
        }

        private async void SubmitCapturedOtherWaste()
        {
            bool isSubmitted = false;

            if (capturedWaste.Count > 0)
            {
                otherWasteIdList = new List<int>();

                foreach (OtherWaste waste in capturedWaste)
                {
                    isSubmitted = dataService.CaptureOtherWaste(waste);

                    if (!isSubmitted)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "Couldn't save data successfully, something went wrong");
                    }
                    else
                        otherWasteIdList.Insert(0, dataService.GetOtherWasteId(waste));
                }

                if (isSubmitted)
                {
                    await alerts.ShowAlertAsync("Operation Successful", "Collected waste material data saved successfully!");

                    //Switch to the Payment Display 
                    SwitchDisplay(false);

                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "Couldn't save data successfully, something went wrong");
            }
            else
                await alerts.ShowAlertAsync("Operation Failed", "Other Waste data was not captured succesfully, please try again!!");
        }

        private async void MaterialTransactionId(List<int> IdList)
        {
            bool isSaved = false;

            if (IdList == null)
            {
                await alerts.ShowAlertAsync("Operation Failed", "No data were captured, please capture waste material first");
                return;
            }
            else
            {
                foreach (int id in IdList)
                {
                    transactions.WasteMaterialId = id;
                    isSaved = dataService.TransRecord(transactions);

                    if (!isSaved)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "One or more transaction records could not be saved");
                        return;
                    }
                }

                if (isSaved)
                {
                    await alerts.ShowAlertAsync("Operation Successful", $"Captured Data Transaction Record Saved Successfully on {transactions.LocalDate}");

                    //Switch to the CaptureBottle Display 
                    SwitchDisplay(true);

                    Clear(true);
                }

            }
        }

   

        #endregion

    }




}
