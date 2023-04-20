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
            setData();
            return View();
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
            setData();
            return View();
        }

        public IActionResult ContactUs()
        {
            setData();
            return View();
        }

        public IActionResult FAQ()
        {
            setData();
            return View();
        }

        public IActionResult Livestream()
        {
			var currentUser = HttpContext.User.Identity.Name;
			if (currentUser == null)
			{
				return RedirectToAction("Login", "Account");
			}
			setData();
			var userId = userManager.GetUserId(this.User);
			ApplicationUser user = userManager.FindByIdAsync(userId).Result;
			ViewBag.user = user;
			return View();
		}

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

    }
}