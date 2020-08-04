using SharedServices.DAL;
using SharedServices.Mutual.Enumerations;
using System;

namespace SharedServices.BL.Domain
{
    public class Request
    {
        public int Id { get; set; }
        public ApplicationUser Receiver { get; set; }
        public ApplicationUser Requester { get; set; }
        public Service Service { get; set; }
        public DateTime DateOfRequest { get; set; }
        public DateTime DateOfAddition { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
        public RequestSource Source { get; set; }
        public RequestStates State { get; set; }
        public Place Place { get; set; }
        public bool RequesterValidation { get; set; }
        public bool ReceiverValidation { get; set; }
    }
}