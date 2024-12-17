using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject ApplicationDbContext
        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to add a user to the database
        public void AddProduct(Product product)
        {
            try
            {
                // Add the user to the context and save changes to the database
                _context.products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception (could be a database error, validation error, etc.)
                Console.WriteLine($"An error occurred while adding the product: {ex.Message}");
                // Optionally, you could throw the exception to be handled by a higher level
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }
        public void UpdateProduct(Product product)
        {
            _context.products.Update(product);
            _context.SaveChanges();
        }

        // Retrieves all products as IQueryable for further filtering and pagination
        public IQueryable<Product> GetAllProducts()
        {
            return _context.products.AsQueryable();
        }

        // Retrieves a count of all products in the database
        public int GetTotalProductCount()
        {
            return _context.products.Count();
        }

        public Product GetProductsByID(int id)
        {
            return _context.products.Where(us => us.Pid == id).FirstOrDefault();
        }
    }
}
