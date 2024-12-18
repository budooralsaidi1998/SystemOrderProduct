using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Servieses;

namespace SystemProductOrder.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IOrderServices _orderService;

        public ReviewController(IReviewService reviewService, IOrderServices orderService)
        {
            _reviewService = reviewService;
            _orderService = orderService;
        }

        // 1. Add a review for a product

        [HttpPost("AddReview")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult AddReview([FromBody] ReviewInput review) // Now accepting Review object
        {
            try
            {
                // Call the service layer to handle the review addition
                _reviewService.AddReview(review);

                return Ok("Review added successfully.");
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Handle other errors like duplicate reviews
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle general errors
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        // 2. Get all reviews for a product with pagination
        [HttpGet("GetProductReviews")]
        public async Task<IActionResult> GetProductReviews(int productId, int page = 1, int pageSize = 10)
        {
            try
            {
                var reviews = _reviewService.GetReviewsForProduct(productId, page, pageSize);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // 3. Update a review (only by the user who created it)
        [HttpPut("UpdateReview/{reviewId}")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewInput reviewInput)
        {
            try
            {
                // Get the authenticated user's ID
                var userId = int.Parse(User.Identity.Name);

                // Fetch the existing review
                var existingReview = _reviewService.GetReviewById(reviewId);
                if (existingReview == null)
                {
                    return NotFound("Review not found.");
                }

                // Ensure the review belongs to the current user
                if (existingReview.UserId != userId)
                {
                    return Unauthorized("You can only update your own review.");
                }

                // Update the review using the service
                _reviewService.UpdateReview(userId, reviewId, reviewInput.Rating, reviewInput.Comment);

                return Ok("Review updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. Delete a review (only by the user who created it)
        [HttpDelete("DeleteReview")]
public async Task<IActionResult> DeleteReview(int reviewId)
{
    try
    {
        var userId = int.Parse(User.Identity.Name); // Get the authenticated user's ID

        var existingReview =  _reviewService.GetReviewById(reviewId);
        if (existingReview == null)
        {
            return NotFound("Review not found.");
        }

        // Check if the review belongs to the current user
        if (existingReview.UserId != userId)
        {
            return Unauthorized("You can only delete your own review.");
        }

        // Delete the review
         _reviewService.DeleteReview(userId, reviewId);

        // Recalculate the product's overall rating
        // _reviewService.RecalculateProductRating(existingReview.ProductId);

        return Ok("Review deleted successfully.");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
}
}
