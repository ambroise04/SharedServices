using System;

namespace SharedServices.DAL.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Service Service { get; set; }
        public DateTime DateOfRequest { get; set; }
        public int Point { get; set; }
        public bool Accepted { get; set; }
    }
}