namespace Zanshin.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class MessageReadByUser
    {
        /// <summary>
        /// Gets or sets the group message identifier.
        /// </summary>
        /// <value>
        /// The group message identifier.
        /// </value>
        [Required, ForeignKey("Message")]
        public int GroupMessageId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public GroupMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [Required, ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }
    }
}