using Aspose.Email;
using Aspose.Email.Clients.Smtp;


namespace TalaGrid.Services
{
    public class EmailService
    {
        #region Email Service Properties
        // create SmtpClient as client and specify server, port, user name and password
        SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587, "lindohgamede@outlook.com", "#PeterS23");

        // create instances of MailMessage class and Specify To, From, Subject, and Message
        string FromEmail = "lindohgamede@outlook.com";
        string Subject;
        string Message;
        string Message1;
        MailMessage mailMessage;

        #endregion

        #region Email Service Methods
        /// <summary>
        /// Send a One Time Pin (OTP) to the user for verifation before the password reset is allowed
        /// </summary>
        /// <param name="ToEmail">Recepient Email</param>
        /// <param name="ToFirstName">Recepient Name</param>
        /// <param name="ToLastName">Recepient Lastname</param>
        /// <param name="OTP">One Time Pin</param>
        public void SendOTP(string ToEmail, string ToFirstName, string ToLastName, string OTP)
        {
            Subject = "Account Verification OTP";
            Message = $"Hi {ToFirstName} {ToLastName},\n\nPlease see your TalaGrid OTP: {OTP}";
            mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);
        }

        /// <summary>
        /// Send a verification email to the Admin for GW_Admin account verification
        /// </summary>
        /// <param name="ToEmail">Recepient Email</param>
        /// <param name="ToFirstName">Recepient Name</param>
        /// <param name="ToLastName">Recepient Lastname</param>
        /// <param name="IdNumber">Recepient Id Number</param>
        public void Send_GW_Verification(string ToEmail, string ToFirstName, string ToLastName, string IdNumber, string Subject, string Message)
        {
            mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);
        }

        /// <summary>
        /// Send a verification response email to the GW_Admin
        /// </summary>
        /// <param name="ToEmail">Recepient Email</param>
        /// <param name="ToFirstName">Recepient Name</param>
        /// <param name="ToLastName">Recepient Lastname</param>
        public void Send_GW_Ver_Response(string ToEmail, string ToFirstName, string ToLastName, bool Approved)
        {
            Subject = "Admin Verification";
            Message = $"Hi {ToFirstName} {ToLastName},\n\nPlease note that your account has been verified, you can now Login and use the application." +
                $"\n\n\nRegards,\nTalagrid Admin";
            Message1 = $"Hi {ToFirstName} {ToLastName},\n\nPlease note that your account verification has been Rejected," +
                $"please contact the responsible administrator for more information";

            if (Approved)
                mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);
            else
                mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message1);

            client.Send(mailMessage);
        }

        /// <summary>
        /// Send a verification email to the Admin for BBC_Admin account verification
        /// </summary>
        /// <param name="ToEmail">Recepient Email</param>
        /// <param name="ToFirstName">Recepient Name</param>
        /// <param name="ToLastName">Recepient Lastname</param>
        /// <param name="IdNumber">Recepient Id Number</param>
        public void Send_BBC_Verification(string ToEmail, string ToFirstName, string ToLastName, string IdNumber)
        {
            Subject = "Admin Verification";
            Message = $"Hi Admin, \n\nA verification for a new registered Buy-Back-Center Admin, {ToFirstName} {ToLastName} " +
                $"with Id Number: {IdNumber} is required. Please Login into the App and action the request. \n\n\nRegards, \nTalagrid";

            mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);
        }

        /// <summary>
        /// Send a verification response email to the BBC_Admin
        /// </summary>
        /// <param name="ToEmail">Recepient Email</param>
        /// <param name="ToFirstName">Recepient Name</param>
        /// <param name="ToLastName">Recepient Lastname</param>
        public void Send_BBC_Ver_Response(string ToEmail, string ToFirstName, string ToLastName)
        {
            Subject = "Admin Verification";
            Message = $"Hi {ToFirstName} {ToLastName},\n\nPlease note that your account has been verified, you can now Login and use the application." +
                $"\n\n\nRegards,\nGreenWay Africa Admin";

            mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);
        }

        #endregion
    }
}
