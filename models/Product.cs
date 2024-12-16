using System.ComponentModel.DataAnnotations;

namespace SystemProductOrder.models
{
    public class Product
    {
        [Key] 
        public int Pid { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name can't exceed 200 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative.")]
        public int Stock { get; set; }

        public decimal OverallRating { get; set; } // Calculated based on reviews

        // Navigation Properties
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<OrderPorduct> OrderProducts { get; set; }
    }
}
