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
    [Route("api/[controller]")]
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

        //api/TechNation
        [HttpPost]
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
        //api/TechNation?minhaCdnLog
        [HttpGet]
        public async Task<ActionResult> ConvertLogURL(string minhaCdnLog)
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
        //api/TechNation/[id]
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateLogDto>> GetLog(int idLog)
        {
            var log = await _logService.GetLogByIdAsync(idLog);

            if (log == null)
            {
                return NotFound();
            }

            var logDto = _mapper.Map<CreateLogDto>(log);
            return Ok(logDto);
        }
    }
}
