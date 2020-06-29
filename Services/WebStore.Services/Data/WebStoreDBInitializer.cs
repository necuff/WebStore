using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public WebStoreDBInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            var db = _db.Database;

            /*
            if (db.EnsureDeleted()) удаление БД
                if (!db.EnsureCreated())    создание БД
                    throw new InvalidOperationException("Ошибка при создании базы данных товаров");
            */
            db.Migrate();   //аналог update-database


            InitializeEmployees();

            InitializeProducts();

            InitializeIdentityAsync().Wait();
        }

        private void InitializeEmployees()
        {
            var db = _db.Database;

            if (!_db.Employees.Any()) return;

            using (db.BeginTransaction())
            {
                var employees = TestData.Employees.ToList();
                /*
                foreach (var employee in employees)
                    employee.Id = 0;
                */

                employees.ForEach(e => e.Id = 0);

                _db.Employees.AddRange(employees);
            }


        }

        private void InitializeProducts()
        {
            var db = _db.Database;

            if (_db.Products.Any()) return;

            using (db.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                //Чтобы вручную править ключи в БД
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSection] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSection] OFF");

                db.CommitTransaction();
            }

            using (var tran = db.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrand] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrand] OFF");

                tran.Commit();
            }

            using (var tran = db.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                tran.Commit();
            }
        }


        private async Task InitializeIdentityAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Role.Administrator))
                await _roleManager.CreateAsync(new Role { Name = Role.Administrator });

            if (!await _roleManager.RoleExistsAsync(Role.User))
                await _roleManager.CreateAsync(new Role { Name = Role.User });

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User { UserName = User.Administrator };

                var create_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (create_result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = create_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при созании пользователя Администратор: {string.Join(",", errors)}");
                }
            }
        }
    }
}
