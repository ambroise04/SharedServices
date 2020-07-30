using Microsoft.AspNetCore.Html;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.UI.Models.NotificationViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.UI.Extensions
{
    public static class NotificationExtensions
    {
        public static List<UserNotificationsVM> ToViewModelCollection(this List<Notification> notifications, bool inFR = true)
        {
            return notifications.Select(n => new UserNotificationsVM
            {
                Id = n.Id,
                Message = inFR ? n.Type.MessageFR : n.Type.MessageEN,
                Date = inFR ? n.DateOfAddition.ToString("dd/MM/yyyy HH:mm") : n.DateOfAddition.ToString("MM-dd-yyyy H:mm tt"),
                CorrespondentId = n.Correspondent.Id,
                CorrespondentPicture = n.Correspondent.PictureSource(),
                CorrespondentName = string.Concat(n.Correspondent.FirstName, " ", n.Correspondent.LastName),
                IsTriggered = n.IsTriggered,
                Buttons = GenerateButtons(n, inFR)
            }).ToList();
        }

        public static List<HtmlString> GenerateButtons(this Notification notification, bool inFR)
        {
            List<HtmlString> buttons = new List<HtmlString>();
            switch (notification.Type.Type)
            {
                case Mutual.Enumerations.NotificationTypes.ReceiverValidation:
                    var btnConfirm = inFR ? "Confirmer" : "Confirm";
                    buttons.Add(new HtmlString($"<button data-source=\"{notification.Request.Id}\" class=\"btn btn-sm btn-outline-my-green confirm-btn\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnConfirm}</span>" +
                                $"</button>"));

                    var btnContest = inFR ? "Contester" : "Contest";
                    buttons.Add(new HtmlString($"<button data-source=\"{notification.Request.Id}\" class=\"btn btn-sm btn-outline-my-red contest-btn\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnContest}</span>" +
                                $"</button>"));
                    break;
                case Mutual.Enumerations.NotificationTypes.RequesterValidation:
                    var btnConfirm1 = inFR ? "Accepter" : "Accept";
                    buttons.Add(new HtmlString($"<button data-source=\"{notification.Request.Id}\" class=\"btn btn-sm btn-outline-my-green receiver-accept-btn\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnConfirm1}</span>" +
                                $"</button>"));

                    var btnSignal = inFR ? "Signaler une erreur" : "Signal an error";
                    buttons.Add(new HtmlString($"<button data-source=\"{notification.Request.Id}\" class=\"btn btn-sm btn-outline-my-red signal-error-btn\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnSignal}</span>" +
                                $"</button>"));
                    break;
                case Mutual.Enumerations.NotificationTypes.RequestPostulation:
                    var btnViewMore = inFR ? "Voir plus" : "View more";
                    buttons.Add(new HtmlString($"<button data-source=\"{notification.RequestMulticast.Id}\" class=\"btn btn-sm btn-outline-my-green postulate-bnt\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnViewMore}</span>" +
                                $"</button>"));
                    break;
                case Mutual.Enumerations.NotificationTypes.RequestBroadcasted:
                    var btnPostulate = inFR ? "Postuler" : "Postulate";
                    buttons.Add(new HtmlString($"<button data-source=\"{notification.RequestMulticast.Id}\" class=\"btn btn-sm btn-outline-my-green broadcast-btn\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnPostulate}</span>" +
                                $"</button>"));
                    break;
                default:
                    var btnDefault = inFR ? "Afficher" : "Display";
                    buttons.Add(new HtmlString($"<button class=\"btn btn-sm btn-outline-my-green default-btn\" " +
                                $"style=\"box-shadow: none!important\" " +
                                $"type=\"button\"><span aria-hidden=\"true\">{btnDefault}</span>" +
                                $"</button>"));
                    break;
            }

            return buttons;
        }
    }
}