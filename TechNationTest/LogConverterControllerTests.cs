using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TechNationAPI.Converter;
using TechNationAPI.Dtos;
using TechNationAPI.Services;
using WebApplication1.Controllers;

namespace TechNationTest
{
    [TestFixture]
    public class LogConverterControllerTests
    {
        [Test]
        public async Task ConvertLog_ValidLog_Chamada_ReturnsOkResult()
        {
            // Arrange
            var mockConverter = new Mock<ILogConverter>();
            mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Returns("Converted Log");
            var mockLogService = new Mock<ILogService>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<IFileLogger>();
            var mockConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();


            var mockCreateLogDto = new CreateLogDto();
            mockMapper.Setup(m => m.Map<CreateLogDto>(It.IsAny<MinhaCdnLog>()))
                      .Returns(mockCreateLogDto);

            var controller = new TechNationController(mockConverter.Object, mockLogService.Object, mockMapper.Object, mockLogger.Object, mockConfiguration.Object);
            ConvertLogRequest request = new ConvertLogRequest
            {
                MinhaCdnLog = "312|200|HIT|\\\"GET \\/robots.txt HTTP\\/1.1\\\"|100.2",
                FormatoSaida = 0
            };


            // Act
            var result = await controller.ConvertLogAsync(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Salvo no banco com sucesso log: Converted Log"));
        }

        [Test]
        public async Task ConvertLog_InvalidLog_Chamada_ReturnsBadRequest()
        {
            // Arrange
            var mockConverter = new Mock<ILogConverter>();
            mockConverter.Setup(c => c.Convert(It.IsAny<string>())).Throws<FormatException>();
            var mockLogService = new Mock<ILogService>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<IFileLogger>();
            var mockConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            var controller = new TechNationController(mockConverter.Object, mockLogService.Object, mockMapper.Object, mockLogger.Object, mockConfiguration.Object);
            ConvertLogRequest request = new ConvertLogRequest
            {
                MinhaCdnLog = "Invalid log",
                FormatoSaida = 0
            };

            // Act
            var result = await controller.ConvertLogAsync(request) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }
    }
}
