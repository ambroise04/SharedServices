using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public Feedback AddFeedback(Feedback feedback)
        {
            if (feedback is null)
            {
                throw new ArgumentNullException(nameof(feedback));
            }

            var addedFeedback = unitOfWork.FeedbackRepository
                      .Insert(Mapping.Mapping.Mapper.Map<DAL.Entities.Feedback>(feedback));

            return Mapping.Mapping.Mapper.Map<Feedback>(addedFeedback);
        }

        public List<Feedback> GetAllFeedbacks()
        {
            var services = unitOfWork.FeedbackRepository
                      .GetAll()
                      .Select(s => Mapping.Mapping.Mapper.Map<Feedback>(s))
                      .ToList();

            return services;
        }

        public List<Feedback> GetFeedbacksByUser(string userId)
        {
            var feedbacks = unitOfWork.FeedbackRepository
                      .GetByPredicate(f => f.User.Id.Equals(userId))
                      .Select(s => Mapping.Mapping.Mapper.Map<Feedback>(s))
                      .ToList();

            return feedbacks;
        }
    }
}