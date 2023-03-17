using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class Review
    {
        public int ID { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
        public string Reviewer { get; set; }

        [ForeignKey("Reviewer")]
        public ApplicationUser User { get; set; }

    }
}
