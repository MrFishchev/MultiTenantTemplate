﻿using Microsoft.AspNetCore.Http;
using MultiTenantTemplate.Accessors.IAccessors;
using MultiTenantTemplate.Core.Extensions;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Accessors
{
    internal class TenantAccessor<T> : ITenantAccessor<T>
        where T : Tenant
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Properties

        public T Tenant => _httpContextAccessor.HttpContext.GetTenant<T>();

        #endregion

        #region Constructor

        public TenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion
    }
}
