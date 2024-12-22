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

            // Check if the user already has an open order
            var existingOrder = _orderRepo.GetOrderByUserId(userId);

            // If no order exists, create a new one
            if (existingOrder == null)
            {
                existingOrder = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0 ,// Will be updated later
                    ProductNames = existingOrder.ProductNames
                };
                _orderRepo.AddOrder(existingOrder);
            }

            // Loop through order details to add/update OrderProduct and calculate total
            foreach (var item in orderDetails)
            {
                // Fetch product details
                var product = _productRepo.GetProductsByID(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                // Check stock availability
                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for product {product.Name}. Available stock: {product.Stock}.");
                }

                // Check if the product is already in the order
                var existingOrderProduct = _orderRepo.GetOrderProduct(existingOrder.Oid, item.ProductId);
                if (existingOrderProduct != null)
                {
                    // Update quantity if product already exists in the order
                    existingOrderProduct.Quantity += item.Quantity;
                    _orderRepo.UpdateOrderProduct(existingOrderProduct);
                }
                else
                {
                    // Add new OrderProduct entry
                    var orderProduct = new OrderPorduct
                    {
                        OrderId = existingOrder.Oid,
                        ProductId = item.ProductId,
                        ProductName = product.Name, // Add the product name
                        Quantity = item.Quantity
                    };
                    _orderRepo.AddOrderProduct(orderProduct);
                }

                // Deduct stock for the product
                product.Stock -= item.Quantity;
                _productRepo.UpdateProduct(product);

                // Calculate the total amount for this product
                totalAmount += product.Price * item.Quantity;
            }

            // Update the total amount for the order
            existingOrder.TotalAmount += totalAmount;
            _orderRepo.UpdateOrder(existingOrder);
        }

        //public List<Order> GetAllOrders(int id, ClaimsPrincipal user)
        //{

        //    return _orderRepo.GetOrdersByUserId(id);
        //}
        public List<Order> GetAllOrders(int userId)
        {
            // Get orders for the user
            var orders = _orderRepo.GetOrdersByUserId(userId);

            // Map orders to DTOs and fetch product names for each order
            return orders.Select(order => new Order
            {
                Oid = order.Oid,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                ProductNames = _orderRepo.GetProductNamesByOrderId(order.Oid) // Use repository to get product names
            }).ToList();


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
