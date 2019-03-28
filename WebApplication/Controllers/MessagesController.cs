using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Commends;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    [Authorize]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;

        public MessagesController(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Messages/Recipient
        [HttpGet("{recipient}")]
        public async Task<ActionResult<IEnumerable<MessagesDTO>>> GetMessages(string recipient)
        {
            Regex syntax = new Regex("^[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}$");
            if (!syntax.IsMatch(recipient.ToLower()))
                return StatusCode(418, "Recipient ID structure of the advertisement is incorrect");

            Guid user1 = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Guid user2 = new Guid(recipient);

            var messages = await _context.Messages.Where( x=>
                (x.Sender == user1 && x.Recipient == user2) ||
                (x.Sender == user2 && x.Recipient == user2)
                ).OrderByDescending(x => x.Date).ToListAsync();

            return _mapper.Map<List<MessagesDTO>>(messages);
        }

        // POST: api/Messages/Recipient
        [HttpPost("{recipient}")]
        public async Task<ActionResult<MessagesCOM>> PostMessages(string recipient, MessagesCOM messagesCOM)
        {
            Regex syntax = new Regex("^[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}$");
            if (!syntax.IsMatch(recipient.ToLower()))
                return StatusCode(418, "Sender ID structure of the advertisement is incorrect");

            Guid senderGuid = new Guid(recipient);

            Messages messagas = new Messages
            {
                Id = null,
                Sender = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Recipient = new Guid(recipient),
                Contents = messagesCOM.Contents,
                Date = DateTime.Now
            };

            _context.Messages.Add(messagas);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
