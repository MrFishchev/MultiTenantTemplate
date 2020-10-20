using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MultiTenantTemplate.Common;
using MultiTenantTemplate.Core.Services;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Classes
{
    public class TenantMiddleware<T> where T: Tenant
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            if (!context.Items.ContainsKey(Constants.HttpContextTenantKey))
            {
                var tenantService = (TenantAccessService<T>) context.RequestServices
                    .GetService(typeof(TenantAccessService<T>));

                context.Items.Add(Constants.HttpContextTenantKey, await tenantService.GetTenantAsync());
            }

            if (_next != null)
                await _next(context);
        }

        #endregion
    }
}
