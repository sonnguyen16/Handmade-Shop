using HandmadeShop.Data.Services;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HandmadeShop.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService productService;
        private readonly IProductImageService productImageService;

        public ProductController(IProductService productService, IProductImageService productImageService)
        {
            this.productService = productService;
            this.productImageService = productImageService;
        }
        public IActionResult Index(int id)
        {
            var pd = productService.GetByIdAsync(id).Result;
            ProductViewModel model = new ProductViewModel()
            {
                Product = pd,
                ProductImages = productImageService.GetAllAsync().Result,
                Products = productService.GetAllAsync().Result.Where(p => p.CategoryID == pd.CategoryID && p.ID != pd.ID)
            };
            setData();
            return View(model);
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
