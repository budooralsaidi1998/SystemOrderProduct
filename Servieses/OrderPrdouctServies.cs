using SystemProductOrder.models;
using SystemProductOrder.Repositry;

namespace SystemProductOrder.Servieses
{

    public class OrderPrdouctServies : IOrderPrdouctServies
    {
        private readonly IOrderProductRepo _orderRepo;

        public OrderPrdouctServies(IOrderProductRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }


        public List<OrderPorduct> GetOrderDetailsById(int id)
        {
            // Fetch order details from the repository
            var orderProducts = _orderRepo.GetOrderDetailsById(id);

            // Check if the order ID exists and has associated products
            if (orderProducts == null || !orderProducts.Any())
            {
                throw new Exception($"Order with ID {id} not found or has no associated products.");
            }
            return _orderRepo.GetOrderDetailsById(id);
        }

    }
}
