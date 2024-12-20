using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<Log> UpdateLogAsync(int id, CreateLogDto logDto)
        {
            var existingLog = await _context.LogsTechNation.FindAsync(id);

            if (existingLog == null)
            {
                throw new Exception("Log não encontrado.");
            }

            existingLog.AgoraLog = logDto.AgoraLog;
            existingLog.MinhaCdnLog = logDto.MinhaCdnLog;
            existingLog.CacheStatus = logDto.CacheStatus;
            existingLog.Date = logDto.Date;
            existingLog.Version = logDto.Version;
            existingLog.ResponseSize = logDto.ResponseSize;
            existingLog.Url = logDto.Url;
            existingLog.TimeTaken = logDto.TimeTaken;

            await _context.SaveChangesAsync();

            return existingLog;
        }

        public async Task<Log> GetLogByIdAsync(int id)
        {
            return await _context.LogsTechNation.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Log>> GetAllLogsAsync()
        {
            var query = _context.LogsTechNation.AsQueryable();

            return await query.ToListAsync();
        }
    }
}
