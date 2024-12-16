using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SystemProductOrder.models
{
    public class Order
    {
        [Key]

        public int Oid { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "TotalAmount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
        public decimal TotalAmount { get; set; } // This can be calculated from OrderProducts.

        // Navigation Properties
        public virtual ICollection<OrderPorduct> OrderProducts { get; set; }
    }
}

