using System;

namespace SharedServices.BL.Domain
{
    public class FaqResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public FaqQuestion Question { get; set; }
        public bool IsDeleted { get; set; }
    }
}