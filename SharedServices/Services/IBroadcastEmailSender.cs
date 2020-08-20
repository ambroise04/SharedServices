using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public interface IBroadcastEmailSender
    {
        Task SendEmailAsync(List<string> emails, string subject, string message);
        Task SendContactEmailAsync(string email, string subject, string message, string name, string receiver);
    }
}
