using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        public List<FaqQuestion> GetFaqQuestions()
        {
            var questions = unitOfWork.FaqQuestionRepository
                                      .GetAll()
                                      .Select(q => Mapping.Mapping.Mapper.Map<FaqQuestion>(q))
                                      .ToList();

            return questions;
        }

        public FaqQuestion GetFaqQuestion(int questionId)
        {
            var question = unitOfWork.FaqQuestionRepository
                                      .GetById(questionId);

            return Mapping.Mapping.Mapper.Map<FaqQuestion>(question);
        }

        public bool AnswerToFaqQuestion(int questionId, FaqResponse response)
        {
            var entity = unitOfWork.FaqQuestionRepository.GetById(questionId);
            if (entity.Responses is null)
                entity.Responses = new List<DAL.Entities.FaqResponse>();

            entity.Responses.Add(Mapping.Mapping.Mapper.Map<DAL.Entities.FaqResponse>(response));
            var result = unitOfWork.FaqQuestionRepository.Update(entity);

            return !(result is null);
        }
    }
}