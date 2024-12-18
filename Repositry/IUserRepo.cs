using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IUserRepo
    {
        void AddUser(User user);
        List<User> GetAllUsers(int iduser);
        User GetUser(string email, string password);
        User GetUserByEmail(string email);
        Task<User> GetUserById(int userId);
        User GetUserByIDFORAccess(int userId);
        User GetUserByPassword(string password);
        User GetUserByPhone(string phone);
    }
}