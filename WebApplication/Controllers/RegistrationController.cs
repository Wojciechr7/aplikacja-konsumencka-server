using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Commends;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;

        public RegistrationController(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST: api/Registration
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<ActionResult<AccountCOM>> PostAccount([FromBody] AccountCOM accountCOM)
        {
            Account account = await _context.Users.SingleOrDefaultAsync(x =>
                x.Email == accountCOM.Email
                );

            if (account != null)
                return BadRequest(new { message = "This e-mail adress exist." });

            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(accountCOM.Password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            account = _mapper.Map<Account>(accountCOM);
            account.Password = hash;

            _context.Users.Add(account);
            await _context.SaveChangesAsync();

            return Created("users", null);
        }
    }
}
