namespace Zanshin.Domain.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}
