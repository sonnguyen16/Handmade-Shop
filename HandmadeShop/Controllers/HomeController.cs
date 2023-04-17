using HandmadeShop.Data.Services;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HandmadeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductImageService productImageService;
        private readonly IWishListService wishListService;
        UserManager<ApplicationUser> userManager;

        public HomeController(IProductService productService, UserManager<ApplicationUser> userManager, IProductImageService productImageService, IWishListService wishListService)
        {
            this.productService = productService;
            this.productImageService = productImageService;
            this.wishListService = wishListService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var allProducts =  productService.GetAllAsync().Result;
            var allProductImage =  productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            return View(listViewModel);
        }



        public IActionResult QuickView(int id)
        {
            var product = productService.GetByIdAsync(id).Result;
            var productImage = productImageService.GetAllAsync().Result.FirstOrDefault(pm => pm.ProductID == id);

            return Json(new {
               product,
                productImage
            });
        }

        public IActionResult AboutUs() {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            return View(listViewModel);
        }

        public IActionResult ContactUs()
        {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            return View(listViewModel);
        }

        public IActionResult FAQ()
        {
            var allProducts = productService.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            return View(listViewModel);
        }

        public IActionResult Livestream()
        {
            return View();
        }


        public IActionResult Clear(string returnUrl)
        {
            Cart cart = GetCart();
            cart.Clear();
            SaveCart(cart);
            return Json("Xóa thành công");
        }


        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }


    }
}