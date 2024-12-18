using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IProductRepo
    {
        void AddProduct(Product product);
        IQueryable<Product> GetAllProducts();
        Product GetProductsByID(int id);
        int GetTotalProductCount();
        void UpdateProduct(Product product);
        Product GetNameProduct(string name);


    }
}