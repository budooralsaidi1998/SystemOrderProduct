using static SystemProductOrder.Repositry.ReviewRepo;
using SystemProductOrder.models;
using Microsoft.EntityFrameworkCore;

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

        public void UpdateReview(Review review)
        {
            _context.reviews.Update(review);
            _context.SaveChanges();
        }

        public void DeleteReview(int reviewId)
        {
            var review = _context.reviews.Find(reviewId);
            if (review != null)
            {
                _context.reviews.Remove(review);
                _context.SaveChanges();
            }
        }

        public Review GetReview(int userId, int productId)
        {
            return _context.reviews.FirstOrDefault(r => r.UserId == userId && r.ProductId == productId);
        }

        public List<Review> GetReviewsForProduct(int productId, int page, int pageSize)
        {
            return _context.reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.ReviewDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public double CalculateOverallRating(int productId)
        {
            var ratings = _context.reviews.Where(r => r.ProductId == productId).Select(r => r.Rating);
            return ratings.Any() ? ratings.Average() : 0.0;
        }
        public async Task<Review> GetReviewByUserAndProduct(int userId, int productId)
        {
            return await _context.reviews
                .Where(r => r.UserId == userId && r.ProductId == productId)
                .FirstOrDefaultAsync();
        }
        public Review GetReviewById(int reviewId)
        {
            return _context.reviews
                .FirstOrDefault(r => r.Rid == reviewId);  // Assuming "Id" is the primary key of the review
        }
    }

}

