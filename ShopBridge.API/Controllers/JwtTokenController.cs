using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopBridge.DTO.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopBridge.API
{
    [AllowAnonymous]
    [ApiController]
    public class JwtTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public JwtTokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("GenerateToken")]
        public ActionResult GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration.GetValue<string>("JwtTokeParameters:SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Issuer = _configuration.GetValue<string>("JwtTokeParameters:Issuer"),
                Audience = _configuration.GetValue<string>("JwtTokeParameters:Audience"),
                Expires = DateTime.UtcNow.AddMonths(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            }; 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JsonResult(tokenHandler.WriteToken(token));
        }
    }
}
