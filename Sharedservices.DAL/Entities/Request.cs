using SharedServices.Mutual.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public ApplicationUser Receiver { get; set; }
        public ApplicationUser Requester { get; set; }
        [Required]
        public Service Service { get; set; }
        [Required]
        public DateTime DateOfRequest { get; set; }
        public DateTime DateOfAddition { get; set; }
        [Required]
        public int Point { get; set; }
        public int Duration { get; set; }
        [Required]
        public RequestStates State { get; set; }
        public RequestSource Source { get; set; }
        public Place Place { get; set; }
        public bool RequesterValidation { get; set; }
        public bool ReceiverValidation { get; set; }
    }
}