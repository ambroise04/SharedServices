using Microsoft.AspNetCore.Identity;
using SharedServices.DAL;
using System.Collections.Generic;

namespace SharedServices.BL.Domain
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ServiceGroup Group { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}