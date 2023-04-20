using HandmadeShop.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class OrderItem: IEntityBase
    {
        public int ID { get; set; }
        public string OrderNumber { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
