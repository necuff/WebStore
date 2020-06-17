﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Domain.Entities.Employees;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewMoodel;

namespace WebStore.Controllers
{
    //[Route("{controller}")]   //Так выглядит маршрут по умолчанию
    //Ниже вариант ручного прописывания маршрутов
    //[Route("Staff")]
    //[Authorize]
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
        public IActionResult Edit(int? id)
        {
            if (id is null) return View(new EmployeeViewModel());
            if (id < 0)
                return BadRequest();

            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
                return NotFound();



            return View(new EmployeeViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            });
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
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
            
            var employee = new Employee
            {
                Id = model.Id,
                FirstName = model.FirstName,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Age = model.Age
            };

            if(id == 0)
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
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            _EmployeesData.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
    }
}