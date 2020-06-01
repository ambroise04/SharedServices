using Microsoft.AspNetCore.Identity;
using SharedServices.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [InverseProperty("Receiver")]
        public ICollection<Request> RequestsReceived { get; set; }
        [InverseProperty("Requester")]
        public ICollection<Request> RequestsSent { get; set; }
        [InverseProperty("RequesterMulticast")]
        public ICollection<RequestMulticast> RequestMulticasts { get; set; }
        public ICollection<ResponseMulticastRequest> Responses { get; set; }
        public ICollection<ApplicationUser> Contacts { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}