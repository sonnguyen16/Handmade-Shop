namespace HandmadeShop.Models.ViewModel
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set;}
        public IEnumerable<Order> Orders { get; set;}
        public IEnumerable<OrderItem> OrderItem { get; set; }
        public IEnumerable<Address> Addresses { get; set;}
    }
}
