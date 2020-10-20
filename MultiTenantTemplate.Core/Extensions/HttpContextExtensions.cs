using Microsoft.AspNetCore.Http;
using MultiTenantTemplate.Common;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Extensions
{
    /// <summary>
    /// Extensions to HttpContext to make multi-tenancy easier to use
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Return the current tenant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T GetTenant<T>(this HttpContext context)
            where T : Tenant
        {
            if (!context.Items.ContainsKey(Constants.HttpContextTenantKey))
                return null;

            return context.Items[Constants.HttpContextTenantKey] as T;
        }

        /// <summary>
        /// Return the current Tenant
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Tenant GetTenant(this HttpContext context)
        {
            return context.GetTenant<Tenant>();
        }
    }
}
