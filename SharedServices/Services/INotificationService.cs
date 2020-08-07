using SharedServices.BL.Domain;
using SharedServices.DAL;
using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public interface INotificationService
    {
        Task MulticastNotification(RequestMulticast request, ApplicationUser requester, string method = "multicast");
    }
}