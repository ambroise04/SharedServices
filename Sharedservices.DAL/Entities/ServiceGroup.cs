using System.Collections.Generic;

namespace SharedServices.DAL.Entities
{
    public class ServiceGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}