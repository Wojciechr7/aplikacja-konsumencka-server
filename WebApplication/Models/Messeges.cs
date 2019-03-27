using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Messeges
    {
             [Key] [Column(TypeName = "int")] public int? Id { get; set; }
        [Required] [Column(TypeName = "uniqueidentifier")] public Guid Sender { get; set; }
        [Required] [Column(TypeName = "uniqueidentifier")] public Guid Recipient { get; set; }
        [Required] [Column(TypeName = "nvarchar(max)")] public string Contents { get; set; }
        [Required] [Column(TypeName = "datetime")] public DateTime Date { get; set; }

        [Required][ForeignKey("Sender")]
        public virtual Account User { get; set; }
    }
}
