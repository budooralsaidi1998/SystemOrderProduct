using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IReviewRepo
    {
        void AddReview(Review review);
        void DeleteReview(Review review);
        IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize);
        Review GetReview(int reviewId);
        double RecalculateProductRating(int productId);
        void UpdateReview(Review review);
        public Review GetReviewfordelete(int reviewid);
    }
}