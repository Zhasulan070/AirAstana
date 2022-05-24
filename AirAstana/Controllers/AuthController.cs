using System;
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
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserDetails _userDetails;

        public AuthController(ILogger<AuthController> logger, IUserDetails userDetails)
        {
            _logger = logger;
            _userDetails = userDetails;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromQuery(Name = "username")] string username, 
            [FromQuery(Name = "password")] string password, [FromQuery(Name = "role")] string role)
        {
            var response = new Response<string>();
            try
            {
                response.Result = await _userDetails.AddUser(username, password, role);
                response.StatusCode = 200;
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in RegisterUser service";
                _logger.LogError(e, response.ErrorMessage);
            }

            return Ok(response);
        }  
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromQuery(Name = "username")] string username, [FromQuery(Name = "password")] string password)
        {
            var response = new Response<string>();
            try
            {
                response.Result = await _userDetails.Login(username, password);
                response.StatusCode = 200;
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in RegisterUser service";
                _logger.LogError(e, response.ErrorMessage);
            }

            return Ok(response);
        }  
        
    }
}