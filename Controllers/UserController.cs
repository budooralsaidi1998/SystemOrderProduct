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
    public class UserController:ControllerBase
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
        public IActionResult addUser( UserInputDto user)
        {
            try
            {
                _userService.AddUser(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("User added successfully." );
        }

        [AllowAnonymous]
        [HttpGet("LOGIN")]
        public IActionResult login(string email, string password)
        {

            var user = _userService.login(email, password);

            if (user != null)
            {
                string token = GenerateJwtToken(user.Uid.ToString(), user.Name);
                return Ok(token);

            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }
        [HttpGet("GETDETALSUSER")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = User.Identity.Name;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;  // Custom claim, for example, "admin"
                return Ok(_userService.GetAllUsers(int.Parse(userId)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [NonAction]
        public string GenerateJwtToken(string userId, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
