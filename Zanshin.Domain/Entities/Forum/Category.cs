namespace Zanshin.Domain.Entities.Forum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Category
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        public Category()
        {
            this.Forums = new List<Forum>();
            this.Tags = new HashSet<Tag>();
        }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required, StringLength(30)]
        public string Name { get; set; }

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
        [Required]
        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; }

        ///// <summary>
        ///// Gets or sets the category image.
        ///// </summary>
        ///// <value>
        ///// The category image.
        ///// </value>
        //[StringLength(512)]
        //public string CategoryImage { get; set;}

        /// <summary>
        /// Gets or sets the category order.
        /// </summary>
        /// <value>
        /// The category order.
        /// </value>
        public int CategoryOrder { get; set; }

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        /// <value>
        /// The category description.
        /// </value>
        [MaxLength(128)]
        public string CategoryDescription { get; set; }
        
        /// <summary>
        /// Gets or sets the forum count.
        /// </summary>
        /// <value>
        /// The forum count.
        /// </value>
        public int ForumCount { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the forums.
        /// </summary>
        /// <value>
        /// The forums.
        /// </value>
        public ICollection<Forum> Forums { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }

    }
}