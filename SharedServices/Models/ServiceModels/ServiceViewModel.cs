using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.UI.Models.ServiceModels
{
    public class ServiceViewModel
    {
        [Required]
        public List<int> Services { get; set; }
    }
}