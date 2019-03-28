using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class AdvertisementImage
    {
        [Key] [Column(TypeName = "uniqueidentifier")] public Guid Id { get; set; }
        [Required] [Column(TypeName = "uniqueidentifier")] public Guid AdvertisementId { get; set; }
        [Required] [Column(TypeName = "nvarchar(max)")] public string Image { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string Description { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string Name { get; set; }

        [Required] [ForeignKey("AdvertisementId")] public Advertisement Advertisement { get; set; }
    }
}
