﻿using Microsoft.AspNetCore.Builder;
using MultiTenantTemplate.Core.Classes;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Extensions
{
    /// <summary>
    /// For middleware registration
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use the tenant middleware to process the request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenancy<T>(this IApplicationBuilder builder)
            where T : Tenant => builder.UseMiddleware<TenantMiddleware<T>>();


        /// <summary>
        /// Use the tenant to process the request with default Tenant
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder builder)
            => builder.UseMiddleware<TenantMiddleware<Tenant>>();

        /// <summary>
        /// Use custom multi-tenant container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenantContainer<T>(this IApplicationBuilder builder)
            where T : Tenant => builder.UseMiddleware<MultiTenantContainerMiddleware<T>>();

        /// <summary>
        /// Use custom multi-tenant container with default Tenant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenantContainer(this IApplicationBuilder builder)
            => builder.UseMiddleware<MultiTenantContainerMiddleware<Tenant>>();

        /// <summary>
        /// Use the Tenant Auth to process the authentication handlers
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenantAuthentication(this IApplicationBuilder builder)
            => builder.UseMiddleware<TenantAuthMiddleware>();
    }
}