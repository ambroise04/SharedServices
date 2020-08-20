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

        public Task SendContactEmailAsync(string email, string subject, string message, string name, string receiver)
        {
            return ExecuteContactEmail(Options.SendGridKey, subject, message, email, receiver, name);
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

        private Task ExecuteContactEmail(string apiKey, string subject, string message, string email, string receiver, string name = "")
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(email, name + "Contact | Between us"),
                Subject = subject,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(receiver));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}