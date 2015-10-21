

namespace Zanshin.Domain.Entities.Forum
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    /// <remarks>
    /// IdentityRole also contains the properties
    /// Id, Role and Users.
    /// </remarks>
    public class Group
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group()
        {
            this.DisplayGroupInLegend = true;
            this.Administrators = new List<User>();
            this.Members = new HashSet<User>();
        }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        [Key]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        [MaxLength(20),Required]
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the founder identifier.
        /// </summary>
        /// <value>
        /// The founder identifier.
        /// </value>
        public int? FounderId { get; set; }

        /// <summary>
        /// Gets or sets the group description.
        /// </summary>
        /// <value>
        /// The group description.
        /// </value>
        [Display(ResourceType = typeof(Common), Name = "GroupDescription")]
        [MaxLength(256)]
        public string GroupDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [display group in legend].
        /// </summary>
        /// <value>
        /// <c>true</c> if [display group in legend]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayGroupInLegend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [group receive private messages].
        /// </summary>
        /// <value>
        /// <c>true</c> if [group receive private messages]; otherwise, <c>false</c>.
        /// </value>
        [Display(ResourceType = typeof(Common), Name = "GroupReceivePrivateMessages")]
        public bool GroupRecievePrivateMessages { get; set; }

        /// <summary>
        /// Gets or sets the color of the group.
        /// </summary>
        /// <value>
        /// The color of the group.
        /// </value>
        [MaxLength(7), Display(ResourceType = typeof(Common), Name = "GroupColor")]
        public string GroupColor { get; set; }


        /// <summary>
        /// Gets or sets the member count.
        /// </summary>
        /// <value>
        /// The member count.
        /// </value>
        public int MemberCount { get; set; }


        /// <summary>
        /// Gets or sets the admin count.
        /// </summary>
        /// <value>
        /// The admin count.
        /// </value>
        public int AdminCount { get; set; }

        /// <summary>
        /// Gets or sets the administrators.
        /// </summary>
        /// <value>
        /// The administrators.
        /// </value>
        public ICollection<User> Administrators { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ICollection<User> Members { get; set; }

    }
}