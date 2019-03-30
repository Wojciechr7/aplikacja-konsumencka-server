﻿using System;
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
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;


        public AdvertisementsController(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Advertisements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetAdvertisements()
        {
            var advertisements =  await _context.Advertisements.ToListAsync();

            return _mapper.Map<List<AdvertisementsDTO>>(advertisements); ;
        }

        // GET: api/Advertisements/5
        [HttpGet("{idStr}")]
        public async Task<ActionResult<AdvertisementDetailsDTO>> GetAdvertisement(string idStr)
        {
            Regex syntax = new Regex("^[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}$");
            if(!syntax.IsMatch(idStr.ToLower()))
                return StatusCode(418, "The ID structure of the advertisement is incorrect");

            Guid id = new Guid(idStr);

            Advertisement advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
                return StatusCode(419, "Advertisement with this id does not exist");

            var user = await _context.Users.SingleOrDefaultAsync(x =>x.Id == advertisement.UserId);
            if (user == null)
                return StatusCode(420, "The owner of the advertisement can not be found");

            List<AdvertisementImage> images = await _context.AdvertisementImages.Where(x => x.AdvertisementId == advertisement.Id).ToListAsync();
            Cities city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == advertisement.City);

            var advertisementDetailsDTO = _mapper.Map<Advertisement, AdvertisementDetailsDTO>(advertisement);
            advertisementDetailsDTO = _mapper.Map(user, advertisementDetailsDTO);
            advertisementDetailsDTO = _mapper.Map(images, advertisementDetailsDTO);

            return advertisementDetailsDTO;
        }

        // GET: api/Advertisements/city/desc:2
        [HttpGet("{parameter}/{type}:{page}")]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetLatestAdvertisements(string parameter, string type, int page)
        {
            parameter = parameter.ToLower();
            type = type.ToLower();

            if (parameter != "price" && parameter != "city" && parameter != "size" && parameter != "category" && parameter != "date")
                return StatusCode(417, "Parameter name not exist");

            if (page <= 0)
                return StatusCode(417, "Number page must be greater than zero");

            if (type != "asc" && type != "desc")
                return StatusCode(417, "Type of sort not exist");

            List<Advertisement> advertisements = new List<Advertisement>();

            if (parameter == "price" && type == "desc")
                advertisements = await _context.Advertisements.OrderByDescending(x => x.Price).ToListAsync();

            else if (parameter == "city" && type == "desc")
                advertisements = await _context.Advertisements.OrderByDescending(x => x.City).ToListAsync();

            else if (parameter == "size" && type == "desc")
                advertisements = await _context.Advertisements.OrderByDescending(x => x.Size).ToListAsync();

            else if (parameter == "category" && type == "desc")
                advertisements = await _context.Advertisements.OrderByDescending(x => x.Category).ToListAsync();

            else if (parameter == "date" && type == "desc")
                advertisements = await _context.Advertisements.OrderByDescending(x => x.Date).ToListAsync();

            else if (parameter == "city" && type == "asc")
                advertisements = await _context.Advertisements.OrderBy(x => x.City).ToListAsync();

            else if (parameter == "size" && type == "asc")
                advertisements = await _context.Advertisements.OrderBy(x => x.Size).ToListAsync();

            else if (parameter == "category" && type == "asc")
                advertisements = await _context.Advertisements.OrderBy(x => x.Category).ToListAsync();

            else if (parameter == "date" && type == "asc")
                advertisements = await _context.Advertisements.OrderBy(x => x.Date).ToListAsync();

            else if (parameter == "price" && type == "asc")
                advertisements = await _context.Advertisements.OrderBy(x => x.Price).ToListAsync();

            if (advertisements.Count < page * 10 - 10)
                return StatusCode(418, "No data to display");

            var advDTO = _mapper.Map<List<AdvertisementsDTO>>(advertisements.Skip(page * 10 - 10).Take(10));

            foreach(AdvertisementsDTO a in advDTO)
            {
                var City = await _context.Cities.SingleOrDefaultAsync(x => x.Id == int.Parse(a.City));
                a.City = City.Name;
            }

            return advDTO;
        }

        // GET: api/Advertisements/latest/5
        [HttpGet("latest/{quantity}")]
        public async Task<ActionResult<IEnumerable<AdvertisementsDTO>>> GetLatestAdvertisements(int quantity)
        {
            if (quantity <= 0)
                return StatusCode(417, "The parameters must be greater than 0 and smaller than 2 147 483 648");

            var advertisements = await _context.Advertisements.OrderByDescending(x => x.Date).Take(quantity).ToListAsync();

            if (quantity < advertisements.Count)
                quantity = advertisements.Count;

            return _mapper.Map<List<AdvertisementsDTO>>(advertisements.Take(quantity));
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
                randomAdvertisements.Add(_mapper.Map<AdvertisementsDTO>(advertisements[index]));

            return randomAdvertisements;
        }

        // POST: api/Advertisements
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Advertisement>> PostAdvertisement(AdvertisementCOM advertisementCOM)
        {
            if(advertisementCOM.Description.Length > 500)
                return StatusCode(418, "Description of the advertisement must have max 500 characters");
            if (advertisementCOM.Street.Length > 100)
                return StatusCode(418, "Street name of the advertisement must have max 100 characters");
            if (advertisementCOM.Category.Length > 30)
                return StatusCode(418, "Category of the advertisement must have max 30 characters");

             List<Cities> City = await _context.Cities.Where(x => x.Id == advertisementCOM.City).ToListAsync();

            if(!City.Any())
                return StatusCode(418, "City ID doesn't exist in database");

            foreach (ImageCOM img in advertisementCOM.Images)
                if(img.Description.Length > 100)
                    return StatusCode(418, "Description of the advertisement image must have max 100 characters");

            Guid _AdvertisementId = Guid.NewGuid();

            Advertisement advertisement = _mapper.Map<Advertisement>(advertisementCOM);
            advertisement.Id = _AdvertisementId;
            advertisement.UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            _context.Advertisements.Add(advertisement);

            foreach(ImageCOM adv in advertisementCOM.Images)
            {
                AdvertisementImage img = _mapper.Map<AdvertisementImage>(adv);
                img.AdvertisementId = _AdvertisementId;
                _context.AdvertisementImages.Add(img);
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
