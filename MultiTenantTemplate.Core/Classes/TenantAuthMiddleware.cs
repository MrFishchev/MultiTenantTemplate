using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MultiTenantTemplate.Core.Classes
{
    /// <summary>
    /// Middleware that performs authentication
    /// </summary>
    public class TenantAuthMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Contstructor

        public TenantAuthMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        #endregion

        #region Public Methods

        /*
            IAuthenticationSchemeProvider has been moved to Invoke.
            Invoke method is called after we have registered tenant services
            it will have all the tenant specific authentication services available to it now
        */
        public async Task Invoke(HttpContext context, IAuthenticationSchemeProvider schemes)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });

            //give only IAuthenticationRequestHandler schemes a chance to handle the request
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await schemes.GetRequestHandlerSchemesAsync())
            {
                if (await handlers.GetHandlerAsync(context, scheme.Name) is IAuthenticationRequestHandler handler
                    && await handler.HandleRequestAsync())
                    return;
            }

            var defaultAuthenticate = await schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                    context.User = result.Principal;
            }

            await _next(context);
        }

        #endregion
    }
}
