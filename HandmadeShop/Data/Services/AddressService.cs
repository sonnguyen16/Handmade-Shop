using HandmadeShop.Data.Base;
using HandmadeShop.Models;

namespace HandmadeShop.Data.Services
{
    public class AddressService : EntityBaseRepository<Address>, IAddressService
    {
        public AddressService(AppDbContext context) : base(context)
        {
        }
    }
}
