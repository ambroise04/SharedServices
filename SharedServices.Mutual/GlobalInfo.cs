using System.ComponentModel.DataAnnotations;

namespace SharedServices.Mutual
{
    public class GlobalInfo
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string DescriptionFR { get; set; }
        public string DescriptionEN { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string AddressFR { get; set; }
        public string AddressEN { get; set; }
        [Required]
        public string AuthorLink { get; set; }
        public int DefaultPointForUsers { get; set; }
    }
}