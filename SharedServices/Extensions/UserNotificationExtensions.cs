using SharedServices.BL.Domain;
using SharedServices.BL.Mapping;
using SharedServices.DAL;

namespace SharedServices.UI.Extensions
{
    public static class UserNotificationExtensions
    {
        public static void Notify(this ApplicationUser user, Notification notification)
        {
            user.Notifications.Add(Mapping.Mapper.Map<DAL.Entities.Notification>(notification));
        }
    }
}