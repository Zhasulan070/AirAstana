using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirAstana.Models;
using AirAstana.Models.Dto;

namespace AirAstana.Services.Interfaces
{
    public interface IFlightDetails
    {
        Task<List<Flight>> GetFlightList();
        Task<string> AddFlightDetail(FlightDto dto);
        Task<string> UpdateFlightDetails(string username, Guid guid, string newStatus);
    }
}