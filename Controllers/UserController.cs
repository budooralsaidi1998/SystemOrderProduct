﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SystemProductOrder.models;
using SystemProductOrder.Servieses;
using static SystemProductOrder.Servieses.UserServies;

namespace SystemProductOrder.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserServices userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;

        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult addUser([FromBody] User user)
        {
            try
            {
                _userService.AddUser(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(user.Uid);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult login(string email, string password)
        {

            var user = _userService.GetUser(email, password);

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
        //[HttpGet]
        //public IActionResult GetAllUsers(int userid)
        //{
        //    var user = _userService.GetAllUsers(userid);
        //}
        [NonAction]
        public string GenerateJwtToken(string userId, string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.UniqueName, username),
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
