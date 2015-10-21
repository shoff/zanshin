namespace Zanshin.Domain.Repositories
{
    using System;
    using System.Linq;

    using Zanshin.Domain.Data.Interfaces;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Repositories.Interfaces;

    public sealed class GeoLocationRepository : EntityRepository<GeoLocation, int>, IGeoLocationRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocationRepository" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public GeoLocationRepository(IDataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Generic method for updating an Entity in the context
        /// </summary>
        /// <param name="entityToUpdate">The story to Update</param>
        public void Update(GeoLocation entityToUpdate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <param name="geoLocation">The geo location.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">geoLocation</exception>
        public GeoLocation AddIfNotExists(GeoLocation geoLocation)
        {
            if (geoLocation == null)
            {
                throw new ArgumentNullException("geoLocation");
            }

            IQueryable<GeoLocation> query = this.dataContext.SetEntity<GeoLocation>();
            query = query.Where(x => x.Address == geoLocation.Address).Select(x => x);

            var location = query.FirstOrDefault();

            if (location == null)
            {
                // ok add this entity 
                this.Insert(geoLocation);
                this.Context.Commit();
                return geoLocation;
            }
            return location;
        }
    }
}