using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SystemProductOrder.models
{
    public class Review
    {
        [Key]
        [JsonIgnore]
        public int Rid { get; set; }

        public int UserId { get; set; }

        
      

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
