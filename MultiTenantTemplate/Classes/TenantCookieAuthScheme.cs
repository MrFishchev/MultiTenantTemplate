using System;
using Microsoft.AspNetCore.Identity;

namespace MultiTenantTemplate.Classes
{
    internal static class TenantCookieAuthScheme
    {
        public static string GetScheme(Guid tenantId)
        {
            return $"{tenantId}--{IdentityConstants.ApplicationScheme}";
        }
    }
}
