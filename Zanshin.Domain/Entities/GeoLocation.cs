namespace Zanshin.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    
    public sealed class GeoLocation
    {
        /// <summary>
        /// Gets or sets the geo location identifier.
        /// </summary>
        /// <value>
        /// The geo location identifier.
        /// </value>
        [Key]
        public int GeoLocationId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [Required, MaxLength(16)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        [MaxLength(60)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        [MaxLength(8)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        [MaxLength(40)]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the name of the region.
        /// </summary>
        /// <value>
        /// The name of the region.
        /// </value>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [MaxLength(256)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the isp.
        /// </summary>
        /// <value>
        /// The isp.
        /// </value>
        public string Isp { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        /// <value>
        /// The organization.
        /// </value>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets as.
        /// </summary>
        /// <value>
        /// As.
        /// </value>
        public string As { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the last seen.
        /// </summary>
        /// <value>
        /// The last seen.
        /// </value>
        [Required]
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }
    }

    /*
    "status": "success",
    "country": "United States",
    "countryCode": "US",
    "region": "MN",
    "regionName": "Minnesota",
    "city": "Saint Paul",
    "zip": "",
    "lat": "44.9444",
    "lon": "-93.0933",
    "timezone": "America/Chicago",
    "isp": "CenturyLink",
    "org": "Patterson Companies",
    "as": "AS32630 Patterson Companies, Inc.",
    "query": "63.234.242.12"
     */
}