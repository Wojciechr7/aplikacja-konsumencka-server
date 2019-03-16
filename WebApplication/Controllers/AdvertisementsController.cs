using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            {
                advertisementsDetailsDTO.Add(new AdvertisementsDTO(adv.City, adv.Street, adv.Price, adv.Size, adv.Category));
            }
            return advertisementsDetailsDTO;
        }

        // GET: api/Advertisements/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<Advertisement>> GetAdvertisement(string id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);

            if (advertisement == null)
            {
                return NotFound();
            }

            return advertisement;
        }*/

        // PUT: api/Advertisements/5
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutAdvertisement(string id, Advertisement advertisement)
        {
            if (id != advertisement.Id)
            {
                return BadRequest();
            }

            _context.Entry(advertisement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

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

            string _AdvertisementId = Guid.NewGuid().ToString();

            _context.Advertisements.Add(new Advertisement {
                Id = _AdvertisementId,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Title = advertisementCOM.Title,
                Description = advertisementCOM.Description,
                PhoneNumber = advertisementCOM.PhoneNumber,
                Price = advertisementCOM.Price,
                City = advertisementCOM.City,
                Street = advertisementCOM.Street,
                Size = advertisementCOM.Size,
                Category = advertisementCOM.Category,
                Floor = advertisementCOM.Floor
            });
            
            foreach(ImageCOM adv in advertisementCOM.Images)
            {
                _context.AdvertisementImages.Add(new AdvertisementImage
                {
                    Id = Guid.NewGuid().ToString(),
                    AdvertisementId = _AdvertisementId,
                    Image = adv.Image,
                    Description = adv.Description,
                    Name = adv.Name
                });
            }

            await _context.SaveChangesAsync();

            return Created("advertisements", null);
        }

        // DELETE: api/Advertisements/5
        /*[HttpDelete("{id}")]
        public async Task<ActionResult<Advertisement>> DeleteAdvertisement(string id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();

            return advertisement;
        }*/

        private bool AdvertisementExists(string id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }
    }
}
