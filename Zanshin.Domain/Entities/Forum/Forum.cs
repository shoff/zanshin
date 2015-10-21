namespace Zanshin.Domain.Entities.Forum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Zanshin.Domain.Collections;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    [Table("Forums")]
    public sealed class Forum
    {
        // TODO Add required groups object.

        /// <summary>
        /// Initializes a new instance of the <see cref="Forum"/> class.
        /// </summary>
        public Forum()
        {
            this.Topics = new List<Topic>();
            this.PostsPerPage = 20;
            this.TopicsPerPage = 20;
            this.HotTopicThreashold = 10;
            this.RequiredRoles = new HashSet<string>();
            this.Tags = new HashSet<Tag>();
        }

        /// <summary>
        /// Gets or sets the forum identifier.
        /// </summary>
        /// <value>
        /// The forum identifier.
        /// </value>
        [Key]
        public int ForumId { get; set; }

        /// <summary>
        /// Gets or sets the post count.
        /// </summary>
        /// <value>
        /// The post count.
        /// </value>
        [Required]
        public int PostCount { get; set; }

        /// <summary>
        /// Gets or sets the topic count.
        /// </summary>
        /// <value>
        /// The topic count.
        /// </value>
        [Required]
        public int TopicCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow HTML].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow HTML]; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool AllowHtml { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow bb code].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow bb code]; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool AllowBBCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow sigs].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow sigs]; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool AllowSigs { get; set; }

        /// <summary>
        /// Gets or sets the posts per page.
        /// </summary>
        /// <value>
        /// The posts per page.
        /// </value>
        [Required]
        public int PostsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the topics per page.
        /// </summary>
        /// <value>
        /// The topics per page.
        /// </value>
        [Required]
        public int TopicsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the hot topic threashold.
        /// </summary>
        /// <value>
        /// The hot topic threashold.
        /// </value>
        [Required]
        public int HotTopicThreashold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is private; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the required roles.
        /// </summary>
        /// <value>
        /// The required roles.
        /// </value>
        public ICollection<string> RequiredRoles { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required, StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the forum description.
        /// </summary>
        /// <value>
        /// The forum description.
        /// </value>
        [StringLength(128)]
        public string ForumDescription { get; set; }

        /// <summary>
        /// Gets or sets the forum password.
        /// </summary>
        /// <value>
        /// The forum password.
        /// </value>
        [MaxLength(128)]
        public string ForumPassword { get; set; }

        /// <summary>
        /// Gets or sets the forum image location, relative to 
        /// the root directory, of an additional image to associate
        /// with this forum.
        /// </summary>
        /// <value>
        /// The forum image.
        /// </value>
        [MaxLength(512)]
        public string ForumImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow indexing].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow indexing]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowIndexing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [display active topics].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display active topics]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayActiveTopics { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [Required, DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [Required, ForeignKey("ForumCategory")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the forum category.
        /// </summary>
        /// <value>
        /// The forum category.
        /// </value>
        public Category ForumCategory { get; set; }

        /// <summary>
        /// Gets or sets the moderator identifier.
        /// </summary>
        /// <value>
        /// The moderator identifier.
        /// </value>
        [ForeignKey("ForumModerator")]
        public int ModeratorId { get; set; }

        /// <summary>
        /// Gets or sets the forum moderator.
        /// </summary>
        /// <value>
        /// The forum moderator.
        /// </value>
        public User ForumModerator { get; set; }

        /// <summary>
        /// Gets or sets the moderator group identifier.
        /// </summary>
        /// <value>
        /// The moderator group identifier.
        /// </value>
        [Required, ForeignKey("ModeratorGroup")]
        public int ModeratorGroupId { get; set; }

        /// <summary>
        /// Gets or sets the moderator group.
        /// </summary>
        /// <value>
        /// The moderator group.
        /// </value>
        public Group ModeratorGroup { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public ICollection<Topic> Topics { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        [DataType(DataType.DateTime)]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the paged topics.
        /// </summary>
        /// <value>
        /// The paged topics.
        /// </value>
        [NotMapped]
        public SerializablePagination<Topic> PagedTopics { get; set; }
    }
}