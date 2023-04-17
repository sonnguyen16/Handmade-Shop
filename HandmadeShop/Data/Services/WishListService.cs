using HandmadeShop.Data.Base;
using HandmadeShop.Models;

namespace HandmadeShop.Data.Services
{
    public class WishListService:EntityBaseRepository<WishList>, IWishListService
    {
        public WishListService(AppDbContext context) : base(context)
        {
        }
    }
}
