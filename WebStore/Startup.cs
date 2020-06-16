using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services.InMemory;
using WebStore.Infrastructure.Services.InSQL;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt=>opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<WebStoreDBInitializer>();

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

            //Это маршрутизация
            app.UseRouting();

            //показывает рекламную страницу MVC
            app.UseWelcomePage("/MVC");

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
