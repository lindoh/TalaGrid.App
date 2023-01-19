using Aspose.Email;
using Aspose.Email.Clients.Smtp;


namespace TalaGrid.Services
{
    public class EmailService
    {
        public void SendEmail(string ToEmail, string ToFirstName, string ToLastName, string OTP)
        {
            // create SmtpClient as client and specify server, port, user name and password
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587, "lindohgamede@outlook.com", "#PeterS23");

            // create instances of MailMessage class and Specify To, From, Subject, and Message
            string FromEmail = "lindohgamede@outlook.com";
            string Subject = "Account Verification OTP";
            string Message = $"Hi {ToFirstName} {ToLastName},\n\nPlease see your TalaGrid OTP: {OTP}";
            MailMessage mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Message);

            client.Send(mailMessage);

            /*
            var sender = new SmtpSender(() => new SmtpClient("smtp-mail.outlook.com")
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential("lindohgamede@outlook.com", "#PeterS23"),
                Port = 587

            });

            Email.DefaultSender = sender;

            var email = await Email
                .From("lindohgamede@outlook.com", "Farecost")
                .To(ToEmail, $"{ToFirstName} {ToLastName}")
                .Subject("Account Verification OTP")
                .Body($"Hi {ToFirstName} {ToLastName},\n\nPlease see your TalaGrid OTP: {OTP}")
                .SendAsync();
            */

        }
    }
}
