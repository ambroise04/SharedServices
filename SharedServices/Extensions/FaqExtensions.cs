using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.UI.Models.FaqViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.UI.Extensions
{
    public static class FaqExtensions
    {
        public static List<FaqQuestionVM> ToViewModels(this List<FaqQuestion> questions, bool inFr)
        {
            List<FaqQuestionVM> viewQuestions = new List<FaqQuestionVM>();
            foreach (var question in questions)
            {
                List<FaqResponseVM> responses = question.Responses.Select(r => new FaqResponseVM 
                { 
                    Id = r.Id,
                    Message = r.Message,
                    Date = inFr ? r.Date.ToString("dd/MM/yyyy HH:mm") : r.Date.ToString("MM-dd-yyyy hh:mm tt"),
                })
                .OrderByDescending(r => r.Date)
                .ToList();   
                
                viewQuestions.Add(new FaqQuestionVM
                {
                    Id = question.Id,
                    Message = question.Message,
                    User = string.Concat(question.User.FirstName, " ", question.User.LastName),
                    UserPicture = question.User.PictureSource(),
                    Date = inFr ? question.Date.ToString("dd/MM/yyyy HH:mm") : question.Date.ToString("MM-dd-yyyy hh:mm tt"),
                    Responses = responses
                });
            }

            return viewQuestions;
        }

        public static FaqQuestionVM ToViewModel(this FaqQuestion question, bool inFr)
        {            
                List<FaqResponseVM> responses = question.Responses.Select(r => new FaqResponseVM
                {
                    Id = r.Id,
                    Message = r.Message,
                    Date = inFr ? r.Date.ToString("dd/MM/yyyy HH:mm") : r.Date.ToString("MM-dd-yyyy hh:mm tt"),
                })
                .OrderByDescending(r => r.Date)
                .ToList();

                var questionVM = new FaqQuestionVM
                {
                    Id = question.Id,
                    Message = question.Message,
                    User = string.Concat(question.User.FirstName, " ", question.User.LastName),
                    UserPicture = question.User.PictureSource(),
                    Date = inFr ? question.Date.ToString("dd/MM/yyyy HH:mm") : question.Date.ToString("MM-dd-yyyy hh:mm tt"),
                    Responses = responses
                };

            return questionVM;
        }

        public static FaqResponseVM ToViewModel(this FaqResponse response, bool inFr)
        {
            var responseVM =  new FaqResponseVM
            {
                Id = response.Id,
                Message = response.Message,
                Date = inFr ? response.Date.ToString("dd/MM/yyyy HH:mm") : response.Date.ToString("MM-dd-yyyy hh:mm tt")
            };           

            return responseVM;
        }
    }
}