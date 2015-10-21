namespace Zanshin.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    
    public sealed class PrivateMessage
    {
        /// <summary>
        /// Gets or sets the private message identifier.
        /// </summary>
        /// <value>
        /// The private message identifier.
        /// </value>
        [Key]
        public int PrivateMessageId { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [StringLength(100)]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [Required, StringLength(255)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [Required, StringLength(4000)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets from user identifier.
        /// </summary>
        /// <value>
        /// From user identifier.
        /// </value>
        [Required, ForeignKey("FromUser")]
        public int FromUserId { get; set; }

        /// <summary>
        /// Gets or sets from user.
        /// </summary>
        /// <value>
        /// From user.
        /// </value>
        public User FromUser { get; set; }

        /// <summary>
        /// Gets or sets to user identifier.
        /// </summary>
        /// <value>
        /// To user identifier.
        /// </value>
        [Required, ForeignKey("ToUser")]
        public int ToUserId { get; set; }

        /// <summary>
        /// Gets or sets to user.
        /// </summary>
        /// <value>
        /// To user.
        /// </value>
        public User ToUser { get; set; }

        /// <summary>
        /// Gets or sets the date sent.
        /// </summary>
        /// <value>
        /// The date sent.
        /// </value>
        [Required]
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Gets or sets the date seen.
        /// </summary>
        /// <value>
        /// The date seen.
        /// </value>
        public DateTime? DateSeen { get; set; }
    }
}