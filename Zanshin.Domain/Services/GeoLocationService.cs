namespace Zanshin.Domain.Services
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Interfaces;
    using Zanshin.Domain.Repositories.Interfaces;
    using Zanshin.Domain.Services.Interfaces;

    public sealed class GeoLocationService : IGeoLocationService, ITransientLifestyle
    {
        private readonly IGeoLocationRepository geoLocationRepository;
        private string url = "http://freegeoip.net/json/";
        //private string ripeUrl = "https://rest.db.ripe.net/search.json?query-string={0}&source=ripe";

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocationService" /> class.
        /// </summary>
        /// <param name="geoLocationRepository">The geo location repository.</param>
        public GeoLocationService(IGeoLocationRepository geoLocationRepository)
        {
            this.geoLocationRepository = geoLocationRepository;
        }

        /// <summary>
        /// Fetches the specified ipaddress.
        /// </summary>
        /// <param name="ipaddress">The ipaddress.</param>
        /// <param name="persistLocation">if set to <c>true</c> [persist location].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">ipaddress</exception>
        public GeoLocation Fetch(string ipaddress, bool persistLocation = true)
        {
            if (string.IsNullOrEmpty(ipaddress))
            {
                throw new ArgumentNullException("ipaddress");
            }

            if ((ipaddress == "127.0.0.1") || (ipaddress == "::1"))
            {
                ipaddress = ConfigurationManager.AppSettings["HomeIp"];
            }

            Uri uri = new Uri(this.url + ipaddress);
            HttpClient httpClient = new HttpClient();
            Task<string> geoTask = httpClient.GetStringAsync(uri);
            JObject location = JObject.Parse(geoTask.Result);

            if (location == null)
            {
                return this.InvalidLocation(ipaddress);
            }

            var geoLocation = this.ParseFreeGeoIp(location);

            if (persistLocation)
            {
                return this.geoLocationRepository.AddIfNotExists(geoLocation);
            }
            return geoLocation;
        }

        //private GeoLocation Parse(JObject jobject)
        //{
        //    GeoLocation location = new GeoLocation
        //    {
        //        DateCreated = DateTime.Now,
        //        LastSeen = DateTime.Now
        //    };

        //    location.Country = (string)jobject.SelectToken("country");
        //    location.Address = (string)jobject.SelectToken("query");
        //    location.CountryCode = (string)jobject.SelectToken("countryCode");
        //    location.Region = (string)jobject.SelectToken("region");
        //    location.RegionName = (string)jobject.SelectToken("country");
        //    location.City = (string)jobject.SelectToken("city");
        //    location.Zip = (string)jobject.SelectToken("zip");
        //    location.Latitude = (string)jobject.SelectToken("lat");
        //    location.Longitude = (string)jobject.SelectToken("lon");
        //    location.TimeZone = (string)jobject.SelectToken("timezone");
        //    location.Isp = (string)jobject.SelectToken("isp");
        //    location.Organization = (string)jobject.SelectToken("org");
        //    location.As = (string)jobject.SelectToken("as");
        //    return location;
        //}

        private GeoLocation ParseFreeGeoIp(JObject jobject)
        {
            GeoLocation location = new GeoLocation
            {
                DateCreated = DateTime.Now,
                LastSeen = DateTime.Now
            };

            location.Country = (string)jobject.SelectToken("country_name");
            location.Address = (string)jobject.SelectToken("ip");
            location.CountryCode = (string)jobject.SelectToken("country_code");
            location.Region = (string)jobject.SelectToken("region_code");
            location.RegionName = (string)jobject.SelectToken("region_name");
            location.City = (string)jobject.SelectToken("city");
            location.Zip = (string)jobject.SelectToken("zipcode");
            location.Latitude = (string)jobject.SelectToken("latitude");
            location.Longitude = (string)jobject.SelectToken("longitude");
            //location.TimeZone = (string)jobject.SelectToken("timezone");
            location.Isp = (string)jobject.SelectToken("isp");
            location.Organization = (string)jobject.SelectToken("metro_code");
            location.As = (string)jobject.SelectToken("areacode");
            return location;
        }

        private GeoLocation InvalidLocation(string ipaddress)
        {
            GeoLocation location = new GeoLocation
            {
                DateCreated = DateTime.Now,
                LastSeen = DateTime.Now
            };

            location.Address = ipaddress;
            return location;
        }
    }
}