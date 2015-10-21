namespace Zanshin.Domain.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Common), Name = "ExternalLoginConfirmationViewModel_Email_Email")]
        public string Email { get; set; }
    }
}