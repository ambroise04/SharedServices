using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public List<FaqQuestion> GetAnsweredFaqQuestions()
        {
            var questions = unitOfWork.FaqQuestionRepository
                                      .GetByPredicate(q => q.Responses.Count() != 0)
                                      .Select(q => Mapping.Mapping.Mapper.Map<FaqQuestion>(q))
                                      .ToList();

            return questions;
        }

        public bool CreateFaqQuestion(FaqQuestion question)
        {
            if (question is null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            var entity = Mapping.Mapping.Mapper.Map<DAL.Entities.FaqQuestion>(question);
            var result = unitOfWork.FaqQuestionRepository.Insert(entity);

            return !(result is null);
        }
    }
}