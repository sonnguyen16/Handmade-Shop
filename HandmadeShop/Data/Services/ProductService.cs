using HandmadeShop.Data.Base;
using HandmadeShop.Models;

namespace HandmadeShop.Data.Services
{
    public class ProductService : EntityBaseRepository<Product>, IProductService
    {
        public ProductService(AppDbContext context) : base(context)
        {
            
        }
    }
}
