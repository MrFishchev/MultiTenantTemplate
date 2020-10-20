using Microsoft.Extensions.DependencyInjection;
using MultiTenantTemplate.Core.Classes;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Extensions
{
    /// <summary>
    /// One of the way to create a tenant builder
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services (application specific tenant class)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TenantBuilder<T> AddMultiTenancy<T>(this IServiceCollection services)
            where T : Tenant => new TenantBuilder<T>(services);

        /// <summary>
        /// Add the services (default tenant class)
        /// </summary>
        /// <typeparam name="Tenant"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TenantBuilder<Tenant> AddMultiTenancy(this IServiceCollection services)
            => new TenantBuilder<Tenant>(services);
    }
}
