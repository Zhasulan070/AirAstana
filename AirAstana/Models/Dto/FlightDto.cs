using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstana.Models.Dto
{
    public class FlightDto
    {
        [Column("username")] public string Username { get; set; }
        [Column("origin")] public string Origin { get; set; }
        [Column("destination")] public string Destination { get; set; }
        [Column("departure")] public DateTime Departure { get; set; }
        [Column("arrival")] public DateTime Arrival { get; set; }
        [Column("status")] public string Status { get; set; }
    }
    
}