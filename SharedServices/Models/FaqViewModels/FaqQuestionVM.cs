using System.Collections.Generic;

namespace SharedServices.UI.Models.FaqViewModels
{
    public class FaqQuestionVM
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public List<FaqResponseVM> Responses { get; set; }
        public string UserPicture { get; set; }
    }
}