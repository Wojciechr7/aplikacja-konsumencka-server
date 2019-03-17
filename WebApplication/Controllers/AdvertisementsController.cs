using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly DataBaseContext _context;


        public AdvertisementsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Advertisements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetAdvertisements()
        {
            List<AdvertisementsDTO> advertisementsDetailsDTO = new List<AdvertisementsDTO>();
            var advertisements =  await _context.Advertisements.ToListAsync();

            foreach(Advertisement adv in advertisements)
                advertisementsDetailsDTO.Add(new AdvertisementsDTO(adv));
            return advertisementsDetailsDTO;
        }

        // GET: api/Advertisements/5
        [HttpGet("{idStr}")]
        public async Task<ActionResult<AdvertisementDetailsDTO>> GetAdvertisement(string idStr)
        {
            Regex syntax = new Regex("^[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}$");
            if(!syntax.IsMatch(idStr))
                return StatusCode(418, "The ID structure of the advertisement is incorrect");

            Guid id = new Guid(idStr);

            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
                return StatusCode(419, "Advertisement with this id does not exist");

            var user = await _context.Users.Where(x =>x.Id == advertisement.UserId).ToListAsync();
            if (!user.Any())
                return StatusCode(420, "The owner of the advertisement can not be found");

            var image = await _context.AdvertisementImages.Where(x => x.AdvertisementId == advertisement.Id).ToListAsync();

            AdvertisementDetailsDTO advertisementDetailsDTO = new AdvertisementDetailsDTO(advertisement, user[0], image);

            return advertisementDetailsDTO;
        }

        // GET: api/Advertisements/latest/5/10
        [HttpGet("latest/{from}/{to}")]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetLatestAdvertisements(int from, int to)
        {
            if (from <= 0 || to <= 0)
                return StatusCode(417, "The parameters must be greater than 0 and smaller than 2 147 483 648");
            if (from >= to)
                return StatusCode(417, "The parameter 'for' must be less than parametr 'to'");

            var advertisements = await _context.Advertisements.OrderByDescending(x => x.Date).ToListAsync();

            if (to > advertisements.Count)
                to = advertisements.Count;
            if (from > advertisements.Count)
                return StatusCode(416, "There are no more advertisements");

            List<AdvertisementsDTO> advertisementsDetailsDTO = new List<AdvertisementsDTO>();
            for (int i=from-1; i<to; i++)
                advertisementsDetailsDTO.Add(new AdvertisementsDTO(advertisements[i]));
            return advertisementsDetailsDTO;
        }

        [HttpGet("latest/{quantity}")]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetLatestAdvertisements(int quantity)
        {
            if (quantity <= 0)
                return StatusCode(417, "The parameters must be greater than 0 and smaller than 2 147 483 648");

            var advertisements = await _context.Advertisements.OrderByDescending(x => x.Date).Take(quantity).ToListAsync();

            if (quantity < advertisements.Count)
                quantity = advertisements.Count;

            List<AdvertisementsDTO> advertisementsDetailsDTO = new List<AdvertisementsDTO>();
            foreach (Advertisement adv in advertisements)
                advertisementsDetailsDTO.Add(new AdvertisementsDTO(adv));

            return advertisementsDetailsDTO;
        }

        // GET: api/Advertisements/random/5
        [HttpGet("random/{quantity}")]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetRandomAdvertisementsRange(int quantity)
        {
            if (quantity <= 0)
                return StatusCode(417, "The parameter must be greater than 0 and smaller than 2 147 483 648");

            var advertisements = await _context.Advertisements.ToListAsync();

            if (quantity > advertisements.Count)
                quantity = advertisements.Count;

            List<int> indexes = new List<int>();
            int i = 0;
            while(i < quantity)
            {
                Random rnd = new Random();
                int number = rnd.Next(0, advertisements.Count);

                bool flag = true;
                foreach(int index in indexes)
                    if(index == number)
                    {
                        flag = false;
                        break;
                    }

                if(flag)
                {
                    indexes.Add(number);
                    i++;
                }
            }

            List<AdvertisementsDTO> randomAdvertisements = new List<AdvertisementsDTO>();
            foreach(int index in indexes)
                randomAdvertisements.Add( new AdvertisementsDTO(advertisements[index]));

            return randomAdvertisements;
        }

        // POST: api/Advertisements
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Advertisement>> PostAdvertisement(AdvertisementCOM advertisementCOM)
        {
            if(advertisementCOM.Description.Length > 500)
                return StatusCode(418, "Description of the advertisement must have max 500 characters");
            if (advertisementCOM.PhoneNumber.Length > 11)
                return StatusCode(418, "Phone number of the advertisement must have max 11 characters");
            if (advertisementCOM.City.Length > 100)
                return StatusCode(418, "City name of the advertisement must have max 100 characters");
            if (advertisementCOM.Street.Length > 100)
                return StatusCode(418, "Street name of the advertisement must have max 100 characters");
            if (advertisementCOM.Category.Length > 30)
                return StatusCode(418, "Category of the advertisement must have max 30 characters");

            foreach (ImageCOM img in advertisementCOM.Images)
                if(img.Description.Length > 100)
                    return StatusCode(418, "Description of the advertisement image must have max 100 characters");

            Guid _AdvertisementId = Guid.NewGuid();

            _context.Advertisements.Add(new Advertisement {
                Id = _AdvertisementId,
                UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Title = advertisementCOM.Title,
                Description = advertisementCOM.Description,
                PhoneNumber = advertisementCOM.PhoneNumber,
                Price = advertisementCOM.Price,
                City = advertisementCOM.City,
                Street = advertisementCOM.Street,
                Size = advertisementCOM.Size,
                Category = advertisementCOM.Category,
                Floor = advertisementCOM.Floor,
                Date = DateTime.Now
            });
            
            foreach(ImageCOM adv in advertisementCOM.Images)
            {
                _context.AdvertisementImages.Add(new AdvertisementImage
                {
                    Id = Guid.NewGuid(),
                    AdvertisementId = _AdvertisementId,
                    Image = adv.Image,
                    Description = adv.Description,
                    Name = adv.Name
                });
            }

            await _context.SaveChangesAsync();

            return Created("advertisements", null);
        }

        private bool AdvertisementExists(Guid id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }
    }
}
