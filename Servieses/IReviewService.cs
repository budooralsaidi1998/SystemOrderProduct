using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IReviewService
    {
        void AddReview(Review review, int userId);
        void DeleteReview(int reviewId, int userId, string role);
        IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize);
        void UpdateReview(Review review, int userId);
    }
}