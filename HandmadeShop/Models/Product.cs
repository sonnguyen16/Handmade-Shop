using HandmadeShop.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class Product : IEntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Seller { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category category { get; set; }
        public string Material { get; set; }
        public string Status { get; set; }
    }
}
