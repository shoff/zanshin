namespace Zanshin.Domain.Entities.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>
        /// The user profile identifier.
        /// </value>
        [Key]
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the birth day.
        /// </summary>
        /// <value>
        /// The birth day.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [MaxLength(200)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the sig.
        /// </summary>
        /// <value>
        /// The sig.
        /// </value>
        [MaxLength(512)]
        public string Sig { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow HTML sig].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow HTML sig]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowHtmlSig { get; set; }

        /// <summary>
        /// Gets or sets the facebook page.
        /// </summary>
        /// <value>
        /// The facebook page.
        /// </value>
        [MaxLength(512)]
        public string FacebookPage { get; set; }

        /// <summary>
        /// Gets or sets the name of the skype user.
        /// </summary>
        /// <value>
        /// The name of the skype user.
        /// </value>
        [MaxLength(256)]
        public string SkypeUserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the twitter.
        /// </summary>
        /// <value>
        /// The name of the twitter.
        /// </value>
        [MaxLength(50)]
        public string TwitterName { get; set; }

        /// <summary>
        /// Gets or sets the home page.
        /// </summary>
        /// <value>
        /// The home page.
        /// </value>
        [MaxLength(512)]
        public string HomePage { get; set; }

    }
}