namespace WebApplication.DTO
{
    public class AdvertisementsDTO
    {
        public string City { get; set; }
        public string Street { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public string Category { get; set; }

        public AdvertisementsDTO(string City, string Street, decimal Price, decimal Size, string Category)
        {
            this.City = City;
            this.Street = Street;
            this.Price = Price;
            this.Size = Size;
            this.Category = Category;
        }
    }
}
