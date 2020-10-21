using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Classes
{

    /// <summary>
    /// Any middleware registered after this, will resolve services
    /// using the current Tenant LifeTimeScope instead of application level container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiTenantContainerMiddleware<T> where T : Tenant
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Contstructor

        public MultiTenantContainerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task Invoke(HttpContext context,
            Func<MultiTenantContainer<T>> multiTenantContainerAccessor)
        {
            //Set to current tenant container.
            //Begin new scope for request as standard scope per-request
            context.RequestServices =
                new AutofacServiceProvider(multiTenantContainerAccessor()
                    .GetCurrentTenantScope().BeginLifetimeScope());

            await _next.Invoke(context);
        }

        #endregion
    }
}
