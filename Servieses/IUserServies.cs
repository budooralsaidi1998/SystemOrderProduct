using SystemProductOrder.DTO;
using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IUserServies
    {
        void AddUser(UserInputDto user);
        List<User> GetAllUsers(int userid);
        User GetUserForAccess(int userid);
        User login(string email, string password);
    }
}