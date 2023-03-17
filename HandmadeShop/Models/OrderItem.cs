using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
