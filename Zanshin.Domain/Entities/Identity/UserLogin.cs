namespace Zanshin.Domain.Entities.Identity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNet.Identity.EntityFramework;
    

    /// <summary>
    /// </summary>
    
    public class UserLogin : IdentityUserLogin<int>
    {
        /// <summary>
        /// Gets or sets the user login identifier.
        /// </summary>
        /// <value>
        /// The user login identifier.
        /// </value>
        [Key]
        public int UserLoginId { get; set; }

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