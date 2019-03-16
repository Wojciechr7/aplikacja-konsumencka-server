using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication.Commends;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public LoginController(DataBaseContext context)
        {
            _context = context;
        }

        // POST: api/Login
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<ActionResult> GetAccount(LoginCOM loginCOM)
        {
            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginCOM.Password));
            string encodeHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            var account = await _context.Users.Where(x =>
                x.Email == loginCOM.Email && x.Password == encodeHash
                ).ToListAsync();

            if (!account.Any())
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            string securityKey = "super_top-Security^KEY-03*03*2019.smesk.io";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claim = new[] {
                    new Claim(ClaimTypes.NameIdentifier, account[0].Id)
            };

            var token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "readers",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claim
                );

            return Ok(new  AccountDTO
            {
               Token = new JwtSecurityTokenHandler().WriteToken(token),
               FirstName = account[0].FirstName,
               LastName = account[0].LastName,
               Email = account[0].Email,
        });
        }
    }
}
