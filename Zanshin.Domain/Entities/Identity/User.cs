
namespace Zanshin.Domain.Entities.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Providers.Interfaces;

    /// <summary>
    /// </summary>
    [Table("Users")]
    
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        public User()
        {
            this.Groups = new HashSet<Group>();
            this.Tags = new List<Tag>();
            this.Messages = new HashSet<PrivateMessage>();
            this.Active = true;
        }
        
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageUserName")]
        [StringLength(20)]
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributeUserName")]
        public override string UserName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [StringLength(20)]
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributeDisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [StringLength(256)]
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributePassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password last changed date.
        /// </summary>
        /// <value>
        /// The password last changed date.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime PasswordLastChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum days between password change.
        /// </summary>
        /// <value>
        /// The maximum days between password change.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public int MaximumDaysBetweenPasswordChange { get; set; }

        /// <summary>
        /// Gets or sets the post count.
        /// </summary>
        /// <value>
        /// The post count.
        /// </value>
        public int PostCount { get; set; }

        /// <summary>
        /// Gets or sets the topic count.
        /// </summary>
        /// <value>
        /// The topic count.
        /// </value>
        public int TopicCount { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required, StringLength(256)]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributeEmailAddress")]
        public override string Email { get; set; }

        /// <summary>
        /// Gets or sets the tagline.
        /// </summary>
        /// <value>
        /// The tagline.
        /// </value>
        [StringLength(512)]
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributeTagline")]
        public string Tagline { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [StringLength(80)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the last search.
        /// </summary>
        /// <value>
        /// The last search.
        /// </value>
        [MaxLength(256)]
        public string LastSearch { get; set; }

        /// <summary>
        /// Gets or sets the rank identifier.
        /// </summary>
        /// <value>
        /// The rank identifier.
        /// </value>
        [ForeignKey("Rank")]
        public int? RankId { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        public Rank Rank { get; set; }

        /// <summary>
        /// Gets or sets the joined date.
        /// </summary>
        /// <value>
        /// The joined date.
        /// </value>
        [Required]
        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }

        /// <summary>
        /// Gets or sets the last login.
        /// </summary>
        /// <value>
        /// The last login.
        /// </value>
        [Required]
        [DataType(DataType.Date)]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Gets or sets the karma.
        /// </summary>
        /// <value>
        /// The karma.
        /// </value>
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributeKarma")]
        public int Karma { get; set; }

        /// <summary>
        /// Gets or sets the user icon.
        /// </summary>
        /// <value>
        /// The user icon.
        /// </value>
        public Avatar UserIcon { get; set; }

        /// <summary>
        /// Gets or sets the avatar identifier.
        /// </summary>
        /// <value>
        /// The avatar identifier.
        /// </value>
        [ForeignKey("UserIcon")]
        public int AvatarId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="User"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [StringLength(512)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>
        /// The user profile identifier.
        /// </value>
        [ForeignKey("Profile")]
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public UserProfile Profile { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<Group> Groups { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public ICollection<PrivateMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }
        
        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}