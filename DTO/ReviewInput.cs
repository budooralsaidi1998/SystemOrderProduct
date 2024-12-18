namespace SystemProductOrder.DTO
{
    public class ReviewInput
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Rating { get; set; }  // Rating should be between 1 and 5
        public string Comment { get; set; }
    }
}
