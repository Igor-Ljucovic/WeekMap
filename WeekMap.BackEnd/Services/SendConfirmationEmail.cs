using MailKit.Net.Smtp;
using MimeKit;
using WebApp.Models;

namespace WebApp.Services
{
    public class EmailConfirmationService
    {
        public void SendConfirmationEmail(User user, string confirmationLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your App Name", "your@email.com"));
            message.To.Add(MailboxAddress.Parse(user.Email));
            message.Subject = "Confirm your email address";

            message.Body = new TextPart("plain")
            {
                Text = $"Hello {user.Username},\n\nPlease confirm your email by clicking this link:\n{confirmationLink}\n\nThanks!"
            };

            using var client = new SmtpClient();
            //client.Connect("sandbox.smtp.mailtrap.io", 2525, MailKit.Security.SecureSocketOptions.StartTls);
            //client.Authenticate("445f285727bc3d", "bd1cb459060c67");

            //client.Send(message);
            //client.Disconnect(true);
        }
    }
}
