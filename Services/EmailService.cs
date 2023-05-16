using Aspose.Email;
using Aspose.Email.Clients.Smtp;


namespace TalaGrid.Services
{
    public class EmailService
    {
        // create SmtpClient as client and specify server, port, user name and password
        SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587, "lindohgamede@outlook.com", "#PeterS23");

        // create instances of MailMessage class and Specify To, From, Subject, and Message
        string FromEmail = "lindohgamede@outlook.com";
        string Subject;
        string Message;
        MailMessage mailMessage;

        public void SendOTP(string ToEmail, string ToFirstName, string ToLastName, string OTP)
        {
            Subject = "Account Verification OTP";
            Message = $"Hi {ToFirstName} {ToLastName},\n\nPlease see your TalaGrid OTP: {OTP}";
            mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);
        }

        public void Send_GW_Verification(string ToEmail, string ToFirstName, string ToLastName, string OTP)
        {
            Subject = "Admin Verification";
            Message = $"Hi {ToFirstName} {ToLastName},\n\nPlease see your TalaGrid OTP: {OTP}";
            mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);
        }
    }
}
