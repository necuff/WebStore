using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Employees;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;

        public SqlEmployeesData(WebStoreDB db) => _db = db; 

        public int Add(Employee Employee)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(Employee Employee)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> Get()
        {
            throw new NotImplementedException();
        }

        public Employee GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
