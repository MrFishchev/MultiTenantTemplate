using System.Threading.Tasks;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Stores.IStores
{
    public interface ITenantStore<T> where T : Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}
