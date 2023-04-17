using HandmadeShop.Data.Services;
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
            return View(model);
        }
    }
}
