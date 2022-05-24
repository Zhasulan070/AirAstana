using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirAstana.Models;

namespace AirAstana.Services.Interfaces
{
    public interface IRoleDetails
    {
        Task<string> AddRole(string name);
        Task<List<Role>> GetRolesList();
        Task<Guid> GetRoleId(string role);
        Task<string> GetRoleCode(Guid id);
    }
}