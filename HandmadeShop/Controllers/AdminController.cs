using HandmadeShop.Data;
using HandmadeShop.Data.Services;
using HandmadeShop.Data.Static;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HandmadeShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IProductService _productRepository;
        private readonly IProductImageService productImageService;
        private readonly IAddressService addressService;
        private readonly IOrderService orderService;
        private readonly IOrderItemService orderItemService;
        public AppDbContext dbContext;
        private IWebHostEnvironment environment { get; set; }
        public AdminController(AppDbContext dbContext,IWebHostEnvironment environment,IOrderItemService orderItemService, IOrderService orderService, IAddressService addressService, IProductService productRepository, IProductImageService productImageService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _productRepository = productRepository;
            this.productImageService = productImageService;
            this.addressService = addressService;
            this.orderService = orderService;
            this.orderItemService = orderItemService;
            this.environment = environment;
            this.dbContext = dbContext;
        }

        public IActionResult Dashboard()
        {
            var currentUser = HttpContext.User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var userId = _userManager.GetUserId(this.User);
            ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
            ViewBag.user = user;
			return View(); 
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Json(true);
                    }
                }
                return Json(false);
            }

            return Json(false);
        }

        public IActionResult GetProduct()
        {
            List<Product> products = (List<Product>)_productRepository.GetAllAsync().Result;
            var result = new
            {
                draw = 1,
                recordsTotal = products.Count,
                recordsFiltered = products.Count,
                data = products
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult Product()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addProduct([FromBody] Product product)
        {
            product.Status = ProductStatus.Approved;

            try
            {
                await _productRepository.AddAsync(product);
                var addedProduct = await dbContext.Products.OrderByDescending(p => p.ID).FirstOrDefaultAsync(); // Lấy sản phẩm với ID mới nhất
                int addedProductId = addedProduct.ID;
                ids = addedProductId;
                return Json(new { 
                    status = true,
                    data= ids
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    status = false,
                    data = ""
                });
            }
             
        }

        int ids;

        [HttpPost]
        public async Task<IActionResult> Upload(int id,List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = file.FileName;
                    string path = Path.Combine(environment.WebRootPath, "Images");
                    string filePath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        await productImageService.AddAsync(new ProductImage
                        {
                            ProductID = id,
                            ImagePath = fileName
                        });
                    }
                }
            }
            return Json(id);

        }


        [HttpPost]

        public async Task<IActionResult> updateProduct(int id,[FromBody] Product product)
        {
            product.Status = ProductStatus.Approved;

            try
            {
                await _productRepository.UpdateAsync(id,product);
                return Json(new
                {
                    status = true,
                    data = id
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    data = ""
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> removeProduct(int id)
        {
            try
            {
                await _productRepository.DeleteAsync(id);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> getaProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return Json(product);
        }

    }
}
