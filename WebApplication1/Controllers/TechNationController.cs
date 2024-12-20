using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TechNationAPI;
using TechNationAPI.Converter;
using TechNationAPI.Dtos;
using TechNationAPI.Migrations;
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
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;


        public TechNationController(ILogConverter converter,
            ILogService logService, IMapper mapper,
            IFileLogger logger,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _converter = converter;
            _logService = logService;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("convert")]
        public async Task<ActionResult> ConvertLogAsync([FromBody] ConvertLogRequest request)
        {
            if (request == null)
                return BadRequest("Informe o log pelo corpo da requisição");

            try
            {

                string minhaCdnLog = request.MinhaCdnLog;
                int formatoSaida = request.FormatoSaida;

                if (!string.IsNullOrEmpty(minhaCdnLog))
                {
                    string agoraLog = _converter.Convert(minhaCdnLog);
                    string mensagemRetorno = "";
                    if (!string.IsNullOrEmpty(agoraLog))
                    {

                        var logAgora = MinhaCdnLog.ConverterCdnAgora(minhaCdnLog);

                        if (formatoSaida == (int)FormatoSaida.RetornoChamada)
                        {
                            var logDto = _mapper.Map<CreateLogDto>(logAgora);
                            logDto.MinhaCdnLog = minhaCdnLog;
                            logDto.AgoraLog = agoraLog;
                            var createdTask = await _logService.CreateLogAsync(logDto);
                            mensagemRetorno = $"Salvo no banco com sucesso log: {agoraLog}";
                        }
                        else if (formatoSaida == (int)FormatoSaida.SalvarServidorComCaminho)
                        {
                            string logDirectory = _configuration.GetSection("FileLogging:LogDirectory").Value;
                            string logFileName = _configuration.GetSection("FileLogging:LogFileName").Value;
                            string logPath = Path.Combine(Directory.GetCurrentDirectory(), logDirectory, logFileName);

                            _logger.Log($"Log Minha CDN: {minhaCdnLog} - Log convertido: {agoraLog}");
                            mensagemRetorno = $"Log Minha CDN: {minhaCdnLog} - Log convertido: {agoraLog}. " +
                                $" Log salvado com sucesso no caminho: {logPath}";

                        }
                        else
                        {
                            return BadRequest("Formato de saída inválido, informe o formato de saída válido: " +
                                "0 - Retorno na chamada e salvar no banco ; " +
                                "1 - Retorno do caminho do log salvo em arquivo de texto no servidor");
                        }


                    }
                    return Ok(mensagemRetorno);
                }

                return NotFound("Informe o log pelo corpo da requisição");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("convertFromId")]
        public async Task<ActionResult> ConvertLogAsyncFromId(ConvertLogRequest request)
        {
            if (request == null)
                return BadRequest("Informe o log pelo corpo da requisição");

            int formatoSaida = request.FormatoSaida;
            string requestMinhaCdnLog = request.MinhaCdnLog;
            int id = request.Id;
            string mensagemRetorno = "";

            var log = await _logService.GetLogByIdAsync(id);

            if (log == null)
                return NotFound("Informe o log pelo Id salvo no banco");


            var minhaCdnLog = log.MinhaCdnLog ?? string.Empty;

            try
            {
                if (string.IsNullOrEmpty(requestMinhaCdnLog))
                    requestMinhaCdnLog = log.MinhaCdnLog;

                if (!string.IsNullOrEmpty(requestMinhaCdnLog))
                {
                    string agoraLog = _converter.Convert(requestMinhaCdnLog);

                    if (formatoSaida == (int)FormatoSaida.RetornoChamada)
                    {
                        var logAgora = MinhaCdnLog.ConverterCdnAgora(requestMinhaCdnLog);
                        var logDto = _mapper.Map<CreateLogDto>(logAgora);
                        logDto.MinhaCdnLog = requestMinhaCdnLog;
                        logDto.AgoraLog = agoraLog;
                        var createdTask = await _logService.UpdateLogAsync(id, logDto);
                        mensagemRetorno = $"Log atualizado no banco de dados: {agoraLog}";

                    }
                    else if (formatoSaida == (int)FormatoSaida.SalvarServidorComCaminho)
                    {
                        string logDirectory = _configuration.GetSection("FileLogging:LogDirectory").Value;
                        string logFileName = _configuration.GetSection("FileLogging:LogFileName").Value;
                        string logPath = Path.Combine(Directory.GetCurrentDirectory(), logDirectory, logFileName);

                        _logger.Log($"Log Minha CDN: {minhaCdnLog} - Log convertido: {agoraLog}");
                        mensagemRetorno = $"Log Minha CDN: {minhaCdnLog} - Log convertido: {agoraLog}. " +
                            $" Log salvado com sucesso no caminho: {logPath}";
                    }
                    else
                    {
                        return BadRequest("Formato de saída inválido, informe o formato de saída válido:" +
                            "Id - Id do log salvo no banco de dados " +
                            "0 - Retorno na chamada " +
                            "1 - Retorno do caminho do log salvo em arquivo de texto no servidor");
                    }

                    return Ok(mensagemRetorno);
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
            string mensagemRetorno = "";
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
                        logDto.AgoraLog = agoraLog;
                        var createdTask = await _logService.CreateLogAsync(logDto);
                        mensagemRetorno = $"Salvo no banco com sucesso log: {agoraLog}";
                        return Ok(mensagemRetorno);
                    }
                }
                mensagemRetorno = "Informe o log pela URL: ?minhaCdnLog='Log Aqui'";

                return NotFound(mensagemRetorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveLogAsync")]
        public async Task<ActionResult<CreateLogDto>> SaveLogAsync(ConvertLogRequest request)
        {
            if (request == null)
                return BadRequest("Informe o log pelo corpo da requisição");

            string minhaCdnLog = request.MinhaCdnLog;

            string mensagemRetorno = "";

            try
            {
                if (string.IsNullOrEmpty(minhaCdnLog))
                {
                    return BadRequest("Informe o log pelo corpo da requisição");
                }

                string agoraLog = _converter.Convert(minhaCdnLog);
                var logAgora = MinhaCdnLog.ConverterCdnAgora(minhaCdnLog);
                var logDto = _mapper.Map<CreateLogDto>(logAgora);
                logDto.MinhaCdnLog = minhaCdnLog;
                logDto.AgoraLog = agoraLog;
                var createdTask = await _logService.CreateLogAsync(logDto);
                mensagemRetorno = $"Salvo no banco com sucesso log: {agoraLog}";

                return Ok(mensagemRetorno);
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

        [HttpGet("GetAllLogsDataBaseAsync")]
        public async Task<ActionResult<CreateLogDto>> GetAllLogsDataBaseAsync()
        {
            var logs = await _logService.GetAllLogsAsync();

            if (logs == null)
            {
                return NotFound();
            }

            var logsDto = _mapper.Map<IEnumerable<CreateLogDto>>(logs);
            return Ok(logsDto);
        }

        [HttpGet("GetAllLogsFile")]
        public ActionResult<CreateLogDto> GetAllLogsFile()
        {

            string logDirectory = _configuration.GetSection("FileLogging:LogDirectory").Value;
            string logFileName = _configuration.GetSection("FileLogging:LogFileName").Value;
            string logPath = Path.Combine(Directory.GetCurrentDirectory(), logDirectory, logFileName);

            var logsFile = _logger.ReadLogFromFile(logPath);

            if (logsFile == null)
            {
                return NotFound();
            }

            return Ok(logsFile);
        }
    }
}
