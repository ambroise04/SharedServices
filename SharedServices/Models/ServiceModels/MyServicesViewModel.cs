using SharedServices.BL.Domain;
using System.Collections.Generic;

namespace SharedServices.UI.Models.ServiceModels
{
    public class MyServicesViewModel
    {
        public ICollection<ServiceTO> SelectServices { get; set; }
        public ICollection<ServiceTO> UserServices { get; set; }
    }
}