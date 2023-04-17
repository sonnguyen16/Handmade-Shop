namespace HandmadeShop.Models.ViewModel
{
    public class WishListViewModel
    {
        public IEnumerable<WishList> wishList { get; set; }
        public IEnumerable<ProductImage> productImages { get; set; }
        public IEnumerable<Product> products { get; set; }
    }
}
