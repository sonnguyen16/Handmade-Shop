using HandmadeShop.Data.Base;
using HandmadeShop.Models;

namespace HandmadeShop.Data.Services
{
    public class OrderItemService : EntityBaseRepository<OrderItem>, IOrderItemService
    {
        public OrderItemService(AppDbContext context) : base(context)
        {
        }
    }
}
