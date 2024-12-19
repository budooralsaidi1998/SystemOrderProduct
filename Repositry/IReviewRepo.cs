using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IReviewRepo
    {
        void AddReview(Review review);
        List<Review> GetReviewsByProductId(int productId);
        bool HasUserReviewedProduct(int userId, int productId);
        void UpdateProductRating(int productId, double averageRating);
        bool UserHasOrderedProduct(int userId, int productId);
    }
}