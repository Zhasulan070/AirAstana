using System.Threading.Tasks;
using AirAstana.Models.Dto;

namespace AirAstana.Services.Interfaces
{
    public interface IUserDetails
    {
        Task<string> AddUser(string username, string password, string role);
        
        Task<string> Login(string username, string password);
    }
}