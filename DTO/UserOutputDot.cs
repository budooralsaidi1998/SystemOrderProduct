using System.ComponentModel.DataAnnotations;
using SystemProductOrder.models;

namespace SystemProductOrder.DTO
{
    public class UserOutputDot
    {
        public string Name { get; set; }

        
        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public Role Roles { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
