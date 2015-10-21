using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zanshin.Domain.Entities
{
    

    /// <summary>
    /// </summary>
    
    public sealed class Website
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Website"/> class.
        /// </summary>
        public Website()
        {
            this.Tags = new List<Tag>();
        }

        /// <summary>
        /// Gets or sets the website identifier.
        /// </summary>
        /// <value>
        /// The website identifier.
        /// </value>
        [Key]
        public int WebsiteId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required, MaxLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ICollection<Tag> Tags { get; set; }
    }
}