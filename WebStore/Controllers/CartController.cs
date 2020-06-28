using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewMoodel;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService) => this.cartService = cartService;

        public IActionResult Details() => View(new CartOrderViewModel
        {
            Cart = cartService.TransformFromCart(),
            Order = new OrderViewModel()

        });

        public IActionResult AddToCart(int id)
        {
            cartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult DecrementFromCart(int id)
        {
            cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveFromCart(int id)
        {
            cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveAll()
        {
            cartService.RemoveAll();
            return RedirectToAction(nameof(Details));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderViewModel Model, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new CartOrderViewModel
                {
                    Cart = cartService.TransformFromCart(),
                    Order = Model
                });
            var order = await OrderService.CreateOrder(User.Identity.Name, cartService.TransformFromCart(), Model);
            cartService.RemoveAll();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id});
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
