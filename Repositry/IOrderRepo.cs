using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IOrderRepo
    {
        void AddOrder(Order order);
        void AddOrderProduct(OrderPorduct orderProduct);
        Order GetOrderByUserId(int userId);
        Order GetOrderDetailsById(int orderId);
        OrderPorduct GetOrderProduct(int orderId, int productId);
        List<Order> GetOrdersByUserId(int userId);
        Task<List<Order>> GetOrdersByUserIdList(int userId);
        void UpdateOrderProduct(OrderPorduct orderProduct);
        void UpdateOrder(Order order);
        List<string> GetProductNamesByOrderId(int orderId);
        List<OrderPorduct> GetOrderProductsByOrderId(int orderId);
    }
}