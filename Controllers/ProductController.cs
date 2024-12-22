using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Servieses;

namespace SystemProductOrder.Controllers
{
    // This controller handles operations related to products.
    // It includes endpoints for adding, updating, and fetching products.
    [Authorize] // Ensures that all actions in this controller require authentication unless overridden.
    [ApiController]
    [Route("api/[Controller]")] // Defines the base route for this controller (e.g., api/Product).
    public class ProductController : ControllerBase
    {
        private readonly IProductServies _ProductService;
        private readonly IConfiguration _configuration;
        private readonly IUserServies userServies;
        private readonly IOrderPrdouctServies _orderPrdouctServies;

        // Constructor for injecting dependencies
        public ProductController(IProductServies ProductService, IConfiguration configuration , IUserServies _userServies , IOrderPrdouctServies oderpdouctserviese)
        {
            _ProductService = ProductService; // Injected service for handling product-related logic.
            _configuration = configuration;   // Injected configuration for application settings.
             userServies = _userServies;
             _orderPrdouctServies = oderpdouctserviese;
        }


        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For successful addition
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // For forbidden access
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For validation errors
        public IActionResult AddProduct(ProductInput product)
        {
            try
            {
                // Check if the user is authorized
                //var isAdmin = User.Claims.Any(c => c.Type == "Role" && c.Value == "Admin");
                //if (!isAdmin)
                //{
                //    return Forbid("You do not have permission to add products.");
                //}
                //var userId = int.Parse(User.Identity.Name); // Get User ID from claims
                //var user = userServies.GetUserForAccess(userId);

                //if (user.Roles = 1) // Check if role is "Admin" (1)
                //{
                //    return Forbid("You do not have permission to add products.");
                //}
                // Call the service to add the product
                _ProductService.AddProduct(product,User );

                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        // Updates an existing product in the system.
        // Accessible only by users with the "Admin" role.
        [HttpPut("UpdateProduct")] // Specifies the route as api/Product/UpdateProduct.
        
        public IActionResult UpdateProduct(int id,ProductInput input)
        {
            try
            {
                // Delegates the task of updating the product to the service layer.
                _ProductService.UpdateProduct(id,input ,User);
            }
            catch (Exception ex)
            {
                // Returns a 400 Bad Request with the error message in case of an exception.
                return BadRequest(ex.Message);
            }

            // Returns a 200 OK response upon successful update of the product.
            return Ok("Product updated successfully.");
        }

        // Fetches a paginated list of products with optional filtering by name and price range.
        [HttpGet("GetPagedProducts")] // Specifies the route as api/Product/GetPagedProducts.
        public IActionResult GetPagedProducts(string name = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 10)
        {
            try
            {
                // Delegates the task of fetching paginated products to the service layer.
                var result = _ProductService.GetProducts(name, minPrice, maxPrice, page, pageSize);

                // Returns the paginated result.
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Returns a 400 Bad Request with the error message in case of an exception.
                return BadRequest(ex.Message);
            }
        }

        // Fetches the details of a single product by its ID.
        [HttpGet("GetProductByid")] // Specifies the route as api/Product/GetProductByid.
        public IActionResult GetProductByid(int id)
        {
            try
            {
                // Delegates the task of fetching the product by ID to the service layer.
                var result = _ProductService.GetDetailsProductByID(id);

                // Returns the product details.
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Returns a 400 Bad Request with the error message in case of an exception.
                return BadRequest(ex.Message);
            }
        }
    }



}

