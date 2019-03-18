using System;
using WebApplication.Models;

namespace WebApplication.DTO
{
    public class AdvertisementsDTO
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public string Category { get; set; }
    }
}
