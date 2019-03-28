using System;
using System.Collections.Generic;

namespace WebApplication.DTO
{
    public class AdvertisementDetailsDTO
    {
        public Guid UserId { get; set; }
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
        public DateTime Date { get; set; }

        public List<ImageDTO> Images = new List<ImageDTO>();
    }
}
