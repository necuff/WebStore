using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;

        public WebStoreDBInitializer(WebStoreDB db) { _db = db; }

        public void Initialize()
        {
            var db = _db.Database;

            /*
            if (db.EnsureDeleted()) удаление БД
                if (!db.EnsureCreated())    создание БД
                    throw new InvalidOperationException("Ошибка при создании базы данных товаров");
            */
            db.Migrate();   //аналог update-database


            if (!_db.Employees.Any())
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
    }
}
