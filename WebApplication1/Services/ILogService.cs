using System.Threading.Tasks;
using TechNationAPI.Dtos;
using TechNationAPI.Models;

namespace TechNationAPI.Services
{
    public interface ILogService
    {
        Task<Log> CreateLogAsync(CreateLogDto taskDto);

        Task<Log> GetLogByIdAsync(int id);
    }
}
