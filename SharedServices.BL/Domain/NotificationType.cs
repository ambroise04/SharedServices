using SharedServices.Mutual.Enumerations;
using System.Collections.Generic;

namespace SharedServices.BL.Domain
{
    public class NotificationType
    {
        public int Id { get; set; }
        public NotificationTypes Type { get; set; }
        public string MessageFR { get; set; }
        public string MessageEN { get; set; }
        public ICollection<Notification>  Notifications { get; set; }
    }
}