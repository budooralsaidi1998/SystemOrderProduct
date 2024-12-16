using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IUserServices
    {
        void AddUser(User user);
        List<User> GetAllUsers(int userid);
        User GetUser(string email, string password);
    }
}