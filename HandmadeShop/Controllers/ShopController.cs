using HandmadeShop.Data.Services;
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

        public IActionResult Index()
        {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage
            };
            return View(listViewModel);
        }
    }
}
