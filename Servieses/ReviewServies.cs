using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Repositry;

namespace SystemProductOrder.Servieses
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;
        private readonly IUserRepo _userRepo;
        public ReviewService(IReviewRepo reviewRepo, IOrderRepo orderRepo, IProductRepo productRepo, IUserRepo userRepo)
        {
            _reviewRepo = reviewRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public void AddReview(ReviewInput review)
        {
            // Ensure rating is valid
            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            // Ensure user has purchased the product
            var orders = _orderRepo.GetOrdersByUserId(review.UserId);
            var hasPurchased = orders.Any(order => order.OrderProducts.Any(op => op.ProductId == review.ProductId));

            if (!hasPurchased)
                throw new InvalidOperationException("User must purchase the product before reviewing.");

            // Ensure user has not already reviewed the product
            var existingReview = _reviewRepo.GetReviewByUserAndProduct(review.UserId, review.ProductId);
            if (existingReview != null)
                throw new InvalidOperationException("User has already reviewed this product.");

            // Add the review
            //review.ReviewDate = DateTime.UtcNow;
            // Set ReviewDate
            var newre = new Review
            {
                UserId = review.UserId,
                ProductId = review.ProductId,
                Comment = review.Comment,
                Rating = review.Rating,
                //ReviewDate = review.ReviewDate,
            };
            _reviewRepo.AddReview(newre);

            // Recalculate overall rating
            var overallRating = _reviewRepo.CalculateOverallRating(review.ProductId);
            var product = _productRepo.GetProductsByID(review.ProductId);
            product.OverallRating = overallRating;
            _productRepo.UpdateProduct(product);
        }

        public void UpdateReview(int userId, int reviewId, int rating, string comment)
        {
            // Fetch the review
            var review = _reviewRepo.GetReview(userId, reviewId);
            if (review == null || review.UserId != userId)
                throw new InvalidOperationException("User does not have permission to update this review.");

            // Update review attributes
            review.Rating = rating;
            review.Comment = comment;
            review.ReviewDate = DateTime.UtcNow;

            _reviewRepo.UpdateReview(review);

            // Recalculate overall rating
            var overallRating = _reviewRepo.CalculateOverallRating(review.ProductId);
            var product = _productRepo.GetProductsByID(review.ProductId);
            product.OverallRating = overallRating;
            _productRepo.UpdateProduct(product);
        }

        public void DeleteReview(int userId, int reviewId)
        {
            // Fetch the review
            var review = _reviewRepo.GetReview(userId, reviewId);
            if (review == null || review.UserId != userId)
                throw new InvalidOperationException("User does not have permission to delete this review.");

            _reviewRepo.DeleteReview(reviewId);

            // Recalculate overall rating
            var overallRating = _reviewRepo.CalculateOverallRating(review.ProductId);
            var product = _productRepo.GetProductsByID(review.ProductId);
            product.OverallRating = overallRating;
            _productRepo.UpdateProduct(product);
        }

        public List<Review> GetReviewsForProduct(int productId, int page, int pageSize)
        {
            return _reviewRepo.GetReviewsForProduct(productId, page, pageSize);
        }
        public async Task<Review> GetReviewByUserAndProduct(int userId, int productId)
        {
            try
            {
                return await _reviewRepo.GetReviewByUserAndProduct(userId, productId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving review for user and product.", ex);
            }
        }
        public Review GetReviewById(int reviewId)
        {
            var review = _reviewRepo.GetReviewById(reviewId);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }
            return review;
        }
    }

}
