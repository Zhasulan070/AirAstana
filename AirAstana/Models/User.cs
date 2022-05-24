using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstana.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")] public Guid Id { get; set; }
        [Column("username")] public string Username { get; set; }
        [Column("password")] public string Password { get; set; }
        [Column("role_id")] public Guid RoleId { get; set; }
    }
}