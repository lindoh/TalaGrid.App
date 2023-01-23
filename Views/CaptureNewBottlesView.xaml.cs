using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.ViewModels;

namespace TalaGrid.Views;

public partial class CaptureNewBottlesView : ContentPage
{
    public CaptureNewBottlesView()
    {
        InitializeComponent();
        viewModel = new CaptureBottlesViewModel();
        BindingContext = viewModel;
        alerts = new AlertService();
    }

    AlertService alerts;

    CaptureBottlesViewModel viewModel;

    /// <summary>
    /// Select a User from the ListView List and update the ViewModel selected user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void usersListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.selectedItem(sender, args);
    }

    private void bottlesListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.selectedBottle(sender, args);
    }

    private void wasteListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.selectedWaste(sender, args);
    }

    /// <summary>
    /// Update the Payment Method as Choosen by the user from the Radio buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PayMethodRadioBtn_CheckedChanged(object sender, CheckedChangedEventArgs args)
    {
        if (BankPaymentRadioBtn.IsChecked)
        {
            viewModel.Display_0 = true;
            viewModel.Display_1 = viewModel.Display_2 = !viewModel.Display_0;

            //Show Updated Banking Details
            viewModel.UpdateBanker();
        }
        else if (MobilePaymentRadioBtn.IsChecked)
        {
            viewModel.Display_1 = true;
            viewModel.Display_0 = viewModel.Display_2 = !viewModel.Display_1;
        }
        else if (CashPaymentRadioBtn.IsChecked)
        {
            viewModel.Display_2 = true;
            viewModel.Display_0 = viewModel.Display_1 = !viewModel.Display_2;
        }
    }

    /// <summary>
    /// Sign Button Clicked Saves the signature as a byte image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_Clicked(object sender, EventArgs e)
    {
        using var stream = await SignatureDrawing.GetImageStream(1024, 1024);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        stream.Position = 0;
        memoryStream.Position = 0;

        byte[] imageFile = memoryStream.ToArray();
        viewModel.Transactions.Signature = memoryStream.ToArray();

        if (viewModel.Transactions.Signature != null)
            await alerts.ShowAlertAsync("Success", "Signature saved successfully");
        else
            await alerts.ShowAlertAsync("Failure", "System could not save Signature");

    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (ClearCheckBox.IsChecked)
            SignatureDrawing.Clear();
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BottleRadioBtn.IsChecked)
        {
            viewModel.ShowBottles = true;
            viewModel.ShowOtherWaste = false;
        }
        else if (OtherWasteRadioBtn.IsChecked)
        {
            viewModel.ShowOtherWaste = true;
            viewModel.ShowBottles = false;
        }
    }

    private void CapturedBottleList_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.CapturedBottleItem = args.SelectedItem as Bottles;
    }

    private void CapturedWasteList_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
        viewModel.CapturedOtherWasteItem = args.SelectedItem as OtherWaste;
    }
}