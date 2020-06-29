using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Employees;
using WebStore.Domain.ViewMoodel;

namespace WebStore.Infrastructure.Mapping
{
    public static class EmployeeMapper
    {
        public static EmployeeViewModel ToView(this Employee e) => new EmployeeViewModel
        {
            Id = e.Id,
            Age = e.Age,
            FirstName = e.FirstName,
            Patronymic = e.Patronymic,
            Surname = e.Surname,
        };

        public static Employee FromView(this EmployeeViewModel e) => new Employee
        {
            Id = e.Id,
            Age = e.Age,
            FirstName = e.FirstName,
            Patronymic = e.Patronymic,
            Surname = e.Surname,
        };

        public static IEnumerable<EmployeeViewModel> ToView(this IEnumerable<Employee> e) => e.Select(ToView);
    }
}
