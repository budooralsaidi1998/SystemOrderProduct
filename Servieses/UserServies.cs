using SystemProductOrder.models;
using SystemProductOrder.Repositry;

namespace SystemProductOrder.Servieses
{
    public class UserServies : IUserServices
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
        public void AddUser(User user)
        {
            // Calls the AddUser method of the IUserRepo implementation
            _userrepo.AddUser(user);
        }

        // Method to retrieve a user by email and password
        public User GetUser(string email, string password)
        {
            // Delegates the task of retrieving the user to the IUserRepo implementation
            return _userrepo.GetUser(email, password);
        }

        // Method to retrieve all users related to a specific user ID
        public List<User> GetAllUsers(int userid)
        {
            // Delegates the task of retrieving all users to the IUserRepo implementation
            return _userrepo.GetAllUsers(userid);
        }
    }





}

