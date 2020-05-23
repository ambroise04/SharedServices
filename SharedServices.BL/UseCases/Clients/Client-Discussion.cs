using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Services;
using SharedServices.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public Discussion AddDiscussion(Discussion discussion)
        {
            if (discussion is null)
            {
                throw new ArgumentNullException(nameof(discussion));
            }

            var addedGroup = unitOfWork.DiscussionRepository
                      .Insert(Mapping.Mapping.Mapper.Map<DAL.Entities.Discussion>(discussion));

            return Mapping.Mapping.Mapper.Map<Discussion>(addedGroup);
        }

        public List<Discussion> GetUserDiscussion(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var discussions = unitOfWork.DiscussionRepository
                                .GetByPredicate(d => d.Emitter.Equals(userId) || d.Receiver.Equals(userId))
                                .OrderByDescending(d => d.DateHour)
                                .Select(d => Mapping.Mapping.Mapper.Map<Discussion>(d))
                                .ToList();
            return discussions;
        }

        public List<Discussion> GetDiscussionBetweenCurrentUserAndAnOtherUser(string currentUserId, string otherUserId)
        {
            if (string.IsNullOrEmpty(otherUserId))
            {
                throw new ArgumentNullException(nameof(otherUserId));
            }

            var discussions = unitOfWork.DiscussionRepository
                                .GetByPredicate(d => (d.Emitter.Equals(currentUserId) && d.Receiver.Equals(otherUserId)) 
                                                     || d.Emitter.Equals(otherUserId) && d.Receiver.Equals(currentUserId))
                                .OrderByDescending(d => d.DateHour)
                                .Select(d => Mapping.Mapping.Mapper.Map<Discussion>(d))
                                .ToList();

            var emitterUser = userManager.Users.Include(u => u.Picture).Include(u => u.Contacts).FirstOrDefault(u => u.Id.Equals(currentUserId));
            var receiverUser = userManager.Users.Include(u => u.Picture).Include(u => u.Contacts).FirstOrDefault(u => u.Id.Equals(otherUserId));

            foreach (var disc in discussions)
            {
                if (disc.Emitter.Equals(currentUserId))
                {
                    disc.EmitterUser = emitterUser;
                    disc.ReceiverUser = receiverUser;
                }
                else
                {
                    disc.EmitterUser = receiverUser;
                    disc.ReceiverUser = emitterUser;
                }
            }

            return discussions;
        }

        public List<ContactTO> GetContacts(string user)
        {
            var discussions = GetUserDiscussion(user).Distinct(new DiscussionComparer())
                                                     .ToList();

            List<ContactTO> userContacts = new List<ContactTO>();
            foreach (var discussion in discussions)
            {
                var aContact = discussion.Emitter.Equals(user) ?
                    userManager.Users.Include(u => u.Picture).First(u => u.Id.Equals(discussion.Receiver)) :
                    userManager.Users.Include(u => u.Picture).First(u => u.Id.Equals(discussion.Emitter));
                
                userContacts.Add(new ContactTO {Contact = aContact, Message = discussion.Message, Date = discussion.DateHour });
            }

            return userContacts;
        }
    }
}