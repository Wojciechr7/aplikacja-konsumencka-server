using WebApplication.Models;

namespace WebApplication.DTO
{
    public class AdvertisementsDTO
    {
        public string Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public string Category { get; set; }

        public AdvertisementsDTO(Advertisement advertisement)
        {
            Id = advertisement.Id;
            City = advertisement.City;
            Street = advertisement.Street;
            Price = advertisement.Price;
            Size = advertisement.Size;
            Category = advertisement.Category;
        }
    }
}
