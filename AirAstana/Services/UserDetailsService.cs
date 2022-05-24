using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AirAstana.Context;
using AirAstana.Models;
using AirAstana.Models.Dto;
using AirAstana.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AirAstana.Services
{
    public class UserDetailsService : IUserDetails
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRoleDetails _roleDetails;
        private readonly IMemoryCache _cache;

        public UserDetailsService(DatabaseContext context, IConfiguration configuration, IRoleDetails roleDetails, IMemoryCache cache)
        {
            _context = context;
            _configuration = configuration;
            _roleDetails = roleDetails;
            _cache = cache;
        }

        public async Task<string> AddUser(string username, string password, string role)
        {
            string message;
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Password = Encrypt(password),
                    RoleId = await _roleDetails.GetRoleId(role)
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                message = "User registered";
                AddUserToCache(user);
            }
            catch (Exception e)
            {
                message = "Catch exception" + e.Message;
                await _context.Database.RollbackTransactionAsync();
                await _context.SaveChangesAsync();
                throw;
            }

            return message;
        }

        public async Task<string> Login(string username, string password)
        {
            User user = null;
            if (!_cache.TryGetValue(username, out user))
            {
                user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (user != null)
                {
                    _cache.Set(user.Username, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
                    return user == null ? "User not found" : Decrypt(user.Password) != password ? "Incorrect password" : "User signed in";
                }
            }

            return Decrypt(user.Password) != password ? "Incorrect password" : "User signed in";
        }

        private string Encrypt(string text)
        {
            using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(_configuration.GetSection("HashKey").Value));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using var transform = tdes.CreateEncryptor();
            var textBytes = Encoding.UTF8.GetBytes(text);
            var bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        private string Decrypt(string cipher)
        {
            using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(_configuration.GetSection("HashKey").Value));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            using var transform = tdes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(cipher);
            var bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }

        private void AddUserToCache(User user)
        {
            _cache.Set(user.Username, user, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        }
        
    }
}