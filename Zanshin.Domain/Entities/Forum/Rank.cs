namespace Zanshin.Domain.Entities.Forum
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// </summary>
    public class Rank
    {
        /// <summary>
        /// Gets or sets the rank identifier.
        /// </summary>
        /// <value>
        /// The rank identifier.
        /// </value>
        [Key]
        public int RankId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rank.
        /// </summary>
        /// <value>
        /// The name of the rank.
        /// </value>
        [MaxLength(128)]
        public string RankName { get; set; }
        
        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        [MaxLength(512)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the required post count.
        /// </summary>
        /// <value>
        /// The required post count.
        /// </value>
        public int RequiredPostCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [special rank].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [special rank]; otherwise, <c>false</c>.
        /// </value>
        public bool SpecialRank { get; set; }
    }
}