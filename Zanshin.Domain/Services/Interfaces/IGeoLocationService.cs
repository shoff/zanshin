namespace Zanshin.Domain.Services.Interfaces
{
    using System;

    using Zanshin.Domain.Entities;

    public interface IGeoLocationService
    {
        /// <summary>
        /// Fetches the specified ipaddress.
        /// </summary>
        /// <param name="ipaddress">The ipaddress.</param>
        /// <param name="persistLocation">if set to <c>true</c> [persist location].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">ipaddress</exception>
        GeoLocation Fetch(string ipaddress, bool persistLocation = true);
    }
}