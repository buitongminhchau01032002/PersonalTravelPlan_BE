using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PersonalTravelPlan_BE.Dtos;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalTravelPlan_BE.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        public AuthController(IConfiguration config, IUserRepository userRepository) {
            _config = config;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public ActionResult<LoginResultDto> Login([FromBody] LoginDto loginInfo) {
            try {
                var user = _userRepository.GetUserByUsernameAndPassword(loginInfo.Username, loginInfo.Password);
                if (user != null) {
                    var token = GenerateToken(user);
                    return Ok(new LoginResultDto() { Username = loginInfo.Username, Token = token});
                }

                return NotFound("user not found");
            } catch (Exception ex) {
                return StatusCode(500);
            }
        }

        // To generate token
        private string GenerateToken(User user) {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddYears(10),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
