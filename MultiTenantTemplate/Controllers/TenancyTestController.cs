using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantTemplate.Services;

namespace MultiTenantTemplate.Controllers
{
    [AllowAnonymous]
    [Route("api/{tenant}/test")]
    [ApiController]
    public class TenancyTestController : ControllerBase
    {
        #region Fields

        private readonly TenantSpecificTestService _tenantSpecificService;
        private readonly ApplicationSpecificTestService _applicationSpecificService;

        #endregion

        #region Constructor

        public TenancyTestController(TenantSpecificTestService tenantSpecificService, ApplicationSpecificTestService applicationSpecificService)
        {
            _tenantSpecificService = tenantSpecificService;
            _applicationSpecificService = applicationSpecificService;
        }

        #endregion

        #region Actions

        [HttpGet("tenant")]
        public async Task<IActionResult> GetTenantSpecificId()
        {
            var result = Task.FromResult(_tenantSpecificService.Id);
            return Ok(await result);
        }

        [HttpGet("application")]
        public async Task<IActionResult> GetApplicationSpecificId()
        {
            var result = Task.FromResult(_applicationSpecificService.Id);
            return Ok(await result);
        }

        #endregion
    }
}
