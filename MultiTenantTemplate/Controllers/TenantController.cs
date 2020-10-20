using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiTenantTemplate.Core.Extensions;
using MultiTenantTemplate.Core.Services;
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


        #endregion

        #region Constructor


        #endregion

        #region Actions

        [HttpGet]
        public Task<string> GetCurrentTenantIdentifier()
        {
            return Task.FromResult(HttpContext.GetTenant().Identifier);
        }

        #endregion
    }
}
