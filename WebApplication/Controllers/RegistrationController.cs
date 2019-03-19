using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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

        public RegistrationController(DataBaseContext context)
        {
            _context = context;
        }

        // POST: api/Registration
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<ActionResult<AccountCOM>> PostAccount([FromBody] AccountCOM accountCOM)
        {
            var account = await _context.Users.SingleOrDefaultAsync(x =>
                x.Email == accountCOM.Email
                );

            if (account != null)
                return BadRequest(new { message = "This e-mail adress exist." });

            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(accountCOM.Password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            _context.Users.Add(new Account
            {
                Id = Guid.NewGuid(),
                FirstName = accountCOM.FirstName,
                LastName = accountCOM.LastName,
                Email = accountCOM.Email,
                Password = hash
            });
            await _context.SaveChangesAsync();

            return Created("users", null);
        }
    }
}
