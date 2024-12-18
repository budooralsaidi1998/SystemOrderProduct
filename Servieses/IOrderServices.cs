using System.Security.Claims;
using SystemProductOrder.DTO;
using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IOrderServices
    {
        List<Order> GetAllOrders(int id, ClaimsPrincipal user);
        Order GetOrderById(int id, ClaimsPrincipal user);
        Task<bool> HasUserPurchasedProduct(int userId, int productId);
        void PlaceOrder(int userId, List<OrderProductInput> orderDetails);
    }
}