using System.Collections.Generic;

namespace SharedServices.BL.Domain
{
    public class ServiceTO
    {
        public ServiceGroup ServiceGroup { get; set; }
        public List<Service> Services { get; set; }
    }
}