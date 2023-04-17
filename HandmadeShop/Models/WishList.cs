using HandmadeShop.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class WishList: IEntityBase
    {
        public int ID { get; set; }
        public string AccountID { get; set; }

        [ForeignKey("AccountID")]
        public ApplicationUser User { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
