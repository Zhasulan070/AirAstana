using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AirAstana.Context;
using AirAstana.Models;
using AirAstana.Models.Dto;
using AirAstana.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace AirAstana.Services
{
    public class FlightDetailsService : IFlightDetails
    {
        private readonly DatabaseContext _context;
        private readonly IRoleDetails _roleDetails;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public FlightDetailsService(DatabaseContext context, IRoleDetails roleDetails, IConfiguration configuration, IMemoryCache cache)
        {
            _context = context;
            _roleDetails = roleDetails;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<List<Flight>> GetFlightList()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<string> AddFlightDetail(FlightDto dto)
        {
            var message = string.Empty;
            try
            {
                if (await CheckPermission(dto.Username) == false)
                {
                    return "You don't have permission to update the flight details";
                }

                var flight = new Flight
                {
                    Id = Guid.NewGuid(),
                    Origin = dto.Origin,
                    Destination = dto.Destination,
                    Departure = dto.Departure,
                    Arrival = dto.Arrival,
                    Status = dto.Status
                };

                await _context.Flights.AddAsync(flight);
                await _context.SaveChangesAsync();
                return "Flight details added successfully";
            }
            catch (Exception e)
            {
                message = "Catch exception" + e.Message;
                await _context.Database.RollbackTransactionAsync();
                await _context.SaveChangesAsync();
                throw new HttpRequestException();
            }
        }

        public async Task<string> UpdateFlightDetails(string username, Guid guid, string newStatus)
        {
            var message = string.Empty;
            try
            {
                if (await CheckPermission(username) == false)
                {
                    return "You don't have permission to update the flight details";
                }
                
                var flight = await _context.Flights.FirstOrDefaultAsync(x => x.Id == guid);
                if (flight == null)
                {
                    return "Flight details not found";
                }

                flight.Status = newStatus;
                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();
                return "Flight details updated successfully";
            }
            catch (Exception e)
            {
                message = "Catch exception" + e.Message;
                await _context.Database.RollbackTransactionAsync();
                await _context.SaveChangesAsync();
                throw new HttpRequestException();
            }
            
        }

        private async Task<bool> CheckPermission(string username)
        {
            User user = null;
            var roleCode = string.Empty;
            if (!_cache.TryGetValue(username, out user))
            {
                user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (user != null)
                {
                    _cache.Set(user.Username, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
                    roleCode = await _roleDetails.GetRoleCode(user.RoleId);
                    return roleCode.ToLower() == _configuration.GetSection("AdminRole").Value;
                }
            }
            roleCode = await _roleDetails.GetRoleCode(user.RoleId);
            return roleCode.ToLower() == _configuration.GetSection("AdminRole").Value;
        }
    }
}