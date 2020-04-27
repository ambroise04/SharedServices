using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}