using static SystemProductOrder.Repositry.ReviewRepo;
using SystemProductOrder.models;
using Microsoft.EntityFrameworkCore;
using SystemProductOrder.DTO;

namespace SystemProductOrder.Repositry
{


    public class ReviewRepo : IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddReview(Review review)
        {
            _context.reviews.Add(review);
            _context.SaveChanges();
        }
        public Review GetReview(int productid)
        {
            return _context.reviews.FirstOrDefault(r => r.ProductId == productid);
        }
        public Review GetReviewfordelete(int reviewid)
        {
            return _context.reviews.FirstOrDefault(r => r.Rid == reviewid);
        }
        public IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize)
        {
            return _context.reviews
                .Where(r => r.ProductId == productId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public void UpdateReview(Review review)
        {
            _context.reviews.Update(review);
            _context.SaveChanges();
        }
        public void DeleteReview(Review review)
        {
            _context.reviews.Remove(review);
            _context.SaveChanges();
        }
        public double RecalculateProductRating(int productId)
        {
            var reviews = _context.reviews.Where(r => r.ProductId == productId).ToList();
            if (reviews.Any())
            {
                var overallRating = (double)reviews.Average(r => (double)r.Rating); // Cast to decimal
                var product = _context.products.FirstOrDefault(p => p.Pid == productId);
                if (product != null)
                {
                    product.OverallRating = overallRating; // Use the calculated rating
                    _context.products.Update(product);
                    _context.SaveChanges();
                }
                return overallRating;
            }
            return 0;
        }
    }
    //public void AddReview(Review review)
    //{
    //    _context.reviews.Add(review);
    //    _context.SaveChanges();
    //}

    //public void UpdateReview(Review review)
    //{
    //    _context.reviews.Update(review);
    //    _context.SaveChanges();
    //}

    //    public void DeleteReview(int reviewId)
    //    {
    //        var review = _context.reviews.Find(reviewId);
    //        if (review != null)
    //        {
    //            _context.reviews.Remove(review);
    //            _context.SaveChanges();
    //        }
    //    }

    //    //public Review GetReview(int userId, int productId)
    //    //{
    //    //    return _context.reviews.FirstOrDefault(r => r.UserId == userId && r.ProductId == productId);
    //    //}

    //    //public List<Review> GetReviewsForProduct(int productId, int page, int pageSize)
    //    //{
    //    //    return _context.reviews
    //    //        .Where(r => r.ProductId == productId)
    //    //        .OrderByDescending(r => r.ReviewDate)
    //    //        .Skip((page - 1) * pageSize)
    //    //        .Take(pageSize)
    //    //        .ToList();
    //    //}

    //    public double CalculateOverallRating(int productId)
    //    {
    //        var ratings = _context.reviews.Where(r => r.ProductId == productId).Select(r => r.Rating);
    //        return ratings.Any() ? ratings.Average() : 0.0;
    //    }
    //    //public async Task<Review> GetReviewByUserAndProduct(int userId, int productId)
    //    //{
    //    //    return await _context.reviews
    //    //        .Where(r => r.UserId == userId && r.ProductId == productId)
    //    //        .FirstOrDefaultAsync();
    //    //}
    //    //public Review GetReviewById(int reviewId)
    //    //{
    //    //    return _context.reviews
    //    //        .FirstOrDefault(r => r.Rid == reviewId);  // Assuming "Id" is the primary key of the review
    //    //}

    //    public Review GetReviewById(int reviewId)
    //    {
    //        return _context.reviews.FirstOrDefault(r => r.Rid == reviewId);
    //    }

    //    // Check if user has ordered the product
    //    public bool UserHasOrderedProduct(int userId, int productId)
    //    {
    //        return _context.orders
    //                         .Any(o => o.UserId == userId && o.OrderProducts.Any(op => op.ProductId == productId));
    //    }

    //    // Check if a review already exists for the product by the user
    //    public bool HasUserReviewedProduct(int userId, int productId)
    //    {
    //        return _context.reviews
    //                         .Any(r => r.UserId == userId && r.ProductId == productId);
    //    }



    //    public void UpdateReview(Review review)
    //    {
    //        _context.reviews.Update(review);
    //        _context.SaveChanges();
    //    }

    //    // Add a review to the database
    //    public void AddReview(Review review)
    //    {
    //        _context.reviews.Add(review);
    //        _context.SaveChanges();
    //    }

    //    // Get all reviews for a product to calculate the average rating
    //    public List<Review> GetReviewsByProductId(int productId)
    //    {
    //        return _context.reviews
    //                      .Where(r => r.ProductId == productId)
    //                      .Include(r => r.User) // Assuming a navigation property to the User table
    //                      .OrderByDescending(r => r.ReviewDate) // Most recent reviews first
    //                      .ToList();
    //    }

    //    // Update the overall rating of the product
    //    public void UpdateProductRating(int productId, double averageRating)
    //    {
    //        var product = _context.products.FirstOrDefault(p => p.Pid == productId);
    //        if (product != null)
    //        {
    //            product.OverallRating = averageRating;
    //            _context.SaveChanges();
    //        }
    //    }
    //}

}

