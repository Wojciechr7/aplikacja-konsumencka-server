using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Cities
    {
             [Key] [Column(TypeName = "int")] public int Id { get; set; }
        [Required] [Column(TypeName = "nvarchar(40)")] public string Name { get; set; }
        [Required] [Column(TypeName = "nvarchar(30)")] public string Voivodeship { get; set; }
    }
}
