using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class ServiceGroup
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int PointsByHour { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}