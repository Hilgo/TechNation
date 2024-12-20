using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechNationAPI.Converter;
using TechNationAPI.Dtos;
using TechNationAPI.Models;
using TechNationAPI.Services;
using WebApplication1.Controllers;

namespace TechNationTest
{
    [TestFixture]
    public class LogConverterControllerTests
    {
        private Mock<ILogConverter> _mockConverter;
        private Mock<ILogService> _mockLogService;
        private Mock<IMapper> _mockMapper;
        private Mock<IFileLogger> _mockLogger;
        private Mock<Microsoft.Extensions.Configuration.IConfiguration> _mockConfiguration;
        private TechNationController _controller;

        [SetUp]
        public void Setup()
        {
            _mockConverter = new Mock<ILogConverter>();
            _mockLogService = new Mock<ILogService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<IFileLogger>();
            _mockConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            _mockConfiguration.Setup(c => c.GetSection("FileLogging:LogDirectory").Value).Returns("Logs");
            _mockConfiguration.Setup(c => c.GetSection("FileLogging:LogFileName").Value).Returns("log.txt");

            _controller = new TechNationController(_mockConverter.Object, _mockLogService.Object, _mockMapper.Object, _mockLogger.Object, _mockConfiguration.Object);
        }

        [Test]
        public async Task ConvertLog_ValidLog_ReturnsOkResult ()
        {
            // Arrange
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Returns("Converted Log");
            var mockCreateLogDto = new CreateLogDto();
            _mockMapper.Setup(m => m.Map<CreateLogDto>(It.IsAny<MinhaCdnLog>())).Returns(mockCreateLogDto);

            ConvertLogRequest request = new ConvertLogRequest
            {
                MinhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2",
                FormatoSaida = 0
            };

            // Act
            var result = await _controller.ConvertLogAsync(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Salvo no banco com sucesso log: Converted Log"));
        }

        [Test]
        public async Task ConvertLog_InvalidLog_Chamada_ReturnsBadRequest()
        {
            // Arrange
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Throws<FormatException>();

            ConvertLogRequest request = new ConvertLogRequest
            {
                MinhaCdnLog = "Invalid log",
                FormatoSaida = 0
            };

            // Act
            var result = await _controller.ConvertLogAsync(request) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("One of the identified items was in an invalid format."));

        }

        [Test]
        public async Task ConvertLogFromId_ValidId_ReturnsOkResult()
        {
            // Arrange
            var log = new Log { Id = 1, MinhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2" };
            _mockLogService.Setup(s => s.GetLogByIdAsync(It.IsAny<int>())).ReturnsAsync(log);
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Returns("Converted Log");
            var mockCreateLogDto = new CreateLogDto();
            _mockMapper.Setup(m => m.Map<CreateLogDto>(It.IsAny<MinhaCdnLog>())).Returns(mockCreateLogDto);

            ConvertLogRequest request = new ConvertLogRequest
            {
                Id = 1,
                FormatoSaida = 0
            };

            // Act
            var result = await _controller.ConvertLogAsyncFromId(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Salvo no banco com sucesso log: Converted Log"));
        }

        [Test]
        public async Task ConvertLogFromId_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockLogService.Setup(s => s.GetLogByIdAsync(It.IsAny<int>())).ReturnsAsync((Log)null);

            ConvertLogRequest request = new ConvertLogRequest
            {
                Id = 1,
                FormatoSaida = 0
            };

            // Act
            var result = await _controller.ConvertLogAsyncFromId(request) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task ConvertLogFromQueryString_ValidLog_ReturnsOkResult()
        {
            // Arrange
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Returns("Converted Log");
            var mockCreateLogDto = new CreateLogDto();
            _mockMapper.Setup(m => m.Map<CreateLogDto>(It.IsAny<MinhaCdnLog>())).Returns(mockCreateLogDto);

            string minhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2";

            // Act
            var result = await _controller.ConvertLogFromQueryStringAsync(minhaCdnLog) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Salvo no banco com sucesso log: Converted Log"));
        }

        [Test]
        public async Task ConvertLogFromQueryString_InvalidLog_ReturnsBadRequest()
        {
            // Arrange
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Throws<FormatException>();

            string minhaCdnLog = "Invalid log";

            // Act
            var result = await _controller.ConvertLogFromQueryStringAsync(minhaCdnLog) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task SaveLogAsync_ValidLog_ReturnsOkResult()
        {
            // Arrange
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Returns("Converted Log");
            var mockCreateLogDto = new CreateLogDto();
            _mockMapper.Setup(m => m.Map<CreateLogDto>(It.IsAny<MinhaCdnLog>())).Returns(mockCreateLogDto);

            ConvertLogRequest request = new ConvertLogRequest
            {
                MinhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2"
            };

            // Act
            var result = await _controller.SaveLogAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Salvo no banco com sucesso log: Converted Log"));
        }

        [Test]
        public async Task SaveLogAsync_InvalidLog_ReturnsBadRequest()
        {
            // Arrange
            _mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Throws<FormatException>();

            ConvertLogRequest request = new ConvertLogRequest
            {
                MinhaCdnLog = "Invalid log"
            };

            // Act
            var result = await _controller.SaveLogAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetLogAsync_ValidId_ReturnsOkResult()
        {
            // Arrange
            var log = new Log { Id = 1, MinhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2" };
            _mockLogService.Setup(s => s.GetLogByIdAsync(It.IsAny<int>())).ReturnsAsync(log);
            var mockCreateLogDto = new CreateLogDto();
            _mockMapper.Setup(m => m.Map<CreateLogDto>(It.IsAny<Log>())).Returns(mockCreateLogDto);

            // Act
            var result = await _controller.GetLogAsync(1) as ActionResult<CreateLogDto>;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(mockCreateLogDto));
        }

        [Test]
        public async Task GetLogAsync_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockLogService.Setup(s => s.GetLogByIdAsync(It.IsAny<int>())).ReturnsAsync((Log)null);

            // Act
            var result = await _controller.GetLogAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
            var notFoundResult = result.Result as NotFoundResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task GetAllLogsDataBaseAsync_ReturnsOkResult()
        {
            // Arrange
            var logs = new List<Log> { new Log { Id = 1, MinhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2" } };
            _mockLogService.Setup(s => s.GetAllLogsAsync()).ReturnsAsync(logs);
            var mockCreateLogDtoList = new List<CreateLogDto> { new CreateLogDto() };
            _mockMapper.Setup(m => m.Map<IEnumerable<CreateLogDto>>(It.IsAny<IEnumerable<Log>>())).Returns(mockCreateLogDtoList);

            // Act
            var result = await _controller.GetAllLogsDataBaseAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(mockCreateLogDtoList));
        }

        [Test]
        public void GetAllLogsFile_ReturnsOkResult()
        {
            // Arrange
            var logsFile = new List<string> { "Log1", "Log2" };
            _mockLogger.Setup(l => l.ReadLogFromFile(It.IsAny<string>())).Returns(logsFile);

            // Act
            var result = _controller.GetAllLogsFile().Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(logsFile));
        }

        [Test]
        public void GetAllLogsFile_ReturnsNotFound()
        {
            // Arrange
            _mockLogger.Setup(l => l.ReadLogFromFile(It.IsAny<string>())).Returns((IEnumerable<string>)null);

            // Act
            var result = _controller.GetAllLogsFile().Result as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }
    }
}
