using SharedServices.DAL;
using System;
using System.Collections.Generic;

namespace SharedServices.BL.Domain
{
    public class RequestMulticast
    {
        public int Id { get; set; }
        public ApplicationUser RequesterMulticast { get; set; }
        public ICollection<ResponseMulticastRequest> Responses { get; set; }
        public Service Service { get; set; }
        public DateTime DateOfRequest { get; set; }
        public DateTime DateOfAddition { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public bool Accepted { get; set; }
        public Place Place { get; set; }
    }
}