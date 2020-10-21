using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace MultiTenantTemplate.Core.Options
{
    /// <summary>
    /// Dictionary of tenant specific options caches
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class TenantOptionsCacheDictionary<TOptions> where  TOptions : class
    {
        #region Fields

        /// <summary>
        /// Caches stored in memory
        /// </summary>
        private readonly ConcurrentDictionary<Guid, IOptionsMonitorCache<TOptions>> _tenantSpecificOptionCaches =
            new ConcurrentDictionary<Guid, IOptionsMonitorCache<TOptions>>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Get options for a specific tenant (create if not exists)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public IOptionsMonitorCache<TOptions> Get(Guid tenantId)
        {
            return _tenantSpecificOptionCaches.GetOrAdd(tenantId, new OptionsCache<TOptions>());
        }

        #endregion
    }
}
