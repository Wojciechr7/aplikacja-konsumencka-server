using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;

        public CitiesController(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cities>>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        // GET: api/Cities/Voivodeship
        [HttpGet("{voivodeship}")]
        public async Task<ActionResult<IEnumerable<CitiesDTO>>> GetCities(string voivodeship)
        {
            voivodeship = voivodeship.ToLower();

            List<Cities> cities = await _context.Cities.Where(x => RemoveDiacritics(x.Voivodeship) == voivodeship).ToListAsync();

            if (!cities.Any())
                return NotFound();

            List<CitiesDTO> citiesDTO = _mapper.Map<List<CitiesDTO>>(cities);

            return citiesDTO;
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
