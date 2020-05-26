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
        [Required]
        public bool Accepted { get; set; }
    }
}