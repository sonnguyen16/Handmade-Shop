using HandmadeShop.Data.Services;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

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
            var allProducts = productService.GetAllAsync().Result.Where(p => categoryId != 0 ? p.CategoryID == categoryId : true);
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            return View(listViewModel);
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
    }
}
