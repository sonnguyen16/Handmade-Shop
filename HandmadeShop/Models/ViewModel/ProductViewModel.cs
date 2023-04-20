namespace HandmadeShop.Models.ViewModel
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public ListViewModel ListViewModel { get; set; }
    }
}
