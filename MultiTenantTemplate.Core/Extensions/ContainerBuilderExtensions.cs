using System;
using Autofac;
using Microsoft.Extensions.Options;
using MultiTenantTemplate.Core.Options;
using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterTenantOptions<TOptions, T>(this ContainerBuilder builder, 
            Action<TOptions, T> tenantConfig)
            where TOptions : class, new() where T : Tenant
        {
            builder.RegisterType<TenantOptionsCache<TOptions, T>>()
                .As<IOptionsMonitorCache<TOptions>>()
                .SingleInstance();

            builder.RegisterType<TenantOptionsFactory<TOptions, T>>()
                .As<IOptionsFactory<TOptions>>()
                .WithParameter(new TypedParameter(typeof(Action<TOptions, T>), tenantConfig))
                .SingleInstance();

            builder.RegisterType<TenantOptions<TOptions>>()
                .As<IOptionsSnapshot<TOptions>>()
                .SingleInstance();

            builder.RegisterType<TenantOptions<TOptions>>()
                .As<IOptions<TOptions>>()
                .SingleInstance();

            return builder;
        }
    }
}
