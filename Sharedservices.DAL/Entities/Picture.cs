using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}