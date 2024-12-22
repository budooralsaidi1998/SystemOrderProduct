using System.Security.Claims;
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
        //public void AddReview(Review review, int userId)
        //{
        //    // Check if the product exists
        //    var product = _productRepo.GetProductsByID(review.ProductId);
        //    if (product == null)
        //    {
        //        throw new InvalidOperationException($"Product with ID {review.ProductId} does not exist.");
        //    }

        //    // Check if the user has already reviewed this product
        //    var existingReview = _reviewRepo.HasUserReviewedProduct(userId, review.ProductId);
        //    if (existingReview)
        //    {
        //        throw new InvalidOperationException("You have already reviewed this product.");
        //    }


        //    // Check if the user has purchased the product
        //    var hasPurchased = _reviewRepo.UserHasOrderedProduct(userId, review.ProductId);
        //    if (!hasPurchased)
        //    {
        //        throw new InvalidOperationException("You can only review products you have purchased.");
        //    }

        //    // Add the review
        //    _reviewRepo.AddReview(review);

        //    // Calculate the average rating for the product
        //    var averageRating = _reviewRepo.CalculateOverallRating(review.ProductId);

        //    // Update the product's overall rating
        //    product.OverallRating = averageRating;
        //    _productRepo.UpdateProduct(product);
        //}


        //public PagedResult<ReviewInput> GetAllReviewsForProduct(int productId, int page, int pageSize, ClaimsPrincipal user)
        //{
        //    // Fetch reviews from the repository
        //    var reviews = _reviewRepo.GetReviewsByProductId(productId);

        //    // Fetch product details
        //    var product = _productRepo.GetProductsByID(productId);
        //    if (product == null)
        //    {
        //        throw new Exception($"Product with ID {productId} not found.");
        //    }

        //    // Paginate results
        //    var pagedReviews = reviews.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    // Extract the user's name from authentication
        //    var userName = user.Identity?.Name ?? "Unknown";

        //    // Map to DTO
        //    var result = new PagedResult<ReviewInput>
        //    {
        //        reviews = pagedReviews.Select(r => new ReviewInput
        //        {
        //            //UserName = userName,
        //            //ProductName = product.Name,
        //             ProductId = productId,
        //            Rating = r.Rating,
        //            Comment = r.Comment,
        //            DateCreated = r.ReviewDate
        //        }).ToList(),
        //        numberpage = page,
        //        PageSize = pageSize,
        //        TotalItems = reviews.Count
        //    };

        //    return result;
        //}


        //public void UpdateReview(ReviewInput reviewInput, int userId,int idreview)
        //{
        //    // Retrieve the existing review
        //    var existingReview = _reviewRepo.GetReviewById(idreview);
        //    if (existingReview == null)
        //    {
        //        throw new InvalidOperationException($"Review with ID {reviewInput.rid} not found.");
        //    }

        //    // Ensure the review belongs to the authenticated user
        //    if (existingReview.UserId != userId)
        //    {
        //        throw new UnauthorizedAccessException("You can only update your own reviews.");
        //    }

        //    // Ensure the user has purchased the product
        //    var hasPurchased = _reviewRepo.UserHasOrderedProduct(userId, reviewInput.ProductId);
        //    if (!hasPurchased)
        //    {
        //        throw new InvalidOperationException("You cannot update a review for a product you haven't purchased.");
        //    }

        //    // Update the review fields
        //    existingReview.Rating = reviewInput.Rating;
        //    existingReview.Comment = reviewInput.Comment;
        //    existingReview.ReviewDate = reviewInput.DateCreated;

        //    // Save the updated review to the database
        //    _reviewRepo.UpdateReview(existingReview);

        //    // Recalculate the product's overall rating
        //    var averageRating = _reviewRepo.CalculateOverallRating(reviewInput.ProductId);
        //    var product = _productRepo.GetProductsByID(reviewInput.ProductId);
        //    if (product != null)
        //    {
        //        product.OverallRating = averageRating;
        //        _productRepo.UpdateProduct(product);
        //    }
        //}
        //public void DeleteReview(int reviewId, int userId, string role)
        //{
        //    // Fetch the review
        //    var review = _reviewRepo.GetReviewById(reviewId);
        //    if (review == null)
        //    {
        //        throw new InvalidOperationException($"Review with ID {reviewId} not found.");
        //    }

        //    // Validate permissions
        //    if (role != "Admin" && review.UserId != userId)
        //    {
        //        throw new UnauthorizedAccessException("You do not have permission to delete this review.");
        //    }

        //    // Delete the review
        //    _reviewRepo.DeleteReview(review.Rid);

        //    // Recalculate the product's overall rating
        //    var averageRating = _reviewRepo.CalculateOverallRating(review.ProductId);
        //    var product = _productRepo.GetProductsByID(review.ProductId);
        //    if (product != null)
        //    {
        //        product.OverallRating = averageRating;
        //        _productRepo.UpdateProduct(product);
        //    }
        //}
        public void AddReview(Review review, int userId)
        {
            var orders = _orderRepo.GetOrdersByUserId(userId);
            if (!orders.Any(o => o.OrderProducts.Any(op => op.ProductId == review.ProductId)))
            {
                throw new InvalidOperationException("User cannot review a product they have not purchased.");
            }
            var existingReview = _reviewRepo.GetAllReviewsForProduct(review.ProductId, 1, int.MaxValue)
                .FirstOrDefault(r => r.UserId == userId);
            if (existingReview != null)
            {
                throw new InvalidOperationException("User cannot review the same product more than once.");
            }
            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new InvalidOperationException("Rating must be between 1 and 5.");
            }
            _reviewRepo.AddReview(review);
            _reviewRepo.RecalculateProductRating(review.ProductId);
        }
        public IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize)
        {
            return _reviewRepo.GetAllReviewsForProduct(productId, page, pageSize);
        }
        //public void UpdateReview(Review review, int userId)
        //{
        //    var existingReview = _reviewRepo.GetReview(review.Rid);
        //    if (existingReview == null)
        //    {
        //        throw new InvalidOperationException("User can only update their own reviews.");
        //    }
        //    if (review.Rating < 1 || review.Rating > 5)
        //    {
        //        throw new InvalidOperationException("Rating must be between 1 and 5.");
        //    }
        //    existingReview.Rating = review.Rating;
        //    existingReview.Comment = review.Comment;
        //    _reviewRepo.UpdateReview(existingReview);
        //    _reviewRepo.RecalculateProductRating(review.ProductId);
        //}
        public void UpdateReview(Review review, int userId)
        {
            // Fetch the existing review
            var existingReview = _reviewRepo.GetReview(review.ProductId);

            // Check if the review exists
            if (existingReview == null)
            {
                throw new InvalidOperationException("The review does not exist.");
            }

            // Check if the review belongs to the current user
            if (existingReview.UserId != userId)
            {
                throw new InvalidOperationException("User can only update their own reviews.");
            }

            // Check if the review is for the specified product
            if (existingReview.ProductId != review.ProductId)
            {
                throw new InvalidOperationException("The review does not match the specified product.");
            }

            // Validate the rating value
            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new InvalidOperationException("Rating must be between 1 and 5.");
            }

            // Update the review details
            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;

            // Save the updated review
            _reviewRepo.UpdateReview(existingReview);

            // Recalculate the product's overall rating
            _reviewRepo.RecalculateProductRating(review.ProductId);
        }
        public void DeleteReview(int reviewid, int userId, string role)
        { 
        //    if (role != "NormalUser")
        //    {
        //        throw new InvalidOperationException("Only users can delete their own reviews.");
        //    }
            var review = _reviewRepo.GetReviewfordelete(reviewid);
            if (review == null || review.UserId != userId)
            {
                throw new InvalidOperationException("User can only delete their own reviews.");
            }
            _reviewRepo.DeleteReview(review);
            _reviewRepo.RecalculateProductRating(review.ProductId);
        }
    }
}

    
