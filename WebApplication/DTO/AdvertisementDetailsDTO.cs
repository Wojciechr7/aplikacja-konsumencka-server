using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.DTO
{
    public class AdvertisementDetailsDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }

        public string Title { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Floor { get; set; }
        public string Category { get; set; }

        public List<ImageDTO> Images = new List<ImageDTO>();

        public AdvertisementDetailsDTO(Advertisement advertisement, Account user, List<AdvertisementImage> image)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            Description = advertisement.Description;
            PhoneNumber = advertisement.PhoneNumber;
            Price = advertisement.Price;
            Size = advertisement.Size;

            Title = advertisement.Title;
            City = advertisement.City;
            Street = advertisement.Street;
            Floor = advertisement.Floor;
            Category = advertisement.Category;

            foreach (AdvertisementImage advImg in image)
                Images.Add( new ImageDTO(advImg.Image, advImg.Description, advImg.Name));
        }
    }
}
