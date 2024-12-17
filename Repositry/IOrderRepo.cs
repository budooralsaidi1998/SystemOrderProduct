using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IOrderRepo
    {
        void AddOrder(Order order);
        void AddOrderProduct(OrderPorduct orderProduct);
        Order GetOrderDetailsById(int orderId);
        List<Order> GetOrdersByUserId(int userId);
        Task<List<Order>> GetOrdersByUserIdList(int userId);
    }
}