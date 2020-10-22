using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantTemplate.Core.Accessors;
using MultiTenantTemplate.Core.Accessors.IAccessors;
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

            //single instance available to all all tenants
            services.AddSingleton(new ApplicationSpecificTestService());
            services.AddSingleton<ITenantAccessor<Tenant>, TenantAccessor<Tenant>>();

            services.AddMultiTenancy()
                .WithResolutionStrategy<RouteResolutionStrategy>()
                .WithStore<InMemoryTenantStore>();
        }

        public static void ConfigureMultiTenantServices(Tenant t, ContainerBuilder c)
        {
            var tenantServices = new ServiceCollection();

            //TODO AddAuthenticationCore?
            var builder = tenantServices.AddAuthentication(o =>
            {
                o.DefaultScheme = $"{t.Id}--{IdentityConstants.ApplicationScheme}";
            }).AddCookie($"{t.Id}--{IdentityConstants.ApplicationScheme}", o =>
            {
                //o.LoginPath...
            });

            //builder.AddFacebook()...

            //there are instances that will be scoped to the current tenant, one instance per tenant
            c.RegisterInstance(new TenantSpecificTestService()).SingleInstance();

            c.RegisterTenantOptions<CookiePolicyOptions, Tenant>((options, tenant) =>
            {
                options.ConsentCookie.Name = $"{tenant.Id}-consent";
                options.CheckConsentNeeded = context => false;
            });

            c.Populate(tenantServices);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultiTenancy()
                .UseMultiTenantContainer()
                .UseMultiTenantAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{tenant}/{action}");
            });
        }
    }
}
