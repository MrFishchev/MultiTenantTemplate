using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MultiTenantTemplate.Core.Services;
using MultiTenantTemplate.Core.Stores.IStores;
using MultiTenantTemplate.Core.Strategies.IStrategies;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Classes
{
    public class TenantBuilder<T> where T : Tenant
    {
        #region Fields

        private readonly IServiceCollection _services;

        #endregion

        #region Constructor

        public TenantBuilder(IServiceCollection services)
        {
            _services.AddTransient<TenantAccessService<T>>();
            _services = services;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithResolutionStrategy<S>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where S : class, ITenantResolutionStrategy
        {
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(S), lifetime));
            return this;
        }

        public TenantBuilder<T> WithStore<S>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where S : class, ITenantStore<T>
        {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(S), lifetime));
            return this;
        }


        #endregion
    }
}
