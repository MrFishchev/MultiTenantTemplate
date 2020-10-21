using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantTemplate.Accessors;
using MultiTenantTemplate.Accessors.IAccessors;
using MultiTenantTemplate.Core.Extensions;
using MultiTenantTemplate.Core.Stores;
using MultiTenantTemplate.Core.Strategies;
using MultiTenantTemplate.Model.Core;
using MultiTenantTemplate.Services;

namespace MultiTenantTemplate
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMultiTenancy()
                .WithResolutionStrategy<HostResolutionStrategy>()
                .WithStore<InMemoryTenantStore>();

            //single instance available to all all tenants
            services.AddSingleton(new ApplicationSpecificTestService());
            services.AddSingleton<ITenantAccessor<Tenant>, TenantAccessor<Tenant>>();
        }

        public static void ConfigureMultiTenantServices(Tenant t, ContainerBuilder c)
        {
            //there are instances that will be scoped to the current tenant, one instance per tenant
            c.RegisterInstance(new TenantSpecificTestService()).SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultiTenancy()
                .UseMultiTenantContainer();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
