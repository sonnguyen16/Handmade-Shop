using HandmadeShop.Data.Services;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HandmadeShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductImageService productImageService;

        public ShopController(IProductService productService, IProductImageService productImageService)
        {
            this.productService = productService;
            this.productImageService = productImageService;
        }

        public IActionResult Index(int categoryId)
        {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts.Where(p => categoryId != 0 ? p.CategoryID == categoryId : true),
                ProductImages = allProductImage,
                Cart = GetCart()
            };

            setData();
            return View(listViewModel);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };

            

            if (!string.IsNullOrEmpty(searchString))
            {
                listViewModel.Products = listViewModel.Products.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                //var filteredResultNew = products.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

            }
            setData();

            return View("Index", listViewModel);
        }

        public void setData()
        {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            ViewBag.ListViewModel = listViewModel;
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
    }
}
