using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using MultiTenantTemplate.Core.Services;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Classes
{
    public class MultiTenantContainer<T> : IContainer where T : Tenant
    {
        #region Fields

        private readonly IContainer _applicationContainer;
        private readonly Action<T, ContainerBuilder> _tenantContainerConfiguration;

        //Keeps track of all of the tenant scopes
        private readonly Dictionary<Guid, ILifetimeScope> _tenantLifetimeScopes =
            new Dictionary<Guid, ILifetimeScope>();

        private readonly object _lock = new object();
        private const string MultiTenantTag = "multitenantcontainer";

        #endregion

        #region Properties

        public IComponentRegistry ComponentRegistry => GetCurrentTenantScope().ComponentRegistry;
        public IDisposer Disposer => GetCurrentTenantScope().Disposer;
        public object Tag => GetCurrentTenantScope().Tag;
        public DiagnosticListener DiagnosticSource => _applicationContainer.DiagnosticSource;

        #endregion

        #region Events

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning;
        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding;
        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning;

        #endregion

        #region Constructor

        public MultiTenantContainer(IContainer applicationContainer, Action<T, ContainerBuilder> tenantContainerConfiguration)
        {
            _applicationContainer = applicationContainer;
            _tenantContainerConfiguration = tenantContainerConfiguration;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the scope of the current tenant
        /// </summary>
        /// <returns></returns>
        public ILifetimeScope GetCurrentTenantScope()
        {
            return GetTenantScope(GetCurrentTenant()?.Id);
        }

        /// <summary>
        /// Get or configure if missing
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public ILifetimeScope GetTenantScope(Guid? tenantId)
        {
            //early in the pipeline, just use the application container
            if (tenantId == null || tenantId == Guid.Empty)
                return _applicationContainer;

            //lifetime for a tenant was created
            if (_tenantLifetimeScopes.ContainsKey((Guid)tenantId))
                return _tenantLifetimeScopes[(Guid)tenantId];

            //this is a new tenant
            lock (_lock)
            {
                //configure a new lifetimescope for it using our tenant sensitive configuration method
                _tenantLifetimeScopes.Add((Guid) tenantId,
                    _applicationContainer.BeginLifetimeScope(MultiTenantTag,
                        c => _tenantContainerConfiguration(GetCurrentTenant(), c)));
                return _tenantLifetimeScopes[(Guid) tenantId];
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var scope in _tenantLifetimeScopes)
                    scope.Value.Dispose();
                _applicationContainer.Dispose();
            }
        }

        public object ResolveComponent(ResolveRequest request)
        {
            return GetCurrentTenantScope().ResolveComponent(request);
        }

        public ValueTask DisposeAsync()
        {
            return GetCurrentTenantScope().DisposeAsync();
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return GetCurrentTenantScope().BeginLifetimeScope();
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            return GetCurrentTenantScope().BeginLifetimeScope(tag);
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return GetCurrentTenantScope().BeginLifetimeScope(configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return GetCurrentTenantScope().BeginLifetimeScope(tag, configurationAction);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the current tenant from application container
        /// </summary>
        /// <returns></returns>
        private T GetCurrentTenant()
        {
            return _applicationContainer.Resolve<TenantAccessService<T>>()
                .GetTenantAsync().GetAwaiter().GetResult();
        }

        #endregion
    }
}
