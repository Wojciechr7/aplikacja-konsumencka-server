using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Commends;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    [Authorize]
    [ApiController]
    public class MessegesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public MessegesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Messeges/Sender/Recipient
        [HttpGet("{sender}/{recipient}")]
        public async Task<ActionResult<IEnumerable<Messeges>>> GetMesseges(string sender, string recipient)
        {
            Regex syntax = new Regex("^[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}$");
            if (!syntax.IsMatch(sender))
                return StatusCode(418, "Sender ID structure of the advertisement is incorrect");
            if (!syntax.IsMatch(recipient))
                return StatusCode(418, "Recipient ID structure of the advertisement is incorrect");

            Guid senderGuid = new Guid(sender);
            Guid recipientGuid = new Guid(recipient);

            var messeges = await _context.Messeges.Where(x=> x.Sender == senderGuid && x.Recipient == recipientGuid)
                .OrderByDescending(x => x.Date).ToListAsync();

            if (!messeges.Any())
                return StatusCode(416, "There is no conversation for these users");

            return messeges;
        }

        // POST: api/Messeges/Recipient
        [HttpPost("{recipient}")]
        public async Task<ActionResult<MessegesCOM>> PostMesseges(string recipient, MessegesCOM messegesCOM)
        {
            Regex syntax = new Regex("^[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}$");
            if (!syntax.IsMatch(recipient))
                return StatusCode(418, "Sender ID structure of the advertisement is incorrect");

            Guid senderGuid = new Guid(recipient);

            Messeges messeges = new Messeges
            {
                Id = null,
                Sender = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Recipient = new Guid(recipient),
                Contents = messegesCOM.Contents,
                Date = DateTime.Now
            };

            _context.Messeges.Add(messeges);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
