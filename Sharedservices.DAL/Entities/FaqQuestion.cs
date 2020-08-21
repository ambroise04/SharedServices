using System;
using System.Collections.Generic;

namespace SharedServices.DAL.Entities
{
    public class FaqQuestion
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public List<FaqResponse> Responses { get; set; }
        public bool IsDeleted { get; set; }
    }
}