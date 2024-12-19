using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IOrderProductRepo
    {
        List<OrderPorduct> GetOrderDetailsById(int id);
    }
}