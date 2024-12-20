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
        private readonly IFileLogger _logger;

        public LogService(ITechNationDbContext context, IMapper mapper, IFileLogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Log> CreateLogAsync(CreateLogDto logDto)
        {
            try
            {
                var log = _mapper.Map<Log>(logDto);
                _context.LogsTechNation.Add(log);
                await _context.SaveChangesAsync();
                await _logger.LogAsync($"Log criado: {log.Id}");
                return log;
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao criar log: {ex.Message}");
                throw new Exception("Um erro ocorreu ao criar o log.", ex);
            }
        }

        public async Task<Log> UpdateLogAsync(int id, CreateLogDto logDto)
        {
            try
            {
                var existingLog = await _context.LogsTechNation.FindAsync(id);
                if (existingLog == null)
                {
                    throw new KeyNotFoundException("Log não encontrado.");
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
                await _logger.LogAsync($"Log Alterado: {existingLog.Id}");
                return existingLog;
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao alterar log: {ex.Message}");
                throw new Exception("Um erro ocorreu ao atualizar o log.", ex);
            }
        }

        public async Task<Log> GetLogByIdAsync(int id)
        {
            try
            {
                var log = await _context.LogsTechNation.FirstOrDefaultAsync(t => t.Id == id);
                if (log == null)
                {
                    throw new KeyNotFoundException("Log não encontrado.");
                }
                return log;
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao recuperar log de Id: {ex.Message}");
                throw new Exception("Um erro ocorreu ao recuperar o log.", ex);
            }
        }

        public async Task<List<Log>> GetAllLogsAsync()
        {
            try
            {
                return await _context.LogsTechNation.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao recupear todos os logs: {ex.Message}");
                throw new Exception("Um erro ocorreu ao recuperar todos os logs.", ex);
            }
        }
    }
}
