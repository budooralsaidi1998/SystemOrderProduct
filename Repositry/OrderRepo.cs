using Microsoft.EntityFrameworkCore;
using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Adds a new order to the database
        public void AddOrder(Order order)
        {
            // Add the order entity to the database
            _context.orders.Add(order);

            // Save the changes to persist the order
            _context.SaveChanges();
        }

        // Adds a new order-product relationship to the database
        public void AddOrderProduct(OrderPorduct orderProduct)
        {
            // Add the order-product entity to the database
            _context.orderPorducts.Add(orderProduct);

            // Save the changes to persist the order-product entry
            _context.SaveChanges();
        }
        // Get all orders for a specific user
        public List<Order> GetOrdersByUserId(int userId)
        {
            return _context.orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderProducts) // Include order-product relationships
                .ThenInclude(op => op.Product) // Include product details
                .ToList();
        }

        // Get order details by ID
        public Order GetOrderDetailsById(int orderId)
        {
            return _context.orders
                .Where(o => o.Oid == orderId)
                .Include(o => o.OrderProducts) // Include order-product relationships
                .ThenInclude(op => op.Product) // Include product details
                .FirstOrDefault();
        }
        public async Task<List<Order>> GetOrdersByUserIdList(int userId)
        {
            return await _context.orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderProducts)  // Ensure that OrderProducts are included in the result
                .ToListAsync();
        }
        public Order GetOrderByUserId(int userId)
        {
            // Retrieve the first existing order for the given user
            return _context.orders.FirstOrDefault(o => o.UserId == userId);
        }
        public OrderPorduct GetOrderProduct(int orderId, int productId)
        {
            return _context.orderPorducts
                           .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
        }
        public void UpdateOrderProduct(OrderPorduct orderProduct)
        {
            // Mark the entity as modified
            _context.orderPorducts.Update(orderProduct);

            // Save changes to the database
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            // Mark the entity as modified
            _context.orders.Update(order);

            // Save changes to the database
            _context.SaveChanges();
        }
        public List<string> GetProductNamesByOrderId(int orderId)
        {
            return _context.orderPorducts
                             .Where(op => op.OrderId == orderId)   // Filter by OrderId
                             .Include(op => op.Product)           // Load related Product data
                             .Select(op => op.Product.Name)       // Select Product Name
                             .ToList();                           // Convert to List and return
        }


    }
}

