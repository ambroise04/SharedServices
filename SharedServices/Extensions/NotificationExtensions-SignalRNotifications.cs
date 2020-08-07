using SharedServices.BL.Domain;
using System;
namespace SharedServices.UI.Extensions
{
    public static partial class NotificationExtensions
    {
        public static Notification MulticastNotification(this RequestMulticast request, NotificationType type)
        {
            return new Notification
            {
                RequestMulticast = request,
                Type = type,
                IsTriggered = false,
                DateOfAddition = DateTime.Now,
            };
        }
    }
}