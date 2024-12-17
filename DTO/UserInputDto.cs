using System.ComponentModel.DataAnnotations;
using SystemProductOrder.models;

namespace SystemProductOrder.DTO
{
    public class UserInputDto
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        //[Index(IsUnique =true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain at least one letter and one number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        
        public string Phone { get; set; }

        [Required(ErrorMessage = "Role is required.")]

        public Role Roles { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
