using System.ComponentModel.DataAnnotations.Schema;

namespace HandmadeShop.Models
{
    public class Address
    {
        public int ID { get; set; }
        public string AccountID { get; set; }

        [ForeignKey("AccountID")]
        public ApplicationUser User { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
    }
}
