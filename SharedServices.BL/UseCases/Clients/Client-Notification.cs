using SharedServices.BL.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using SharedServices.Mutual.Enumerations;
using System.Threading.Tasks;

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
                                          .GetByPredicateWithoutTracking(x => x.Correspondent.Id.Equals(userId))
                                          .OrderBy(n => n.IsTriggered)
                                          .ThenByDescending(x => x.DateOfAddition)
                                          .Select(x => Mapping.Mapping.Mapper.Map<Notification>(x))
                                          .ToList();

            return notifications;
        }

        public int GetUserNotTriggeredNotifications(string userId)
        {
            var notifications = unitOfWork.NotificationRepository
                                          .GetByPredicate(x => x.Correspondent.Id.Equals(userId) && !x.IsTriggered)
                                          .Count();

            return notifications;
        }

        public async Task MarkUserNotificationsAsTriggered(string userId)
        {
            var notifications = GetUserNotifications(userId);
            unitOfWork.CreateTransaction();
            try
            {                
                bool result = false;
                foreach (var notif in notifications)
                {
                    result = TriggerNotification(notif.Id);
                    if (!result)
                    {
                        break;
                    }
                }

                if (result)
                    unitOfWork.CommitTransaction();
                else
                    unitOfWork.RollbackTransaction();

                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                unitOfWork.RollbackTransaction();
                await Task.FromResult(0);
            }            
        }

        public List<Notification> GetUserMulticastNotifications(string userId)
        {
            var notifications = unitOfWork.NotificationRepository
                                          .GetByPredicate(x => x.Correspondent.Id.Equals(userId) &&
                                                         !x.IsTriggered &&
                                                          x.Type.Type == NotificationTypes.RequestBroadcasted)
                                          .OrderByDescending(n => n.DateOfAddition)
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

        public bool TriggerNotification(int notificationId)
        {
            var notification = unitOfWork.NotificationRepository.GetById(notificationId);
            notification.IsTriggered = true;
            var result = unitOfWork.NotificationRepository
                                   .Update(notification);

            return result.IsTriggered;
        }
    }
}