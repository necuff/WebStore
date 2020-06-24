using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.ViewMoodel;

namespace WebStore.Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product p) => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.ImageUrl,
            Order = p.Order,
            Price = p.Price,
        };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> p) => p.Select(ToView);
    }
}
