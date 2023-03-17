using HandmadeShop.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class ProductImage:IEntityBase
    {
        public int ID { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public string ImagePath { get; set; }
    }
}
