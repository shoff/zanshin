namespace Zanshin.Domain.Entities.Forum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Post : IComparable<Post>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class.
        /// </summary>
        public Post()
        {
            this.Tags = new List<Tag>();
        }

        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [Key]
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [MaxLength(60), Required(ErrorMessageResourceType = typeof (Common), ErrorMessageResourceName = "SubjectIsRequried")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [ForeignKey("Poster")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        /// <value>
        /// The poster.
        /// </value>
        [Required]
        public User Poster { get; set; }

        /// <summary>
        /// Gets or sets the topic identifier.
        /// </summary>
        /// <value>
        /// The topic identifier.
        /// </value>
        [ForeignKey("PostTopic")]
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the post topic.
        /// </summary>
        /// <value>
        /// The post topic.
        /// </value>
        [Required]
        public Topic PostTopic { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [Required, DataType(DataType.MultilineText)]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the reply to post identifier.
        /// </summary>
        /// <value>
        /// The reply to post identifier.
        /// </value>
        [ForeignKey("ReplyToPost")]
        public int? ReplyToPostId { get; set; }

        /// <summary>
        /// Gets or sets the reply to post.
        /// </summary>
        /// <value>
        /// The reply to post.
        /// </value>
        public Post ReplyToPost { get; set; }

        /// <summary>
        /// Gets or sets the forum identifier.
        /// </summary>
        /// <value>
        /// The forum identifier.
        /// </value>
        [Required]
        public int ForumId { get; set; }

        /// <summary>
        /// Gets or sets the post karma.
        /// </summary>
        /// <value>
        /// The post karma.
        /// </value>
        [Required]
        [Display(ResourceType = typeof(Common), Name = "DisplayAttributeKarma")]
        public int PostKarma { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        /// <value>
        /// The date updated.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(Post other)
        {
            return this.DateCreated.CompareTo(other.DateCreated);
        }
    }
}