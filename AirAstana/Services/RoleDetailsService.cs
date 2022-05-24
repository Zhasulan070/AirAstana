using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using AirAstana.Context;
using AirAstana.Models;
using AirAstana.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AirAstana.Services
{
    public class RoleDetailsService : IRoleDetails
    {
        private readonly DatabaseContext _context;

        public RoleDetailsService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> AddRole(string name)
        {
            var message = string.Empty;
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    Code = name
                };
                var res = await _context.Roles.AddAsync(role);
                if (res != null)
                {
                    message = "Role added successfully";
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                message = "Catch exception" + e.Message;
                await _context.Database.RollbackTransactionAsync();
                await _context.SaveChangesAsync();
                throw new InvalidDataException(message);
            }

            return message;
        }

        public async Task<List<Role>> GetRolesList()
        {
            try
            {
                return await _context.Roles.ToListAsync();
            }
            catch (Exception e)
            {
                throw new NpgsqlException();
            }
        }
        
        public async Task<Guid> GetRoleId(string role)
        {
            
            var result = await _context.Roles.FirstOrDefaultAsync(x => x.Code == role);
            if (result == null)
            {
                throw new DataException("Role not found");
            }

            return result.Id;
        }
        
        public async Task<string> GetRoleCode(Guid id)
        {
            
            var result = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new DataException("Code of role not found");
            }

            return result.Code;
        }
    }
}