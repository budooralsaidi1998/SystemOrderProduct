﻿using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        public void AddProduct(ProductInput input, ClaimsPrincipal user)
        {
            //Creates a new Product object from the input data transfer object(DTO).
            var isAdmin = user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only admin users can add products.");
            }

            //var nameproduct=_productrepo.GetNameProduct(input.Name);
            //if (nameproduct != null)
            //    {

            //    throw new ArgumentException("Product name is already exits .");
            //}
            {
                
            }
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new ArgumentException("Product name is required and cannot be empty.");
            }

            if (input.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }

            if (input.Stock < 0)
            {
                throw new ArgumentException("Product stock cannot be negative.");
            }

            //if (input.Description?.Length > 500)
            //{
            //    throw new ArgumentException("Product description cannot exceed 500 characters.");
            //}
            try
            {
                // Create a new Product object
                var newProduct = new Product
                {
                    Name = input.Name.Trim(), // Ensure no trailing spaces
                    Price = input.Price,
                    Stock = input.Stock,
                    Description = input.Description // Handle null description gracefully
                };

                // Add the product to the repository
                _productrepo.AddProduct(newProduct);
            }
            catch (Exception ex)
            {
                // Log the error (if logging is implemented)
                Console.WriteLine($"Error adding product: {ex.Message}");

                // Rethrow the exception for higher-level handling
                throw new InvalidOperationException("An error occurred while adding the product.", ex);
            }
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
        public void UpdateProduct(int id, ProductInput updatedProduct, ClaimsPrincipal user)
        {
            // Check if the user is an admin
            var isAdmin = user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only admin users can update products.");
            }

            // Retrieve the existing product by ID from the repository
            var existingProduct = _productrepo.GetProductsByID(id);

            // Throw an exception if the product is not found
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            // Update the product with new values from the DTO
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Stock = updatedProduct.Stock ;
            existingProduct.Description = updatedProduct.Description;

            // Delegate the task of updating the product to the repository
            _productrepo.UpdateProduct(existingProduct);
        }

        // Retrieves a product's details by its ID.
        public Product GetDetailsProductByID(int id)
        {
            // Delegates the task of fetching the product by ID to the repository layer.
            return _productrepo.GetProductsByID(id);
        }
    }

}
