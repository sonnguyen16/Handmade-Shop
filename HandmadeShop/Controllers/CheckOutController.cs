using HandmadeShop.Data.Services;
using HandmadeShop.Data.Static;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace HandmadeShop.Controllers
{
    public class CheckoutController : Controller
    {
        private IProductService _productRepository;
        private readonly IProductImageService productImageService;
        private readonly IOrderService orderService;
        private readonly IOrderItemService orderItemService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAddressService addressService;


        public CheckoutController(IAddressService addressService,UserManager<ApplicationUser> userManager,IOrderService orderService, IOrderItemService orderItemService,IProductService productRepository, IProductImageService productImageService)
        {
            _productRepository = productRepository;
            this.productImageService = productImageService;
            this.orderService = orderService;
            this.orderItemService = orderItemService;
            _userManager = userManager;
            this.addressService= addressService;
        }
        public IActionResult Index()
        {
            setData();
            var currentUser = HttpContext.User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = _userManager.GetUserId(this.User);
            IEnumerable<Address> addresses = addressService.GetAllAsync().Result.Where(a => a.AccountID == userId);



            return View(addresses);
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

        [HttpPost]
        public async Task<IActionResult> Confirm([FromBody]Order order)
        {
            Cart cart = GetCart();
           
            order.AccountID = _userManager.GetUserId(this.User);
            order.Status = OrderStatus.Pending;
            order.Time= DateTime.Now;
            order.OrderNumber = order.AccountID + order.Time;
            order.Total = (decimal)cart.ComputeTotalValue();

            await orderService.AddAsync(order);


            foreach(var item in cart.Items)
            {
                await orderItemService.AddAsync(new OrderItem()
                {
                    OrderNumber = order.OrderNumber,
                    ProductID = item.Product.ID,
                    Quantity = item.Quantity,
                });
            }

            
            cart.Clear();
            SaveCart(cart);
            return Json(order);
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
