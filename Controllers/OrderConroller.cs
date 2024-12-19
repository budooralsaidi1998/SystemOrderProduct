using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Servieses;

namespace SystemProductOrder.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class OrderConroller:ControllerBase
    {
        private readonly  IOrderPrdouctServies _orderPrdouctServies;
        private readonly IOrderServices _orderServices;
        private readonly IConfiguration _configuration;
        public OrderConroller(IOrderServices orderServices, IConfiguration configuration, IOrderPrdouctServies orderprduct )
        {
            _orderServices= orderServices;
          _configuration = configuration;
            _orderPrdouctServies= orderprduct;

        }
        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder([FromBody] List<OrderProductInput> orderDetails)
        {
            try
            {
                // Assuming we get the UserId from the authenticated user
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _orderServices.PlaceOrder(userId, orderDetails);
                return Ok("Order placed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet("GetOrders")]
        public IActionResult GetOrdersForUser()
        {
            try
            {
                //Get the user's ID from the token
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null) return Unauthorized("User ID claim is missing.");

                int userId;
                if (!int.TryParse(userIdClaim.Value, out userId)) return BadRequest("Invalid user ID claim.");

                // Fetch orders for the user
                var orders = _orderServices.GetAllOrders(userId);

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching orders: {ex.Message}");
            }
        }

       
        [HttpGet("GetOrderDetails/{orderId}")]
        public IActionResult GetOrderDetails(int orderId)
        {
            try
            {
                
                var order = _orderPrdouctServies.GetOrderDetailsById(orderId);

               

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching order details: {ex.Message}");
            }
        }

    }
}
