using Microsoft.EntityFrameworkCore;
using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject ApplicationDbContext
        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to add a user to the database
        public void AddUser(User user)
        {
            try
            {
                // Add the user to the context and save changes to the database
                _context.users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception (could be a database error, validation error, etc.)
                Console.WriteLine($"An error occurred while adding the user: {ex.Message}");
                // Optionally, you could throw the exception to be handled by a higher level
                throw new Exception("An error occurred while adding the user.", ex);
            }
        }

        // Method to retrieve a user by email and password
        public User GetUser(string email, string password)
        {
            try
            {
                // Query the database to find a user matching the provided email and password
                return _context.users
                    .Where(u => u.Email == email && u.Password == password) // Fixed & to &&
                    .FirstOrDefault(); // Returns the first match or null if no match found
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred while retrieving the user: {ex.Message}");
                // Optionally, rethrow the exception or return null
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }

        // Method to retrieve all users for a specific user ID (e.g., users related to an admin)
        public List<User> GetAllUsers(int iduser)
        {
            try
            {
                // Query the database to find users with the specified user ID
                return _context.users
                    .Where(u => u.Uid == iduser) // Filter users by the provided user ID
                    .ToList(); // Convert the result to a list
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
                // Optionally, rethrow the exception or return an empty list
                throw new Exception("An error occurred while retrieving users.", ex);
            }
        }
        public User GetUserByEmail(string email)
        {
            return _context.users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserByPassword(string password)
        {
            return _context.users.FirstOrDefault(u => u.Password == password);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.users
                .Where(u => u.Uid == userId)
                .FirstOrDefaultAsync();
        }

        public User GetUserByPhone(string phone)
        {
            return _context.users.FirstOrDefault(u => u.Phone == phone);
        }
        public User GetUserByIDFORAccess(int userId)
        {
            return _context.users.FirstOrDefault(u => u.Uid == userId);
        }

    }
}


