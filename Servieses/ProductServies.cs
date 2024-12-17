using Microsoft.EntityFrameworkCore;
using SystemProductOrder.DTO;
using SystemProductOrder.Migrations;
using SystemProductOrder.models;
using SystemProductOrder.Repositry;

namespace SystemProductOrder.Servieses
{
    public class ProductServies : IProductServies
    {
        private readonly IProductRepo _productrepo;


        public ProductServies(IProductRepo productrepo)
        {

            _productrepo = productrepo;

        }

        public void AddProduct(ProductInput input)
        {
            var newprduct = new Product
            {
                Name = input.Name,
                Price = input.Price,
                Stock = input.Stock,
                Description = input.Description

            };

            _productrepo.AddProduct(newprduct);
        }

        public PagedResult<Product> GetProducts(string name, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 10)
        {
            // Base query from the repository
            var query = _productrepo.GetAllProducts();

            // Apply filters in the service layer
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Count total items after filtering
            var totalItems = query.Count();

            // Apply pagination
            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paginated result
            return new PagedResult<Product>
            {
                Items = products,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public void UpdateProduct(int id)
        {
            var existingProduct = _productrepo.GetProductsByID(id);

            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            // Update all attributes
            var newupdate = new Product
            {
                Name = existingProduct.Name,
                Price = existingProduct.Price,
                Stock = existingProduct.Stock,
                Description = existingProduct.Description,

            };
            _productrepo.UpdateProduct(newupdate);

        }

        public Product GetDetailsProductByID(int id)
        {
            return _productrepo.GetProductsByID(id);
        }
    }
}
