using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;
using WebStore.Models;
using WebStore.ViewMoodel;

namespace WebStore.Infrastructure.Services.InCookies
{
    public class CookiesCartService : ICartService
    {
        private readonly IProductData _productData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName;

        public Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[_cartName];

                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaceCookie(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set
            {
                ReplaceCookie(_httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
            }
        }

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cookie);
        }

        public CookiesCartService(IProductData productData, IHttpContextAccessor httpContextAccessor)
        {
            this._productData = productData;
            this._httpContextAccessor = httpContextAccessor;

            var user = _httpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _cartName = $"WebStore.Cart[{user_name}]";
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 0 });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveAll()
        {
            Cart cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;
            cart.Items.Remove(item);
            
            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            }) ;

            var product_view_models = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items.Select(item=>(product_view_models[item.ProductId], item.Quantity))
            }
        }
    }
}
