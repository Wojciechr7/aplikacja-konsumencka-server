﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Advertisement
    {
             [Key] [Column(TypeName = "uniqueidentifier")] public Guid Id { get; set; }
        [Required] [Column(TypeName = "uniqueidentifier")] public Guid UserId { get; set; }
        [Required] [Column(TypeName = "varchar(25)")] public string Title { get; set; }
        [Required] [Column(TypeName = "varchar(500)")] public string Description { get; set; }
        [Required] [Column(TypeName = "decimal(20, 2)")] public decimal Price { get; set; }
        [Required] [Column(TypeName = "int")] public int City { get; set; }
        [Required] [Column(TypeName = "varchar(100)")] public string Street { get; set; }
        [Required] [Column(TypeName = "int")] public int Floor { get; set; }
        [Required] [Column(TypeName = "decimal(20, 2)")] public decimal Size { get; set; }
                   [Column(TypeName = "varchar(30)")] public string Category { get; set; }
        [Required] [Column(TypeName = "datetime")] public DateTime Date { get; set; }

        [Required] [ForeignKey("UserId")] public Account User { get; set; }
        [Required] [ForeignKey("City")] public Cities Location { get; set; }
    }
}
