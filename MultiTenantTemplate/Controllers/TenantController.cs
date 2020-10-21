using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiTenantTemplate.Core.Accessors.IAccessors;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Controllers
{
    /// <summary>
    /// A controller for tenant management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        #region Fields

        private readonly ITenantAccessor<Tenant> _tenantAccessor;

        #endregion

        #region Constructor

        public TenantController(ITenantAccessor<Tenant> tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        #endregion

        #region Actions

        [HttpGet]
        public Task<string> GetCurrentTenantIdentifier()
        {
            return Task.FromResult(_tenantAccessor.Tenant?.Identifier);
        }

        #endregion
    }
}
