using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IOrderPrdouctServies
    {
        List<OrderPorduct> GetOrderDetailsById(int id);
    }
}