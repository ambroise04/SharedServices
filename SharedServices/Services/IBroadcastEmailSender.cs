using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public interface IBroadcastEmailSender
    {
        Task SendEmailAsync(List<string> emails, string subject, string message);
    }
}
