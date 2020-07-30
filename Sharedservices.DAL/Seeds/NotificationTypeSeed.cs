using SharedServices.DAL.Entities;
using SharedServices.Mutual;
using SharedServices.Mutual.Enumerations;
using System.Collections.Generic;

namespace SharedServices.DAL.Seeds
{
    public static class NotificationTypeSeed
    {
        public static List<NotificationType> SeedTypes()
        {
            return new List<NotificationType>
            {
                new NotificationType
                {
                    Id = 1,
                    Type = NotificationTypes.ReceiverValidation,
                    MessageFR = "Votre correspondant a marqué un service comme rendu. Veuillez donner votre accord pour le transfert des points.",                    
                    MessageEN = "Your correspondent has marked a service as rendered. Your confirmation is required for the transfer of points."
                },
                new NotificationType
                {
                    Id = 2,
                    Type = NotificationTypes.RequesterValidation,
                    MessageFR = "Votre correspondant a marqué un service comme rendu. Veuillez accepter le transfert des points",
                    MessageEN = "Your correspondent has marked a service as rendered. Please accept the transfer of points.",
                },
                new NotificationType
                {
                    Id = 3,
                    Type = NotificationTypes.RequestPostulation,
                    MessageFR = "Une nouvelle réponse a votre demande a été envoyée. Consulter la liste des demandes ?",
                    MessageEN = "New response to your request. Consult the list of requests ?",
                },
                new NotificationType
                {
                    Id = 4,
                    Type = NotificationTypes.RequestBroadcasted,
                    MessageFR = "Une nouvelle demande a été publiée. Voulez-vous en savoir plus ?",
                    MessageEN = "A new request has been published. Do you want to know more ?",
                }
            };
        }
    }
}