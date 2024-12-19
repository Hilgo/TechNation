using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TechNationAPI.Data;
using TechNationAPI.Dtos;
using TechNationAPI.Models;

namespace TechNationAPI.Services
{
    public class LogService : ILogService
    {
        private readonly ITechNationDbContext _context;
        private readonly IMapper _mapper;

        public LogService(ITechNationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<Log> CreateLogAsync(CreateLogDto logDto)
        {
            var log = _mapper.Map<Log>(logDto);

            _context.LogsTechNation.Add(log);
            await _context.SaveChangesAsync();

            return log;
        }

        public async Task<Log> GetLogByIdAsync(int id)
        {
            return await _context.LogsTechNation.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
