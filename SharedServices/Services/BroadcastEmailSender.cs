using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public class BroadcastEmailSender : IBroadcastEmailSender
    {
        private readonly AuthMessageSenderOptions Options;
        public BroadcastEmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, emails);
        }

        private Task Execute(string apiKey, string subject, string message, List<string> emails)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("info@betweenus.be", "Information | Between us"),
                Subject = subject,
                HtmlContent = message
            };
            List<EmailAddress> emailAddresses = new List<EmailAddress>();
            foreach (var email in emails)
            {
                emailAddresses.Add(new EmailAddress(email));
            }
            msg.AddTos(emailAddresses);

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}