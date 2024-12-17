using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IReviewRepo
    {
        void AddReview(Review review);
        double CalculateOverallRating(int productId);
        void DeleteReview(int reviewId);
        Review GetReview(int userId, int productId);
        Review GetReviewById(int reviewId);
        Task<Review> GetReviewByUserAndProduct(int userId, int productId);
        List<Review> GetReviewsForProduct(int productId, int page, int pageSize);
        void UpdateReview(Review review);
    }
}