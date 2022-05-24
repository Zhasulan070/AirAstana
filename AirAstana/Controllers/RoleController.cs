using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AirAstana.Context;
using AirAstana.Models;
using AirAstana.Services;
using AirAstana.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirAstana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleDetails _roleDetails;

        public RoleController(ILogger<RoleController> logger, IRoleDetails roleDetails)
        {
            _logger = logger;
            _roleDetails = roleDetails;
        }

        [HttpGet("GetRolesList")]
        public async Task<IActionResult> GetRolesList()
        {
            var response = new Response<List<Role>>();
            try
            {
                response.StatusCode = 200;
                response.Result = await _roleDetails.GetRolesList();
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in GetRolesList service";
                _logger.LogError(e, response.ErrorMessage);
            }
            return Ok(response);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromQuery(Name = "name")] string name)
        {
            var response = new Response<string>();
            try
            {
                response.Result = await _roleDetails.AddRole(name);
                response.StatusCode = 200;
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                response.ErrorMessage = "Some error in AddRole service";
                _logger.LogError(e, response.ErrorMessage);
            }
            
            return Ok(response);
        }
        
        
    }
}
