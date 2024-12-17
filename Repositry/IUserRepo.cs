using SystemProductOrder.models;

namespace SystemProductOrder.Repositry
{
    public interface IUserRepo
    {
        void AddUser(User user);
        List<User> GetAllUsers(int iduser);
        User GetUser(string email, string password);
        User GetUserByEmail(string email);
        User GetUserByPassword(string password);
    }
}