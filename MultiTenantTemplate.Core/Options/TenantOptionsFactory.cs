using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MultiTenantTemplate.Core.Accessors.IAccessors;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Options
{
    /// <summary>
    /// Create a new options instance with configuration applied
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class TenantOptionsFactory<TOptions, T> : IOptionsFactory<TOptions>
        where TOptions : class, new()
        where T: Tenant
    {
        #region Fields

        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;
        private readonly Action<TOptions, T> _tenantConfig;
        private readonly ITenantAccessor<T> _tenantAccessor;

        #endregion

        #region Constructor

        public TenantOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, 
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, 
            Action<TOptions, T> tenantConfig, 
            ITenantAccessor<T> tenantAccessor)
        {
            _setups = setups;
            _postConfigures = postConfigures;
            _tenantConfig = tenantConfig;
            _tenantAccessor = tenantAccessor;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new options instance
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TOptions Create(string name)
        {
            var options = new TOptions();

            ApplyOptionsSetupConfiguration(options, name);
            ApplyTenantSpecificConfiguration(options);
            ApplyPostConfiguration(options, name);

            return options;
        }

        #endregion

        #region Private Methods

        private void ApplyOptionsSetupConfiguration(TOptions options, string name)
        {
            foreach (var setup in _setups)
            {
                if(setup is IConfigureNamedOptions<TOptions> namedSetup)
                    namedSetup.Configure(name, options);
                else
                    setup.Configure(options);
            }
        }

        private void ApplyTenantSpecificConfiguration(TOptions options)
        {
            if (_tenantAccessor.Tenant != null)
                _tenantConfig(options, _tenantAccessor.Tenant);
        }

        private void ApplyPostConfiguration(TOptions options, string name)
        {
            foreach (var postConfig in _postConfigures)
                postConfig.PostConfigure(name, options);
        }

        #endregion
    }
}
