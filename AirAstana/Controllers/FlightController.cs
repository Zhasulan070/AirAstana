using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AirAstana.Models;
using AirAstana.Models.Dto;
using AirAstana.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AirAstana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;
        private readonly IFlightDetails _flightDetails;

        public FlightController(ILogger<FlightController> logger, IFlightDetails flightDetails)
        {
            _logger = logger;
            _flightDetails = flightDetails;
        }

        [HttpPost("AddFlightDetails")]
        public async Task<IActionResult> AddFlightDetails(FlightDto dto)
        {
            var response = new Response<string>();
            try
            {
                response.StatusCode = 200;
                response.Result = await _flightDetails.AddFlightDetail(dto);
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in AddFlightDetails service";
                _logger.LogError(e, response.ErrorMessage);
            }

            return Ok(response);
        }
        
        [HttpPost("UpdateFlightDetails")]
        public async Task<IActionResult> UpdateFlightDetails([FromQuery(Name = "username")] string username,
            [FromQuery(Name = "guid")] Guid guid,[FromQuery(Name = "newStatus")] string newStatus)
        {
            var response = new Response<string>();
            try
            {
                response.StatusCode = 200;
                response.Result = await _flightDetails.UpdateFlightDetails( username, guid, newStatus);
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in UpdateFlightDetails service";
                _logger.LogError(e, response.ErrorMessage);
            }

            return Ok(response);
        }
        [HttpPost("GetFlightDetails")]
        public async Task<IActionResult> GetFlightDetails()
        {
            var response = new Response<List<Flight>>();
            try
            {
                response.StatusCode = 200;
                response.Result = await _flightDetails.GetFlightList();
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in GetFlightList service";
                _logger.LogError(e, response.ErrorMessage);
            }

            return Ok(response);
        }

    }
}