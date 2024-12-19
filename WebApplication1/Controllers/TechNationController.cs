using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechNationAPI.Converter;
using TechNationAPI.Dtos;
using TechNationAPI.Models;
using TechNationAPI.Services;

namespace WebApplication1.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class TechNationController : ControllerBase
    {
        private readonly ILogConverter _converter;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public TechNationController(ILogConverter converter,ILogService logService, IMapper mapper)
        {
            _converter = converter;
            _logService = logService;
            _mapper = mapper;
        }

        [HttpPost("convert")]
        public async Task<ActionResult> ConvertLogAsync([FromBody] string minhaCdnLog)
        {
            try
            {
                if (!string.IsNullOrEmpty(minhaCdnLog))
                {
                    string agoraLog = _converter.Convert(minhaCdnLog);
                    if (!string.IsNullOrEmpty(agoraLog))
                    {
                        var cdnlog = MinhaCdnLog.ConverterCdnAgora(minhaCdnLog);
                        var logDto = _mapper.Map<CreateLogDto>(cdnlog);
                        logDto.MinhaCdnLog = minhaCdnLog;
                        var createdTask = await _logService.CreateLogAsync(logDto);
                    }
                    return Ok(agoraLog);
                }

                return NotFound("Informe o log pelo corpo da requisição");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("convert/{id}")]
        public async Task<ActionResult> ConvertLogAsyncFromId(int id)
        {
            var log = await _logService.GetLogByIdAsync(id);

            if (log == null)
            {
                return NotFound();
            }
            var minhaCdnLog = log.MinhaCdnLog ?? string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(minhaCdnLog))
                {
                    string agoraLog = _converter.Convert(minhaCdnLog);
                    return Ok(agoraLog);
                }

                return NotFound("Informe o log pelo Id salvo no banco");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("convert")]
        public async Task<ActionResult> ConvertLogFromQueryStringAsync([FromQuery] string minhaCdnLog)
        {
            try
            {
                if (!string.IsNullOrEmpty(minhaCdnLog))
                {
                    string agoraLog = _converter.Convert(minhaCdnLog);
                    if (!string.IsNullOrEmpty(agoraLog))
                    {
                        var cdnlog = MinhaCdnLog.ConverterCdnAgora(minhaCdnLog);
                        var logDto = _mapper.Map<CreateLogDto>(cdnlog);
                        logDto.MinhaCdnLog = minhaCdnLog;
                        var createdTask = await _logService.CreateLogAsync(logDto);
                    }
                    return Ok(agoraLog);
                }

                return NotFound("Informe o log pela URL: ?minhaCdnLog='Log Aqui'");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetLogAsync")]
        public async Task<ActionResult<CreateLogDto>> GetLogAsync(int id)
        {
            var log = await _logService.GetLogByIdAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            var logDto = _mapper.Map<CreateLogDto>(log);
            return Ok(logDto);
        }
    }
}
