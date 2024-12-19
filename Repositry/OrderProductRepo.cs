using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public class OrderProductRepo : IOrderProductRepo
    {
        private readonly ApplicationDbContext _context;


        // Constructor to inject ApplicationDbContext
        public OrderProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<OrderPorduct> GetOrderDetailsById(int id)
        {
            return _context.orderPorducts.Where(us => us.OrderId == id).ToList();
        }
    }
}
