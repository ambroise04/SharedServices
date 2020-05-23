using SharedServices.DAL;
using System;

namespace SharedServices.BL.Domain
{
    public class ContactTO
    {
        public ApplicationUser Contact { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}