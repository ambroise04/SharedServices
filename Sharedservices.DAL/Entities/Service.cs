using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public ServiceGroup Group { get; set; }
        public ICollection<ApplicationUserServices> UserServices { get; set; }
    }
}