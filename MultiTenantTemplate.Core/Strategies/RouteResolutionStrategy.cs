using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MultiTenantTemplate.Core.Strategies.IStrategies;

namespace MultiTenantTemplate.Core.Strategies
{
    /// <summary>
    /// Resolving tenant identifier by route path (ex. wwww.hostname.com/tenant/)
    /// </summary>
    public class RouteResolutionStrategy : ITenantResolutionStrategy
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public RouteResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <returns></returns>
        public Task<string> GetTenantIdentifierAsync()
        {
            if (_httpContextAccessor.HttpContext == null)
                return Task.FromResult(string.Empty);

            var parts = _httpContextAccessor.HttpContext.Request
                .Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2 || parts[0] != "api")
                return Task.FromResult(string.Empty);

            return Task.FromResult(parts[1]);
        }

        #endregion
    }
}
