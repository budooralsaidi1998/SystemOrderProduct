using SystemProductOrder.DTO;
using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IReviewService
    {
        void AddReview(ReviewInput review);
        void DeleteReview(int userId, int reviewId);
        Review GetReviewById(int reviewId);
        Task<Review> GetReviewByUserAndProduct(int userId, int productId);
        List<Review> GetReviewsForProduct(int productId, int page, int pageSize);
        void UpdateReview(int userId, int reviewId, int rating, string comment);
    }
}