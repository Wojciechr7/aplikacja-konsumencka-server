namespace WebApplication.DTO
{
    public class ImageDTO
    {
        public string Image { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public ImageDTO(string Image, string Description, string Name)
        {
            this.Image = Image;
            this.Description = Description;
            this.Name = Name;
        }
    }
}
