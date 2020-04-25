using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SharedServices.DAL.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ServiceGroup Group { get; set; }
        public ICollection<IdentityUser> Users { get; set; }
    }
}