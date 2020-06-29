using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Domain.Entities.Employees;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewMoodel;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;

namespace WebStore.Controllers
{
    //[Route("{controller}")]   //Так выглядит маршрут по умолчанию
    //Ниже вариант ручного прописывания маршрутов
    //[Route("Staff")]
    [Authorize]
    public class EmployeesController : Controller
    {
        //private static readonly List<Employee> __Employees = TestData.Employees;

        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        //[Route("List")]
        public IActionResult Index()
        {
            return View(_EmployeesData.Get());
        }

        //[Route("{id}")]
        //[Authorize]
        public IActionResult EmployeeDetails(int id)
        {
            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        #region Редактирование
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int? id, [FromServices] IMapper mapper)
        {
            if (id is null) return View(new EmployeeViewModel());
            if (id < 0)
                return BadRequest();

            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
                return NotFound();


            return View(mapper.Map<EmployeeViewModel>(employee));
            //return View(employee.ToView());
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeeViewModel model, [FromServices] IMapper mapper)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            
            //Проверка мадели на валидность
            if (!ModelState.IsValid)
                return View(model);

            //Ручная проверка значений на валидность
            if (model.Age < 18 || model.Age > 75)
                ModelState.AddModelError("Age", "Сотрудник не проходит по возрасту");
            if (model.FirstName == "123" && model.Surname == "QWE")
                ModelState.AddModelError(string.Empty, "Странное сочетание имени и фамилии");


            var id = model.Id;

            var employee = mapper.Map<Employee>(model);
            //var employee = model.FromView();

            if (id == 0)
            {
                _EmployeesData.Add(employee);
            } else
            {
                _EmployeesData.Edit(employee);
            }

            _EmployeesData.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Удаление
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(employee.ToView());
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            _EmployeesData.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
    }
}