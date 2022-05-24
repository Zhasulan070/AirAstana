using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstana.Models
{
    [Table("role")]
    public class Role
    {
        [Key]
        [Column("id")] public Guid Id { get; set; }
        [Column("code")] public string Code { get; set; }
    }
}