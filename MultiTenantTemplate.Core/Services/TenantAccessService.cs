using System.Threading.Tasks;
using MultiTenantTemplate.Core.Stores.IStores;
using MultiTenantTemplate.Core.Strategies.IStrategies;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Services
{
    public class TenantAccessService<T> where T : Tenant
    {
        #region Fields

        private readonly ITenantResolutionStrategy _tenResolutionStrategy;
        private readonly ITenantStore<T> _tenantStore;

        #endregion

        #region Constructor

        public TenantAccessService(ITenantResolutionStrategy tenResolutionStrategy, ITenantStore<T> tenantStore)
        {
            _tenResolutionStrategy = tenResolutionStrategy;
            _tenantStore = tenantStore;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the current tenant
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetTenantAsync()
        {
            var tenantIdentifier = await _tenResolutionStrategy.GetTenantIdentifierAsync();
            return await _tenantStore.GetTenantAsync(tenantIdentifier);

        }

        #endregion
    }
}
