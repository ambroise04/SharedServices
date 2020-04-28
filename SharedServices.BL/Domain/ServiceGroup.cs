using System.Collections.Generic;

namespace SharedServices.BL.Domain
{
    public class ServiceGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PointsByHour { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}