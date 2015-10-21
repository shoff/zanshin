using System.Collections.Generic;

namespace Zanshin.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    

    /// <summary>
    /// 
    /// </summary>
    
    public sealed class Avatar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class.
        /// </summary>
        public Avatar()
        {
            this.Tags = new List<Tag>();
        }


        /// <summary>
        /// Gets or sets the avatar identifier.
        /// </summary>
        /// <value>
        /// The avatar identifier.
        /// </value>
        [Key]
        public int AvatarId { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        [Required, MaxLength(512)]
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the MIME.
        /// </summary>
        /// <value>
        /// The type of the MIME.
        /// </value>
        [Required, MaxLength(30)]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Avatar"/> is display.
        /// </summary>
        /// <value>
        ///   <c>true</c> if display; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool Display { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        [Required]
        public int Weight { get; set; }

        /// <summary>
        /// Gets or sets the user count.
        /// </summary>
        /// <value>
        /// The user count.
        /// </value>
        [Required]
        public int UserCount { get; set; }


        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }
    }
}