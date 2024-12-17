using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Repositry;
using System.Security.Cryptography;
using BCrypt.Net;
namespace SystemProductOrder.Servieses
{
    public class UserServies :IUserServies
    {

        // Private readonly field to store the IUserRepo instance
        private readonly IUserRepo _userrepo;

        // Constructor to inject the IUserRepo dependency
        public UserServies(IUserRepo userrepo)
        {
            // Assigning the injected IUserRepo instance to the private field
            _userrepo = userrepo;
        }

        // Method to add a new user
        public void AddUser(UserInputDto user)
        {
            var existingUserByEmail = _userrepo.GetUserByEmail(user.Email); // New method in IUserRepo
            if (existingUserByEmail != null)
            {
                throw new ArgumentException("A user with this email already exists.");
            }

            // Check for duplicate password
            var existingUserByPassword = _userrepo.GetUserByPassword(user.Password); // New method in IUserRepo
            if (existingUserByPassword != null)
            {
                throw new ArgumentException("A user with this password already exists. Please choose a different password.");
            }
            var hashedPassword = HashPassword(user.Password);
            var completeUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                Roles = user.Roles,
                CreatedAt = user.CreatedAt,

            };
            // Calls the AddUser method of the IUserRepo implementation
            _userrepo.AddUser(completeUser);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Method to retrieve a user by email and password
        public User login(string email, string password)
        {
            var user = _userrepo.GetUserByEmail(email);
            if (user == null)
            {
                throw new ArgumentException("Invalid email or password.");
            }

            // 2. Verify the password against the hashed password in the database
            if (!VerifyPassword(user.Password, password))
            {
                throw new ArgumentException("Invalid email or password.");
            }

            return user; // Return the user if login is successful
        }

            // Delegates the task of retrieving the user to the IUserRepo implementation
           // return _userrepo.GetUser(email, password);
        
        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
        // Method to retrieve all users related to a specific user ID
        public List<User> GetAllUsers(int userid)
        {
            //var usser = new UserOutputDot
            //{
            //    Email = user.Email,
            //    Password = user.Password,
            //    Phone = user.Phone,
            //};

            // Delegates the task of retrieving all users to the IUserRepo implementation
            return _userrepo.GetAllUsers(userid);
        }



    }


}

