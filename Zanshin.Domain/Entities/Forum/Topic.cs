namespace Zanshin.Domain.Entities.Forum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Zanshin.Domain.Collections;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Topic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Topic"/> class.
        /// </summary>
        public Topic()
        {
            this.Posts = new List<Post>();
            this.Tags = new List<Tag>();
        }

        /// <summary>
        /// Gets or sets the topic identifier.
        /// </summary>
        /// <value>
        /// The topic identifier.
        /// </value>
        [Key]
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [Required, StringLength(255)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last post date.
        /// </summary>
        /// <value>
        /// The last post date.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime LastPostDate { get; set; }

        /// <summary>
        /// Gets or sets the forum identifier.
        /// </summary>
        /// <value>
        /// The forum identifier.
        /// </value>
        public int ForumId { get; set; }

        /// <summary>
        /// Gets or sets the post count.
        /// </summary>
        /// <value>
        /// The post count.
        /// </value>
        public int PostCount { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [ForeignKey("CreatedBy")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public User CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the forum.
        /// </summary>
        /// <value>
        /// The name of the forum.
        /// </value>
        public string ForumName { get; set; }

        /// <summary>
        /// Gets or sets the name of the topic starter.
        /// </summary>
        /// <value>
        /// The name of the topic starter.
        /// </value>
        public string TopicStarterName { get; set; }

        /// <summary>
        /// Gets or sets the topic icon.
        /// </summary>
        /// <value>
        /// The topic icon.
        /// </value>
        public string TopicIcon { get; set; }

        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        /// <value>
        /// The views.
        /// </value>
        [Required]
        public int Views { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Topic"/> is sticky.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sticky; otherwise, <c>false</c>.
        /// </value>
        public bool Sticky { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Topic"/> is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if locked; otherwise, <c>false</c>.
        /// </value>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Topic"/> is moved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if moved; otherwise, <c>false</c>.
        /// </value>
        public bool Moved { get; set; }

        /// <summary>
        /// Gets or sets the moved to topic identifier.
        /// </summary>
        /// <value>
        /// The moved to topic identifier.
        /// </value>
        public int? MovedToTopicId { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        /// <summary>
        /// Gets or sets the moved reason.
        /// </summary>
        /// <value>
        /// The moved reason.
        /// </value>
        [MaxLength(60)]
        public string MovedReason { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public ICollection<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the paged posts.
        /// </summary>
        /// <value>
        /// The paged posts.
        /// </value>
        [NotMapped]
        public SerializablePagination<Post> PagedPosts { get; set; }
    }
}