using SharedServices.DAL;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.BL.Domain
{
    public class Notification
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Correspondent { get; set; }
        [Required]
        public NotificationType Type { get; set; }        
        public Request Request { get; set; }
        public Service Service { get; set; }
        public RequestMulticast RequestMulticast { get; set; }
        public bool IsTriggered { get; set; }
        [Required]
        public DateTime DateOfAddition { get; set; }
    }
}