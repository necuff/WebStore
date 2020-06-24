using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services.InCookies;
using WebStore.Infrastructure.Services.InSQL;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt => 
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<WebStoreDBInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;

                //opt.User.AllowedUserNameCharacters = "abcdefghigklmnopqrstuvwxyzABDC1234567890";
                opt.User.RequireUniqueEmail = false;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
#endif
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GeekBrains.ru";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddControllersWithViews(opt=>
            { 
                //Здесь добавляются фильтры и соглашения для MVC
                //opt.Filters.Add<>()
                //opt.Conventions.Add()
            })
                .AddRazorRuntimeCompilation();

            services.AddScoped<IEmployeesData, SqlEmployeesData>();
            //services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();     //Объект создается на все время существования приложения
            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();   //Каждый раз при вызове создается новый объект
            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();      //Один объект на одну область действия

            //services.AddSingleton<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, CookiesCartService>();

        }
     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDBInitializer db)
        {
            db.Initialize();

            //Здесь подключается все промежуточное ПО
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    //Отвечает за отображение страницы с эксепшенами. Если отсутствует, то просто возвращается код ошибки 500

                app.UseBrowserLink();
            }

            //это промежуточное ПО отвечает за возврат статического содержимого
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseWelcomePage("/MVC");

            //Это маршрутизация
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
                        

            //Вызвать свое промежуточное ПО
            
            //app.Use(async (context, next) => 
            //{
            //    Debug.WriteLine($"Request to {context.Request.Path}");
            //    await next(); //Если не вызвать next() конвейер прервется
            //}); 

            //app.UseMiddleware<>();

            //Маппинг
            app.UseEndpoints(endpoints =>
            {                

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    /*
                     * id? - '?' - опчиональный параметр
                     * action=Index - значение по умолчанию
                     */
                    );
            });
        }
    }
}
