using System.Text.Json.Serialization;

namespace SystemProductOrder.DTO
{
    public class ReviewInput
    {
        [JsonIgnore]
        public int rid {  get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int ProductId { get; set; }
       // public string ProductName { get; set; }
        public int Rating { get; set; }  // Rating should be between 1 and 5
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
       // public string UserName { get; set; }
    }
}
