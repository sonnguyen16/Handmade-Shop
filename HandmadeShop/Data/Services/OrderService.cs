using HandmadeShop.Data.Base;
using HandmadeShop.Models;

namespace HandmadeShop.Data.Services
{
    public class OrderService : EntityBaseRepository<Order>, IOrderService
    {
        public OrderService(AppDbContext context) : base(context)
        {
        }
    }
}
