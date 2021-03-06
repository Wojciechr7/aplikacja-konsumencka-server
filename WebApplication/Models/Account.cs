﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Account
    {
        [Key] [Column(TypeName = "uniqueidentifier")] public Guid Id { get; set; }
        [Required] [Column(TypeName = "varchar(20)")] public string FirstName { get; set; }
        [Required] [Column(TypeName = "varchar(30)")] public string LastName { get; set; }
        [Required] [Column(TypeName = "varchar(11)")] public string PhoneNumber { get; set; }
        [Required] [Column(TypeName = "varchar(40)")] public string Email { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string Password { get; set; }
    }
}
