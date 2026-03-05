namespace Nager.EuropeanCommissionVies
{
    /// <summary>
    /// Represents the configuration options for the VIES client, 
    /// specifically for managing caching behavior.
    /// </summary>
    public class ViesClientOptions
    {
        /// <summary>
        /// Gets or sets the duration in minutes for which VIES API responses 
        /// should be stored in the cache.
        /// </summary>
        /// <value>
        /// The number of minutes before a cache entry expires. 
        /// The default value is 60 minutes.
        /// </value>
        public int CacheMinutes { get; set; } = 60;
    }
}
