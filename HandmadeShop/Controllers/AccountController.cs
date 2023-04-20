using HandmadeShop.Data;
using HandmadeShop.Data.Services;
using HandmadeShop.Data.Static;
using HandmadeShop.Infrastructure;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandmadeShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private IProductService _productRepository;
        private readonly IProductImageService productImageService;
        private readonly IAddressService addressService;
        private readonly IOrderService orderService;
        private readonly IOrderItemService orderItemService;

        public AccountController(IOrderItemService orderItemService,IOrderService orderService,IAddressService addressService,IProductService productRepository, IProductImageService productImageService,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _productRepository = productRepository;
            this.productImageService = productImageService;
            this.addressService = addressService;
            this.orderService = orderService;
            this.orderItemService = orderItemService;
        }


        public async Task<IActionResult> Users(string userName)
        {
            var users =  _context.Users.ToListAsync();
            return View(users);
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

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        public IActionResult Login()
        {
            setData();
            return View();
        }

        public IActionResult Dashboard() {
            
            setData();
            var currentUser = HttpContext.User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login","Account");
            }

            string userId = _userManager.GetUserId(this.User);

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            IEnumerable<Order> orders = orderService.GetAllAsync().Result.Where(o => o.AccountID == userId);
            IEnumerable<OrderItem> orderItems = orderItemService.GetAllAsync().Result;
            IEnumerable<Address> addresses = addressService.GetAllAsync().Result.Where(a => a.AccountID == userId);

            UserViewModel userViewModel = new UserViewModel()
            {
                User = user,
                Orders = orders,
                Addresses = addresses,
                OrderItem = orderItems
            };

            return View(userViewModel);
        }

        public async Task<IActionResult> newAddress(Address address)
        {
            string userId = _userManager.GetUserId(this.User);

            address.AccountID = userId;

            await addressService.AddAsync(address);

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            IEnumerable<Order> orders = orderService.GetAllAsync().Result.Where(o => o.AccountID == userId);
            IEnumerable<OrderItem> orderItems = orderItemService.GetAllAsync().Result;
            IEnumerable<Address> addresses = addressService.GetAllAsync().Result.Where(a => a.AccountID == userId);

            UserViewModel userViewModel = new UserViewModel()
            {
                User = user,
                Orders = orders,
                Addresses = addresses,
                OrderItem = orderItems
            };
            setData();

            return View("Dashboard",userViewModel);
        }

        public async Task<IActionResult> removeAddress(int id)
        {
            string userId = _userManager.GetUserId(this.User);

            await addressService.DeleteAsync(id);

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            IEnumerable<Order> orders = orderService.GetAllAsync().Result.Where(o => o.AccountID == userId);
            IEnumerable<OrderItem> orderItems = orderItemService.GetAllAsync().Result;
            IEnumerable<Address> addresses = addressService.GetAllAsync().Result.Where(a => a.AccountID == userId);

            UserViewModel userViewModel = new UserViewModel()
            {
                User = user,
                Orders = orders,
                Addresses = addresses,
                OrderItem = orderItems
            };
            setData();

            return View("Dashboard", userViewModel);
        }

        public IActionResult viewCart(int id)
        {
            setData();
            string userId = _userManager.GetUserId(this.User);

            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            IEnumerable<Order> orders = orderService.GetAllAsync().Result.Where(o => o.AccountID == userId && o.ID == id);
            IEnumerable<OrderItem> orderItems = orderItemService.GetAllAsync().Result.Where(oi => oi.OrderNumber == orders.FirstOrDefault().OrderNumber);
            IEnumerable<Address> addresses = addressService.GetAllAsync().Result.Where(a => a.AccountID == userId);
            UserViewModel userViewModel = new UserViewModel()
            {
                User = user,
                Orders = orders,
                Addresses = addresses,
                OrderItem = orderItems
            };
            return View("/Views/Account/Cart.cshtml", userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            setData();
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                      
                        return RedirectToAction("Index","Home");
                    }
                }
                TempData["Error"] = "Thông tin đăng nhập không đúng";
                return View(loginVM);
            }

            TempData["Error"] = "Thông tin đăng nhập không đúng";
            return View(loginVM);
        }


        public IActionResult Register()
        {
            setData();
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            setData();
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "Email này đã được sử dụng";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                await _signInManager.PasswordSignInAsync(newUser, registerVM.Password, false, false);
            }

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

        public RedirectToActionResult Clear()
        {
            Cart cart = GetCart();
            cart.Clear();
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

    }
}
