using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MultiTenantTemplate.Core.Strategies.IStrategies;

namespace MultiTenantTemplate.Core.Strategies
{
    /// <summary>
    /// Resolving tenant identifier by host name (ex. wwww.hostname.com)
    /// </summary>
    public class HostResolutionStrategy : ITenantResolutionStrategy
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public HostResolutionStrategy(IHttpContextAccessor httpContextAccessor)
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
            return _httpContextAccessor.HttpContext != null
                ? Task.FromResult(_httpContextAccessor.HttpContext.Request.Host.Host)
                : Task.FromResult(string.Empty);
        }

        #endregion
    }
}
