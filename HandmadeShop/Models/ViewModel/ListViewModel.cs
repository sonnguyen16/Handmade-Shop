using HandmadeShop.Models;

namespace HandmadeShop.Models.ViewModel
{
    public class ListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }

    }
}
