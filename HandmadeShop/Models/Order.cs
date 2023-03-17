using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string AccountID { get; set; }

        [ForeignKey("AccountID")]
        public ApplicationUser User { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }

    }
}
