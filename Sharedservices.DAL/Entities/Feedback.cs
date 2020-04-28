using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        public int Mark { get; set; }
        public string Comment { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Advisor { get; set; }
        public bool DisplayAdvisorName { get; set; }
    }
}