using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TechNationAPI;
using TechNationAPI.Converter;
using TechNationAPI.Dtos;
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
        private readonly IFileLogger _logger;
        private readonly string _logDirectory;
        private readonly string _logFileName;

        public TechNationController(ILogConverter converter,
            ILogService logService, IMapper mapper,
            IFileLogger logger,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _converter = converter;
            _logService = logService;
            _mapper = mapper;
            _logger = logger;
            _logDirectory = configuration.GetSection("FileLogging:LogDirectory").Value;
            _logFileName = configuration.GetSection("FileLogging:LogFileName").Value;
        }

        [HttpPost("convert")]
        public async Task<ActionResult> ConvertLogAsync([FromBody] ConvertLogRequest request)
        {
            if (request == null)
                return BadRequest("Informe o log pelo corpo da requisição");

            try
            {
                return await ProcessLogConversion(request.MinhaCdnLog, request.FormatoSaida);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao converter log: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("convertFromId")]
        public async Task<ActionResult> ConvertLogAsyncFromId(ConvertLogRequest request)
        {
            if (request == null)
                return BadRequest("Informe o log pelo corpo da requisição");

            try
            {
                var log = await _logService.GetLogByIdAsync(request.Id);
                if (log == null)
                    return NotFound("Informe o log pelo Id salvo no banco");

                string minhaCdnLog = string.IsNullOrEmpty(request.MinhaCdnLog) ? log.MinhaCdnLog : request.MinhaCdnLog;
                return await ProcessLogConversion(minhaCdnLog, request.FormatoSaida, request.Id);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao converter o log por Id: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("convert")]
        public async Task<ActionResult> ConvertLogFromQueryStringAsync([FromQuery] string minhaCdnLog)
        {
            if (string.IsNullOrEmpty(minhaCdnLog))
                return BadRequest("Informe o log pela URL: ?minhaCdnLog='Log Aqui'");

            try
            {
                return await ProcessLogConversion(minhaCdnLog, (int)FormatoSaida.RetornoChamada);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao converter o log da requisição: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveLogAsync")]
        public async Task<ActionResult<CreateLogDto>> SaveLogAsync(ConvertLogRequest request)
        {
            if (request == null)
                return BadRequest("Informe o log pelo corpo da requisição");

            try
            {
                return await ProcessLogConversion(request.MinhaCdnLog, (int)FormatoSaida.RetornoChamada);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao salvar log: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetLogAsync")]
        public async Task<ActionResult<CreateLogDto>> GetLogAsync(int id)
        {
            try
            {
                var log = await _logService.GetLogByIdAsync(id);
                if (log == null)
                {
                    return NotFound($"Log de Id {id} não encontrado.");
                }

                var logDto = _mapper.Map<CreateLogDto>(log);
                return Ok(logDto);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Erro ao recuperar log por ID: {ex.Message}");
                return StatusCode(500, "Um erro ocorreu ao recuperar o log.");
            }
        }

        [HttpGet("GetAllLogsDataBaseAsync")]
        public async Task<ActionResult<IEnumerable<CreateLogDto>>> GetAllLogsDataBaseAsync()
        {
            var logs = await _logService.GetAllLogsAsync();
            if (logs == null)
                return NotFound();

            var logsDto = _mapper.Map<IEnumerable<CreateLogDto>>(logs);
            return Ok(logsDto);
        }

        [HttpGet("GetAllLogsFile")]
        public ActionResult<IEnumerable<string>> GetAllLogsFile()
        {
            string logPath = Path.Combine(Directory.GetCurrentDirectory(), _logDirectory, _logFileName);
            var logsFile = _logger.ReadLogFromFile(logPath);
            if (logsFile == null)
                return NotFound();

            return Ok(logsFile);
        }

        private async Task<ActionResult> ProcessLogConversion(string minhaCdnLog, int formatoSaida, int? id = null)
        {
            if (string.IsNullOrEmpty(minhaCdnLog))
                return BadRequest("Informe o log pelo corpo da requisição");

            string agoraLog = _converter.Convert(minhaCdnLog);
            if (string.IsNullOrEmpty(agoraLog))
                return BadRequest("Erro ao converter o log");

            var logAgora = MinhaCdnLog.ConverterCdnAgora(minhaCdnLog);
            var logDto = _mapper.Map<CreateLogDto>(logAgora);
            logDto.MinhaCdnLog = minhaCdnLog;
            logDto.AgoraLog = agoraLog;

            string mensagemRetorno;
            if (formatoSaida == (int)FormatoSaida.RetornoChamada)
            {
                if (id.HasValue)
                    await _logService.UpdateLogAsync(id.Value, logDto);
                else
                    await _logService.CreateLogAsync(logDto);

                mensagemRetorno = $"Salvo no banco com sucesso log: {agoraLog}";
            }
            else if (formatoSaida == (int)FormatoSaida.SalvarServidorComCaminho)
            {
                string logPath = Path.Combine(Directory.GetCurrentDirectory(), _logDirectory, _logFileName);
                await _logger.LogAsync($"Log Minha CDN: {minhaCdnLog} - Log convertido: {agoraLog}");
                mensagemRetorno = $"Log Minha CDN: {minhaCdnLog} - Log convertido: {agoraLog}. Log salvado com sucesso no caminho: {logPath}";
            }
            else
            {
                return BadRequest("Formato de saída inválido, informe o formato de saída válido: 0 - Retorno na chamada e salvar no banco; 1 - Retorno do caminho do log salvo em arquivo de texto no servidor");
            }

            return Ok(mensagemRetorno);
        }
    }
}
