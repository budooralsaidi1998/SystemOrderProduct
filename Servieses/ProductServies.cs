using Microsoft.EntityFrameworkCore;
using SystemProductOrder.DTO;
using SystemProductOrder.Migrations;
using SystemProductOrder.models;
using SystemProductOrder.Repositry;

namespace SystemProductOrder.Servieses
{
    // This service layer handles business logic for product-related operations.
    // It acts as an intermediary between the repository layer and the controller.
    public class ProductServies : IProductServies
    {
        private readonly IProductRepo _productrepo;

        // Constructor for injecting the repository dependency
        public ProductServies(IProductRepo productrepo)
        {
            _productrepo = productrepo; // Injected repository to handle data access logic.
        }

        // Adds a new product to the system.
        public void AddProduct(ProductInput input)
        {
            // Creates a new Product object from the input data transfer object (DTO).
            var newProduct = new Product
            {
                Name = input.Name,
                Price = input.Price,
                Stock = input.Stock,
                Description = input.Description
            };

            // Delegates the task of adding the product to the repository layer.
            _productrepo.AddProduct(newProduct);
        }

        // Retrieves a paginated and filtered list of products.
        public PagedResult<Product> GetProducts(string name, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 10)
        {
            // Gets the base query from the repository layer.
            var query = _productrepo.GetAllProducts();

            // Applies filtering by name, if provided.
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            // Applies filtering by minimum price, if provided.
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            // Applies filtering by maximum price, if provided.
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Counts the total number of items after applying filters.
            var totalItems = query.Count();

            // Applies pagination logic to the query.
            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Returns a paginated result with the filtered and paginated products.
            return new PagedResult<Product>
            {
                Items = products,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        // Updates an existing product in the system.
        public void UpdateProduct(int id)
        {
            // Retrieves the existing product by its ID from the repository layer.
            var existingProduct = _productrepo.GetProductsByID(id);

            // Throws an exception if the product is not found.
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            // Creates a new Product object with updated attributes.
            // Here, you'd likely replace attributes with updated values.
            var newUpdate = new Product
            {
                Name = existingProduct.Name, // Retains the existing name for simplicity.
                Price = existingProduct.Price, // Retains the existing price.
                Stock = existingProduct.Stock, // Retains the existing stock.
                Description = existingProduct.Description // Retains the existing description.
            };

            // Delegates the task of updating the product to the repository layer.
            _productrepo.UpdateProduct(newUpdate);
        }

        // Retrieves a product's details by its ID.
        public Product GetDetailsProductByID(int id)
        {
            // Delegates the task of fetching the product by ID to the repository layer.
            return _productrepo.GetProductsByID(id);
        }
    }

}
