
namespace TalaGrid.Services
{
    public interface IAlertService
    {
        //async calls (use with "await" - must be on dispatcher thread---
        Task ShowAlertAsync(string title, string message, string cancel = "OK");
        Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");

        //"Fire and Forget" calls
        void ShowAlert(string title, string message, string cancel = "OK");
        // <param, name="callback">Action to perform afterwards</param>
        void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No");

    }
}
