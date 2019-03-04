using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly AccountContext _context;

        public LoginController(AccountContext context)
        {
            _context = context;
        }

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult> GetAccount(LoginCOM loginCOM)
        {
            var account = await _context.Users.Where(x =>
                x.Email == loginCOM.Email && x.Password == loginCOM.Password
                ).ToListAsync();

            if (!account.Any())
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            string securityKey = "super_top-Security^KEY-03*03*2019.smesk.io";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "user"));

            var token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "readers",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims
                );

            return Ok(new AccountDTO
            {
               Token = new JwtSecurityTokenHandler().WriteToken(token),
               FirstName = account[0].FirstName,
               LastName = account[0].LastName,
               Email = account[0].Email
            });
        }
    }
}
