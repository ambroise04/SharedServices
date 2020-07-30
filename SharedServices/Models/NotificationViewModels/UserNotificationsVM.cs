using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
namespace SharedServices.UI.Models.NotificationViewModels
{
    public class UserNotificationsVM
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string CorrespondentId { get; set; }
        public string CorrespondentName { get; set; }
        public string CorrespondentPicture { get; set; }
        public bool IsTriggered { get; set; }
        public List<HtmlString> Buttons { get; set; }
    }
}