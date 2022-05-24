using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirAstana.Models.Dto;

namespace AirAstana.Models
{
    [Table("flight")]
    public class Flight
    {
        [Key]
        [Column("id")] public Guid Id { get; set; }
        [Column("origin")] public string Origin { get; set; }
        [Column("destination")] public string Destination { get; set; }
        [Column("departure")] public DateTime Departure { get; set; }
        [Column("arrival")] public DateTime Arrival { get; set; }
        [Column("status")] public string Status { get; set; }
    }
}