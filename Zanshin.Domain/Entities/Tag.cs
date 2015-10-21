using System.Collections.Generic;

namespace Zanshin.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    
    public sealed class Tag 
    {
        public Tag()
        {
            this.Posts = new List<Post>();
            this.Forums = new List<Forum.Forum>();
            this.Topics = new List<Topic>();
            this.Categories = new List<Category>();
            this.Users = new List<User>();
            this.Avatars = new List<Avatar>();
            this.Websites = new List<Website>();

        }

        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
        /// </value>
        [Key]
        public int TagId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [Required, MaxLength(20)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public ICollection<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the forums.
        /// </summary>
        /// <value>
        /// The forums.
        /// </value>
        public ICollection<Forum.Forum> Forums { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public ICollection<Topic> Topics { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ICollection<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the websites.
        /// </summary>
        /// <value>
        /// The websites.
        /// </value>
        public ICollection<Website> Websites { get; set; }

        /// <summary>
        /// Gets or sets the avatars.
        /// </summary>
        /// <value>
        /// The avatars.
        /// </value>
        public ICollection<Avatar> Avatars { get; set; }
    }
}