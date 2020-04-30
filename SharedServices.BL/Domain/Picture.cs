using SharedServices.DAL;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.BL.Domain
{
    public class Picture
    {
        public int Id { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
    }
}