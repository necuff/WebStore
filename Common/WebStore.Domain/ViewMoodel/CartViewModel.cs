using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewMoodel
{
    public class CartViewModel
    {
        //public Dictionary<ProductViewModel, int> Items { get; set; } = new Dictionary<ProductViewModel, int>();

        public IEnumerable<(ProductViewModel Product, int Quantity)> Items { get; set; }

        public int ItemsCount => Items?.Sum(item => item.Quantity) ?? 0;
    }
}
