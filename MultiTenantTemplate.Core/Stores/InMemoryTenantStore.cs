using System;
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
                new Tenant {Id = Guid.Parse("ac2d0b1e-126a-4d77-94c8-cc85049b528f"), Identifier = "localhost"},
                new Tenant {Id = Guid.Parse("24706142-114c-4110-aaab-bc388e2bf3ea"), Identifier = "t01"},
                new Tenant {Id = Guid.Parse("8b00deaa-870f-4684-a677-e8b0bb487331"), Identifier = "t02"}
            }.SingleOrDefault(t => string.Equals(t.Identifier, identifier, StringComparison.CurrentCultureIgnoreCase));

            return Task.FromResult(tenant);
        }
    }
}
