using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewMoodel;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;        

        public CatalogController(IProductData ProductData)
        {
            _ProductData = ProductData;            
        }

        public IActionResult Shop(int? SectionId, int? BrandId, [FromServices]IMapper mapper)
        {
            var filter = new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId
            };

            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = SectionId,
                BrandId = BrandId,
                //Products = products.Select(p=>p.ToView()).OrderBy(p => p.Order)
                Products = products
                    .Select(mapper.Map<ProductViewModel>)
                    //.Select(p=> mapper.Map<ProductViewModel>(p))
                    //.ToView()
                    .OrderBy(p=>p.Order)
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }
    }
}
