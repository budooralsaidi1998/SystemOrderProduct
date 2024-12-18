using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SystemProductOrder.DTO;
using SystemProductOrder.models;
using SystemProductOrder.Servieses;
using static SystemProductOrder.Servieses.UserServies;

namespace SystemProductOrder.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServies _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserServies userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;

        }
        [AllowAnonymous]
        [HttpPost("AddUSER")]
        public IActionResult addUser(UserInputDto user)
        {
            try
            {
                _userService.AddUser(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("User added successfully.");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For successful login
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For invalid credentials
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // For unauthorized access
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For unexpected errors
        public IActionResult Login(string email, string password)
        {
            try
            {
                // Authenticate the user via the service layer
                var user = _userService.login(email, password);

                if (user != null)
                {
                    // Generate claims for the user
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Uid.ToString()), // User ID
                new Claim(ClaimTypes.Name, user.Name),                    // User Name
                new Claim(ClaimTypes.Role, user.Roles.ToString()) ,
                new Claim("id", user.Uid.ToString())// User Role (Admin/NormalUser)
            };

                    // Generate JWT token using claims
                    string token = GenerateJwtToken(claims);
                    return Ok(new
                    {
                        Token = token,
                        Role = user.Roles.ToString(),         // Return the user's role
                        Message = "Login successful."
                    });
                }

                // If user is null, return BadRequest
                return BadRequest(new { Error = "Invalid credentials." });
            }
            catch (ArgumentException ex)
            {
                // Handle invalid email or password format
                return BadRequest(new { Error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle invalid credentials
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("GetDetailsUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetAllUsers()
        {
            try
            {
                // Extract claims from the token
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                {
                    return Forbid("Access denied. Unable to retrieve user details.");
                }

                // Call the service to retrieve all users
                var users = _userService.GetAllUsers(int.Parse(userId));

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [NonAction]
        public string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            // Generate security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
