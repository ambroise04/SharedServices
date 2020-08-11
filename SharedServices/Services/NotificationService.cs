using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SharedServices.BL.Domain;
using SharedServices.BL.Mapping;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.Mutual.Enumerations;
using SharedServices.UI.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public class NotificationService : INotificationService
    {
        private IHubContext<NotificationHub> hubContext;
        private readonly IUserConnectionManager userConnectionManager;
        private UserManager<ApplicationUser> userManager;
        private IUnitOfWork unitOfWork;

        public NotificationService(IHubContext<NotificationHub> hubContext,
                                   UserManager<ApplicationUser> userManager,
                                   IUnitOfWork unitOfWork, 
                                   IUserConnectionManager userConnectionManager)
        {
            this.hubContext = hubContext;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.userConnectionManager = userConnectionManager;
        }

        public async Task MulticastNotification(RequestMulticast request, ApplicationUser requester, string method = "multicast")
        {
            var notificationType = unitOfWork.NotificationTypeRepository
                                             .GetByPredicate(x => x.Type == NotificationTypes.RequestBroadcasted)
                                             .Select(x => Mapping.Mapper.Map<NotificationType>(x))
                                             .First();
            int serviceId = request.Service.Id;
            request.Service = null;
            var notification = request.MulticastNotification(notificationType);
            var usersToNotify = userManager.Users.Where(u => u.UserServices.Any(us => us.ServiceId == serviceId) && !u.Id.Equals(requester.Id));
            foreach (var user in usersToNotify)
            {
                if (user.Notifications is null) user.Notifications = new List<DAL.Entities.Notification>();
                //user.Notify(notification);
                //await userManager.UpdateAsync(user);
                notification.User = requester;
                notification.Correspondent = user;
                unitOfWork.NotificationRepository.Insert(Mapping.Mapper.Map<DAL.Entities.Notification>(notification));

                await UserNotificationSender(method, user);
            }
        }

        private async Task UserNotificationSender(string method, ApplicationUser user)
        {
            var connections = userConnectionManager.GetUserConnections(user.Id);

            foreach (var connection in connections)
            {
                await hubContext.Clients.Client(connection).SendAsync(method);
            }
        }
    }
}