using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Models;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Estore
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                x=>
                {
                    x.LoginPath = "/Home/Index";
                }
                );
            services.AddSession();
            services.AddDbContext<PRN211_DB_ASMContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Conn"))
            );
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(Configuration);
            services.AddSingleton(mapper);

            services.AddScoped<ICategoryRepository>(x =>
                new CategoryRepository(x.GetRequiredService<PRN211_DB_ASMContext>()));
            services.AddScoped<IMemberRepository>(x =>
                new MemberRepository(x.GetRequiredService<PRN211_DB_ASMContext>()));
            services.AddScoped<IProductRepository>(x =>
                new ProductRepository(x.GetRequiredService<PRN211_DB_ASMContext>()));
            services.AddScoped<IOrderDetailRepository>(x =>
                new OrderDetailRepository(x.GetRequiredService<PRN211_DB_ASMContext>()));
            services.AddScoped<IOrderRepository>(x =>
                new OrderRepository(x.GetRequiredService<PRN211_DB_ASMContext>()));
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
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
