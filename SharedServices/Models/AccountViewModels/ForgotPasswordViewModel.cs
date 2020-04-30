using System.ComponentModel.DataAnnotations;

namespace SharedServices.UI.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}