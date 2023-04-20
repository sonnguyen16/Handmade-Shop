using Microsoft.AspNetCore.Mvc;
using HandmadeShop.Models;
using HandmadeShop.Data.Services;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models.ViewModel;

namespace ProductManagement.Controllers
{
    public class CartController : Controller
    {
       private IProductService _productRepository;
        private readonly IProductImageService productImageService;

        public CartController(IProductService productRepository, IProductImageService productImageService)
        {
            _productRepository = productRepository;
            this.productImageService = productImageService;
        }

        public IActionResult Index()
        {
            setData();
            return View();
        }
     
        public IActionResult AddToCart(int id, int quantity)
        {
          
            Product product = _productRepository.GetAllAsync().Result.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, quantity);
                SaveCart(cart);
            }
            setData();
            return PartialView("/Views/Home/Cart.cshtml");
        }

        public IActionResult AddToCart2(int id)
        {
           
            Product product = _productRepository.GetAllAsync().Result.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            setData();
            return PartialView("/Views/Cart/Cart.cshtml");
        }

        public IActionResult RemoveFromCart2(int id)
        {
          
            Product product = _productRepository.GetAllAsync().Result.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, -1);
                SaveCart(cart);
            }
            setData();
            return PartialView("/Views/Cart/Cart.cshtml");
        }

        public IActionResult RemoveFromCart(int id)
        {
          
            Product product = _productRepository.GetAllAsync().Result.FirstOrDefault(p => p.ID == id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveItem(product);
                SaveCart(cart);
            }
            setData();
            return PartialView("/Views/Cart/Cart.cshtml");
        }

        public void setData()
        {
            var allProducts = _productRepository.GetAllAsync().Result;
            var allProductImage = productImageService.GetAllAsync().Result;

            ListViewModel listViewModel = new ListViewModel()
            {
                Products = allProducts,
                ProductImages = allProductImage,
                Cart = GetCart()
            };
            ViewBag.ListViewModel = listViewModel;
        }

        public RedirectToActionResult Clear()
        {
            Cart cart = GetCart();
            cart.Clear();
            SaveCart(cart);
            return RedirectToAction("Index");
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
