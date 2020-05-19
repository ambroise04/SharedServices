using Microsoft.AspNetCore.Identity;
using SharedServices.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PostalCode { get; set; }
        public int Point { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        [Range(0, 5)]
        public int Start { get; set; }
        public Picture Picture { get; set; }
        public ICollection<ApplicationUserServices> UserServices { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<ApplicationUser> Contacts { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}