﻿using HandmadeShop.Data.Services;
using HandmadeShop.Models;
using HandmadeShop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HandmadeShop.Controllers
{
    public class WishListController : Controller
    {

        private readonly IProductService productService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductImageService productImageService;
        private readonly IWishListService wishListService;
        WishListViewModel wishListViewModel;

        public WishListController(IProductService productService, IProductImageService productImageService, IWishListService wishListService, UserManager<ApplicationUser> userManager)
        {
            this.productService = productService;
            this.productImageService = productImageService;
            this.wishListService = wishListService;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = _userManager.GetUserId(this.User);

            IEnumerable<ProductImage> productImages = productImageService.GetAllAsync().Result;
            IEnumerable<WishList> wishList = wishListService.GetAllAsync().Result.Where(w => w.AccountID == userId);


           wishListViewModel = new WishListViewModel()
            {
                wishList = wishList,
                productImages = productImages,
                products = productService.GetAllAsync().Result,
            };


            return View(wishListViewModel);
        }

        public async Task<IActionResult> AddToWishList(int id)
        {
            string userId = _userManager.GetUserId(this.User);
           
            WishList wishList = new WishList()
            {
                AccountID = userId,
                ProductID = id
            };
            if (wishListService.GetAllAsync().Result.FirstOrDefault(w => w.ProductID == id && w.AccountID == userId) != null)
            {
                return Json(0);
            }
            await wishListService.AddAsync(wishList);

            return Json(1);
        }


        public async Task<IActionResult> Remove(int id)
        {
            await wishListService.DeleteAsync(id);

            var currentUser = HttpContext.User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = _userManager.GetUserId(this.User);

            IEnumerable<ProductImage> productImages = await productImageService.GetAllAsync();
            IEnumerable<WishList> wishList = (await wishListService.GetAllAsync()).Where(w => w.AccountID == userId);

            wishListViewModel = new WishListViewModel()
            {
                wishList = wishList,
                productImages = productImages,
                products = await productService.GetAllAsync(),
            };

            return View("Index", wishListViewModel);
        }

    }
}
