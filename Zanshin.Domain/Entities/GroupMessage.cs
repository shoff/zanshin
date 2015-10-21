namespace Zanshin.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    
    public class GroupMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMessage"/> class.
        /// </summary>
        public GroupMessage()
        {
            this.ReadBy = new List<MessageReadByUser>();
        }

        /// <summary>
        /// Gets or sets the group message identifier.
        /// </summary>
        /// <value>
        /// The group message identifier.
        /// </value>
        [Key]
        public int GroupMessageId { get; set; }

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
        /// Gets or sets the date sent.
        /// </summary>
        /// <value>
        /// The date sent.
        /// </value>
        [Required]
        public DateTime DateSent { get; set; }

        /// <summary>
        /// Gets or sets the read by.
        /// </summary>
        /// <value>
        /// The read by.
        /// </value>
        public ICollection<MessageReadByUser> ReadBy { get; set; }
    }
}