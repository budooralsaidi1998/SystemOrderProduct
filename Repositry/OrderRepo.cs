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
    }
}

