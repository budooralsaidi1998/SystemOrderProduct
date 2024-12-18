using System.Security.Claims;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Repositry;

namespace SystemProductOrder.Servieses
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;
        private readonly IUserRepo _userRepo;
        public OrderServices(IOrderRepo orderRepo, IProductRepo productRepo, IUserRepo userRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public void PlaceOrder(int userId, List<OrderProductInput> orderDetails)
        {
            decimal totalAmount = 0;

            // Check stock and calculate the total amount
            foreach (var item in orderDetails)
            {
                var product = _productRepo.GetProductsByID(item.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for product {product.Name}. Available stock: {product.Stock}.");
                }

                // Calculate total price for the current product
                totalAmount += product.Price * item.Quantity;
            }

            // Create the Order
            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount
            };

            _orderRepo.AddOrder(newOrder); // Save the Order
            int orderId = newOrder.Oid; // Get the generated Order ID

            // Create entries in the OrderProduct table and update stock
            foreach (var item in orderDetails)
            {
                var product = _productRepo.GetProductsByID(item.ProductId);

                // Add OrderProduct entry
                var orderProduct = new OrderPorduct
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                _orderRepo.AddOrderProduct(orderProduct);

                // Deduct stock
                product.Stock -= item.Quantity;
                _productRepo.UpdateProduct(product);
            }

        }
        public List<Order> GetAllOrders(int id,ClaimsPrincipal user)
        {

            //Creates a new Product object from the input data transfer object(DTO).
            var isAdmin = user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only admin users can Show the order  products.");
            }
            return _orderRepo.GetOrdersByUserId(id);
        }

        public Order GetOrderById(int id, ClaimsPrincipal user)
        {

            //Creates a new Product object from the input data transfer object(DTO).
            var isAdmin = user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only admin users can Show the order  products.");
            }
            return _orderRepo.GetOrderDetailsById(id);
        }
        public async Task<bool> HasUserPurchasedProduct(int userId, int productId)
        {
            try
            {
                // Ensure that the user exists
                var user = await _userRepo.GetUserById(userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found.");
                }

                // Check if the user has any order that includes the specified product
                var orders = await _orderRepo.GetOrdersByUserIdList(userId);

                // Check if any order contains the product
                foreach (var order in orders)
                {
                    if (order.OrderProducts.Any(op => op.ProductId == productId))
                    {
                        return true; // The user has purchased the product
                    }
                }

                return false; // The user hasn't purchased the product
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error checking if user has purchased product.", ex);
            }
        }
    }
}
