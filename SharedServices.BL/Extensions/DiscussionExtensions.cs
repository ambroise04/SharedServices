using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.Mutual.TO;
using System;
using System.Globalization;

namespace SharedServices.Mutual.Extensions
{
    public static class DiscussionExtensions
    {
        public static DiscussionTO ToTransfer(this Discussion discussion)
        {
            if (discussion is null)
            {
                throw new ArgumentNullException(nameof(discussion));
            }

            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            var culture = (cultureFR) ? CultureInfo.CreateSpecificCulture("fr-FR") : CultureInfo.CreateSpecificCulture("en-US");
            var discussionTO = new DiscussionTO
            {
                Id = discussion.Id,
                Emitter = discussion.Emitter,
                Receiver = discussion.Receiver,
                EmitterName = string.Concat(discussion.EmitterUser.FirstName, " ",discussion.EmitterUser.LastName),
                Message = discussion.Message,
                Date = discussion.DateHour.ToString("HH:mm - dd/MM/yyyy", culture),
                PictureSource = discussion.EmitterUser?.ResizePicture(50, 50)
            };

            return discussionTO;
        }
    }
}