using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Account
    {
        [Key] [Column(TypeName = "varchar(100)")] public string Id { get; set; }
        [Required] [Column(TypeName = "varchar(20)")] public string FirstName { get; set; }
        [Required] [Column(TypeName = "varchar(30)")] public string LastName { get; set; }
        [Required] [Column(TypeName = "varchar(40)")] public string Email { get; set; }
        [Required] [Column(TypeName = "varchar(30)")] public string Password { get; set; }
    }
}
