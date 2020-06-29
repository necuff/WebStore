using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    public class WebApiTestController : Controller
    {
        private readonly IValueService valueService;

        public WebApiTestController(IValueService ValueService) => valueService = ValueService;

        public IActionResult Index()
        {
            var values = valueService.Get();
            return View(values);
        }
    }
}
