using System.Collections.Generic;
using System.Threading.Tasks;
using TechNationAPI.Dtos;
using TechNationAPI.Models;

namespace TechNationAPI.Services
{
    public interface ILogService
    {
        Task<Log> CreateLogAsync(CreateLogDto taskDto);
        Task<Log> UpdateLogAsync(int id, CreateLogDto taskDto);

        Task<Log> GetLogByIdAsync(int id);

        Task<List<Log>> GetAllLogsAsync();
    }
}
