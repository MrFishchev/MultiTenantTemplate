using System.Linq;
using System.Threading.Tasks;
using MultiTenantTemplate.Core.Stores.IStores;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Stores
{
    public class InMemoryTenantStore : ITenantStore<Tenant>
    {
        /// <summary>
        /// Get a tenant for a given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Task<Tenant> GetTenantAsync(string identifier)
        {
            var tenant = new[]
            {
                new Tenant {Id = "ac2d0b1e-126a-4d77-94c8-cc85049b528f", Identifier = "localhost"}
            }.SingleOrDefault(t => t.Identifier == identifier);

            return Task.FromResult(tenant);
        }
    }
}
