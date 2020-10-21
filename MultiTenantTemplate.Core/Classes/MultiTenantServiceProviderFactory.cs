using Autofac;
using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Classes
{
    public class MultiTenantServiceProviderFactory<T> : IServiceProviderFactory<ContainerBuilder>
        where T : Tenant
    {
        #region Fields

        public Action<T, ContainerBuilder> _tenantServiceConfiguration;

        #endregion

        #region Constructor

        public MultiTenantServiceProviderFactory(Action<T, ContainerBuilder> tenantServiceConfiguration)
        {
            _tenantServiceConfiguration = tenantServiceConfiguration;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a builder populated with global services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            return builder;
        }

        /// <summary>
        /// Create custom service provider
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            MultiTenantContainer<T> container = null;
            Func<MultiTenantContainer<T>> containerAccessor = () => { return container; };

            containerBuilder.RegisterInstance(containerAccessor)
                .SingleInstance();

            container = new MultiTenantContainer<T>(containerBuilder.Build(),
                _tenantServiceConfiguration);

            return new AutofacServiceProvider(containerAccessor());
        }

        #endregion
    }
}
