using HandmadeShop.Data.Base;
using HandmadeShop.Models;

namespace HandmadeShop.Data.Services
{
    public class ProductImageService : EntityBaseRepository<ProductImage>, IProductImageService
    {
        public ProductImageService(AppDbContext context) : base(context)
        {
        }
    }
}
