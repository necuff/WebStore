using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

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
            services.AddControllersWithViews(opt=>
            { 
                //����� ����������� ������� � ���������� ��� MVC
                //opt.Filters.Add<>()
                //opt.Conventions.Add()
            })
                .AddRazorRuntimeCompilation();
        }
     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����� ������������ ��� ������������� ��
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    //�������� �� ����������� �������� � �����������. ���� �����������, �� ������ ������������ ��� ������ 500

                app.UseBrowserLink();
            }

            //��� ������������� �� �������� �� ������� ������������ �����������
            app.UseStaticFiles();
            app.UseDefaultFiles();

            //��� �������������
            app.UseRouting();

            //���������� ��������� �������� MVC
            app.UseWelcomePage("/MVC");

            //������� ���� ������������� ��
            app.Use(async (context, next) => 
            {
                Debug.WriteLine($"Request to {context.Request.Path}");
                await next(); //���� �� ������� next() �������� ���������
            });

            //app.UseMiddleware<>();

            //�������
            app.UseEndpoints(endpoints =>
            {                

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    /*
                     * id? - '?' - ������������ ��������
                     * action=Index - �������� �� ���������
                     */
                    );
            });
        }
    }
}
