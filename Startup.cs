using BikeLostAndFound.Data;
using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Models;
using BikeLostAndFound.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeLostAndFound
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
            services.AddControllersWithViews();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDbContextPool<MyDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("BLFDB")));
            services.AddScoped<IBikeLostAndFoundRepository, BikeLostAndFoundRepositoryBase>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();
            
            services.AddAuthentication();
            services.AddAuthorization();
            services.ConfigureApplicationCookie(config => { config.LoginPath = "/Account/Login"; });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimPrincipalFactory>();
            services.AddSingleton<DataProtectionPurposeString>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvc();
        }
    }
}
