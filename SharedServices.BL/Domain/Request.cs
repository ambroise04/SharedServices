using SharedServices.DAL;
using System;

namespace SharedServices.BL.Domain
{
    public class Request
    {
        public int Id { get; set; }
        public ApplicationUser Receiver { get; set; }
        public ApplicationUser Sender { get; set; }
        public Service Service { get; set; }
        public DateTime DateOfRequest { get; set; }
        public DateTime DateOfAddition { get; set; }
        public int Point { get; set; }
        public bool Accepted { get; set; }
    }
}