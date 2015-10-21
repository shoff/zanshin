namespace Zanshin.Domain.Entities.Identity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNet.Identity.EntityFramework;
    

    /// <summary>
    /// </summary>
    
    public class UserClaim : IdentityUserClaim<int>
    {
        /// <summary>
        /// Gets or sets the user claim identifier.
        /// </summary>
        /// <value>
        /// The user claim identifier.
        /// </value>
        [Key]
        public int UserClaimId { get; set; }


        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}