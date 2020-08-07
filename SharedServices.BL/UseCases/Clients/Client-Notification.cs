using SharedServices.BL.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public Notification AddNotification(Notification notification)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var addedNotification = unitOfWork.NotificationRepository
                      .Insert(Mapping.Mapping.Mapper.Map<DAL.Entities.Notification>(notification));

            return Mapping.Mapping.Mapper.Map<Notification>(addedNotification);
        }

        public List<Notification> GetUserNotifications(string userId)
        {
            var notifications = unitOfWork.NotificationRepository
                                          .GetByPredicate(x => x.Correspondent.Id.Equals(userId))
                                          .OrderBy(n => n.IsTriggered)
                                          .ThenByDescending(x => x.DateOfAddition)
                                          .Select(x => Mapping.Mapping.Mapper.Map<Notification>(x))
                                          .ToList();

            return notifications;
        }

        public bool DeleteNotification(int id)
        {
            var result = unitOfWork.NotificationRepository
                                   .Delete(id);

            return result;
        }
    }
}