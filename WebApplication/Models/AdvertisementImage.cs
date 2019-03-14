using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class AdvertisementImage
    {
        [Key] [Column(TypeName = "varchar(100)")] public string Id { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string AdvertisementId { get; set; }
        [Required] [Column(TypeName = "nvarchar(max)")] public string Image { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string Description { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string Name { get; set; }
    }
}
