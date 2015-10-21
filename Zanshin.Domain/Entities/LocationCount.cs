namespace Zanshin.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    /// <summary>
    /// 
    /// </summary>
    public sealed class LocationCount
    {
        /// <summary>
        /// Gets or sets the location count id.
        /// </summary>
        /// <value>
        /// The location count id.
        /// </value>
        [Key]
        public int LocationCountId { get; set; }

        /// <summary>
        /// Gets or sets the geo location id.
        /// </summary>
        /// <value>
        /// The geo location id.
        /// </value>
        [Required, ForeignKey("GeoLocation")]
        public int GeoLocationId { get; set; }

        /// <summary>
        /// Gets or sets the geo location.
        /// </summary>
        /// <value>
        /// The geo location.
        /// </value>
        public GeoLocation GeoLocation { get; set; }

        /// <summary>
        /// Gets or sets the hit count.
        /// </summary>
        /// <value>
        /// The hit count.
        /// </value>
        [Required]
        public int HitCount { get; set; }
    }
}