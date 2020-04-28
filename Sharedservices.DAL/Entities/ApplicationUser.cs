using Microsoft.AspNetCore.Identity;
using SharedServices.DAL.Entities;
using System.Collections.Generic;

namespace SharedServices.DAL
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PostalCode { get; set; }
        public int Point { get; set; }
        public string City { get; set; }
        public ICollection<ApplicationUserServices> UserServices { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}