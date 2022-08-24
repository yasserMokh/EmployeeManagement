using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("EmployeeDbConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyStore.DeleteRole, policy => policy.RequireAssertion(context =>
                    (context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == ClaimStore.DeleteRole)) || context.User.IsInRole("Super Admin")
                ));
                options.AddPolicy(PolicyStore.EditRole, policy => policy.RequireAssertion(context =>
                    (context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == ClaimStore.EditRole)) || context.User.IsInRole("Super Admin")
                ));
                options.AddPolicy(PolicyStore.CreateRole, policy => policy.RequireAssertion(context =>
                    (context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == ClaimStore.CreateRole)) || context.User.IsInRole("Super Admin")
                ));
            });

            //services.AddMockRepositories();
            services.AddRepositories();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

            }

            //app.Run(async (context) =>
            //{
            //    //await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //    await context.Response.WriteAsync(_configuration["MyKey"]);
            //});

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});


            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=employee}/{action=index}/{id?}");
            });


        }
    }
}

