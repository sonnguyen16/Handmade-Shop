using HandmadeShop.Data.Services;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HandmadeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductImageService productImageService;

        public HomeController(IProductService productService, IProductImageService productImageService)
        {
            this.productService = productService;
            this.productImageService = productImageService;
        }

        public IActionResult Index()
        {
            var allProducts =  productService.GetAllAsync().Result;
            var allProductImage =  productImageService.GetAllAsync().Result;
            
            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage
            };
            return View(listViewModel);
        }

        public IActionResult AboutUs() {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }


    }
}