using System.Security.Claims;
using SystemProductOrder.DTO;
using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IProductServies
    {
        void AddProduct(ProductInput input, ClaimsPrincipal user);
        Product GetDetailsProductByID(int id);
        PagedResult<Product> GetProducts(string name, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 10);
        void UpdateProduct(int id);
    }
}