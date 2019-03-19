using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public LoginController(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST: api/Login
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<ActionResult> GetAccount(LoginCOM loginCOM)
        {
            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginCOM.Password));
            string encodeHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            Account account = await _context.Users.SingleOrDefaultAsync(x =>
                x.Email == loginCOM.Email && x.Password == encodeHash
                );

            if (account == null)
                return BadRequest(new { message = "Invalid credentials." });

            string securityKey = "super_top-Security^KEY-03*03*2019.smesk.io";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claim = new[] {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "readers",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claim
                );

            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);
            accountDTO.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(accountDTO);
        }
    }
}
