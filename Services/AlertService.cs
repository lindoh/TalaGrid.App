

namespace TalaGrid.Services
{
    internal class AlertService : IAlertService
    {

        //Async calls (use with "await" - Must be on dispatcher thread---
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        // ---- "Fire and Forget" calls
        /// <summary>
        /// Fire and Forget method returns before showing alert
        /// </summary>

        public void ShowAlert(string title, string message, string cancel = "OK")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            await ShowAlertAsync(title, message, cancel));
        }

        /// <summary>
        /// Fire and Forget method returns before showing alert
        /// </summary>
        /// <param name="callback">Action to perform afaterwards</param>

        public void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(answer);
            });

        }
    }
}
