using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TalaGrid.Models;
using TalaGrid.Services;
using TalaGrid.Views;
using System.Text;


namespace TalaGrid.ViewModels
{
    public partial class ManagePasswordViewModel : ObservableObject
    {
        public ManagePasswordViewModel()
        {
            dataService = new DatabaseService();
            alerts = new AlertService();
            createLoginsVM = new CreateLoginsViewModel();
            login = new LoginViewModel();
            email = new EmailService();

            Reset_or_Update_Password();
        }

        [ObservableProperty]
        string oldPassword;

        [ObservableProperty]
        string newPassword;

        [ObservableProperty]
        string confirmPassword;

        [ObservableProperty]
        string oneTimePin;

        [ObservableProperty]
        string userOTP;

        DatabaseService dataService;

        AlertService alerts;

        [ObservableProperty]
        LoginViewModel login;

        CreateLoginsViewModel createLoginsVM;

        EmailService email;

        [ObservableProperty]
        string idNumber;

        //If false it means we are reseting password, else, it's an update operation
        [ObservableProperty]
        bool reset_Update_Password;

        #region Class Buttons

        [RelayCommand]
        async void Update()
        {
            //If user updates password return true, else false move to validate password for Password reset
            if (reset_Update_Password)
            {
                    //Confirm Old Password
                    if (oldPassword == null || NewPassword == null || confirmPassword == null)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "One or more empty fields found");
                        return;
                    }

                    if (login.UserLogin.Password != oldPassword)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "The provided Old Password is invalid!");
                        return;
                    }


                    if (newPassword == oldPassword)
                    {
                        await alerts.ShowAlertAsync("Operation Failed", "Your new password cannot be similar to your old password");
                        return;
                    }
            }


            //Validate password, Password Reset
            if (!createLoginsVM.ValidatePassword(NewPassword))
                await alerts.ShowAlertAsync("Invalid Password", "Please check Password Guidlines Highlighted in red below!");
            else if (!(NewPassword == confirmPassword))
                await alerts.ShowAlertAsync("Operation Failed", "Passwords do not match");
            else
            {
                bool isUpdated = dataService.UpdatePassword(login.UserLogin.AdminId, newPassword);

                if (isUpdated)
                {
                    await alerts.ShowAlertAsync("Operation Successful", "Password Updated Successfully");

                    //Navigate to the Login Page
                    App.Current.MainPage = new LoginView();
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "The password could not be updated!");
            }

            Clear();
        }

        [RelayCommand]
        async void Continue()
        {
            if (userOTP != null)
            {
                if (userOTP == oneTimePin)
                {
                    await alerts.ShowAlertAsync("Success", "Head to the next page to create a new Password");
                    App.Current.MainPage = new ManagePasswordView();
                }
                else
                    await alerts.ShowAlertAsync("Operation Failed", "Type in a correct OTP and click Generate New OTP");
            }
            else
                await alerts.ShowAlertAsync("Operation Failed", "Type in a correct OTP and click Generate New OTP");
        }

        [RelayCommand]
        async void GenerateNewOTP()
        {
           // TakePhoto();


            //If The Generate OTP Button is pressed it means we are resetting Password
            
            Reset_Update_Password = false;

            Users user = new Users();

            user = dataService.SearchAdmin(idNumber);

            if (user.Id == 0 || user.Email == null)
                await alerts.ShowAlertAsync("Operation Failed", "User is not recognized, please enter a valid Id number");
            else
            {
                GenerateOTP(user);
                await alerts.ShowAlertAsync("Success", "User found, please check your email address for the OTP");
            }
            
        }

        [RelayCommand]
        void GoBack()
        {
            App.Current.MainPage = new LoginView();
        }

        #endregion
        /// <summary>
        /// Clear Text Fields
        /// </summary>
        private void Clear()
        {
            //Login.UserLogin.Username = "";
            //Login.UserLogin.Password = "";
            NewPassword = String.Empty;
            ConfirmPassword = String.Empty;
        }

        private void GenerateOTP(Users user)
        {
            Random rand = new Random();
            StringBuilder randomString = new StringBuilder();

            //The length of the generated OTP
            int OTP_Length = 6;

            //Clear previous OTP
            OneTimePin = string.Empty;

            for (int i = 0; i < OTP_Length; i++)
            {
                int number = rand.Next(10);
                randomString.Append(number);
            }

            OneTimePin = randomString.ToString();

            email.SendOTP(user.Email, user.FirstName, user.LastName, oneTimePin);
        }

        private void Reset_or_Update_Password()
        {
            if (Login.UserLogin.IsLoggedIn)
            {
                reset_Update_Password = true;
            }
            else
                reset_Update_Password = false;
        }

        /// <summary>
        /// Take a Photo
        /// </summary>
        public async void TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    
                    using Stream stream = await photo.OpenReadAsync();


                    //using var stream = await SignatureDrawing.GetImageStream(1024, 1024);
                    using var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);

                    stream.Position = 0;
                    memoryStream.Position = 0;

#if WINDOWS
                    await System.IO.File.WriteAllBytesAsync(
                        @"C:\Users\Lindo\Desktop\SignatureImage\test.png", memoryStream.ToArray());

#elif ANDROID
                    var context = Platform.CurrentActivity;

                    Android.Content.ContentResolver resolver = context.ContentResolver;
                    Android.Content.ContentValues contentValues = new();
                    contentValues.Put(Android.Provider.MediaStore.IMediaColumns.DisplayName, "test.png");
                    contentValues.Put(Android.Provider.MediaStore.IMediaColumns.MimeType, "image/png");
                    contentValues.Put(Android.Provider.MediaStore.IMediaColumns.RelativePath, "DCIM/" + "test");
                    Android.Net.Uri imageUri = resolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);
                    var os = resolver.OpenOutputStream(imageUri);
                    Android.Graphics.BitmapFactory.Options options = new();
                    options.InJustDecodeBounds = true;
                    var bitmap = Android.Graphics.BitmapFactory.DecodeStream(stream);
                    bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, os);
                    os.Flush();
                    os.Close();

#endif
                }
            }
        }
    }
}
