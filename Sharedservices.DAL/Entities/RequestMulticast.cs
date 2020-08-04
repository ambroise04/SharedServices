using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class RequestMulticast
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser RequesterMulticast { get; set; }
        public ICollection<ResponseMulticastRequest> Responses { get; set; }
        [Required]
        public Service Service { get; set; }        
        public DateTime DateOfRequest { get; set; }
        [Required]
        public DateTime DateOfAddition { get; set; }
        [Required]
        public int Point { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Accepted { get; set; }
        public Place Place { get; set; }
    }
}