using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Food_Delivery.Helpers;
using Food_Delivery.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Food_Delivery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
        Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        var requestPath = ctx.Request.Path;
                        if (requestPath.StartsWithSegments("/Admin"))
                        {
                            ctx.Response.Redirect("/Admin/Dashboard/Login");
                        }
                        else if (requestPath.StartsWithSegments("/Employee"))
                        {
                            ctx.Response.Redirect("/Employee/Dashboard/Login");
                        }
                        else {
                            ctx.Response.Redirect("/Identity/Account/Login");
                        }
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
                        var requestPath = ctx.Request.Path;
                        if (requestPath.StartsWithSegments("/Admin"))
                        {
                            ctx.Response.Redirect("/Admin/Dashboard/Login");
                        }
                        else if (requestPath.StartsWithSegments("/Employee"))
                        {
                            ctx.Response.Redirect("/Employee/Dashboard/Login");
                        }
                        else {
                            ctx.Response.Redirect("/Identity/Account/Login");
                        }
                        return Task.CompletedTask;
                    }
                };
                // Cookie settings
                options.Cookie.HttpOnly = true;

                options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });

            //services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "DocumentFiles")),
                //RequestPath = "/Documents"
            });

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "employeesCorner",
                    areaName:"Employee",
                    pattern: "Employee/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "SystemAdminsCorner",
                    areaName: "SystemAdmin",
                    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            RolesData.SeedRoles(serviceProvider).Wait();
        }
    }
}
