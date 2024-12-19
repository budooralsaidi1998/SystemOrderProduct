using SystemProductOrder.DTO;
using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IReviewService
    {
        // void AddReview(int userid,ReviewInput review);
        void AddReview(int userId, int productId, string comment, int rating);
        //void DeleteReview(int userId, int reviewId);
        //Review GetReviewById(int reviewId);
        //Task<Review> GetReviewByUserAndProduct(int userId, int productId);
        //List<Review> GetReviewsForProduct(int productId, int page, int pageSize);
        //void UpdateReview(int userId, int reviewId, int rating, string comment);
    }
}