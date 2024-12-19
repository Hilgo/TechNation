using NUnit.Framework;
using System;
using TechNationAPI.Converter;

namespace TechNationTest
{
    public class TechNationTests
    {
        private readonly ILogConverter _converter;

        public TechNationTests()
        {
            _converter = new MinhaCdnToAgoraConverter();
        }

        //Teste para log com HIT
        [Test]
        public void Convert_ValidLog_HIT_ReturnsAgoraFormat()
        {
            // Arrange
            string minhaCdnLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
            string expectedAgoraLog = "MINHA CDN GET 200 /robots.txt 100 312 HIT";

            // Act
            string actualAgoraLog = _converter.Convert(minhaCdnLog);

            // Assert
            Assert.That(actualAgoraLog, Is.EqualTo(expectedAgoraLog));
        }

        //Teste para log com MISS
        [Test]
        public void Convert_ValidLog_MISS_ReturnsAgoraFormat()
        {
            // Arrange
            string minhaCdnLog = "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4";
            string expectedAgoraLog = "MINHA CDN POST 200 /myImages 319 101 MISS";

            // Act
            string actualAgoraLog = _converter.Convert(minhaCdnLog);

            // Assert
            Assert.That(actualAgoraLog, Is.EqualTo(expectedAgoraLog));
        }

        //Teste para log com 404
        [Test]
        public void Convert_ValidLog_Error400_ReturnsAgoraFormat()
        {
            // Arrange
            string minhaCdnLog = "199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9";
            string expectedAgoraLog = "MINHA CDN GET 404 /not-found 143 199 MISS";

            // Act
            string actualAgoraLog = _converter.Convert(minhaCdnLog);

            // Assert
            Assert.That(actualAgoraLog, Is.EqualTo(expectedAgoraLog));
        }

        //Teste para log com invalidate
        [Test]
        public void Convert_ValidLog_Invalidate_ReturnsAgoraFormat()
        {
            // Arrange
            string minhaCdnLog = "312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1";
            string expectedAgoraLog = "MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT";

            // Act
            string actualAgoraLog = _converter.Convert(minhaCdnLog);

            // Assert
            Assert.That(actualAgoraLog, Is.EqualTo(expectedAgoraLog));
        }

        //Teste para formato Inválido
        [Test]
        public void Convert_InvalidMethod_ThrowsFormatException()
        {
            // Arrange
            string invalidLog = "312|200|HIT|\"INVALID /robots.txt HTTP/1.1\"|100.2";

            // Act & Assert
            Assert.Throws<FormatException>(() => _converter.Convert(invalidLog));
        }

        //Teste para StatusCode Inválido
        [Test]

        public void Convert_InvalidStatusCode_ThrowsFormatException()
        {
            // Arrange
            string invalidLog = "312|INVALID|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act & Assert
            Assert.Throws<FormatException>(() => _converter.Convert(invalidLog));
        }

        //Teste para ResponseSize inválido
        [Test]
        public void Convert_InvalidResponseSize_ThrowsFormatException()

        {
            // Arrange

            string invalidLog = "INVALID|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act & Assert

            Assert.Throws<FormatException>(() => _converter.Convert(invalidLog));
        }

        // Teste para TimeTaken inválido
        [Test]
        public void Convert_InvalidTimeTaken_ThrowsFormatException()
        { 
            // Arrange
            string invalidLog = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|INVALID";

            // Act & Assert
            Assert.Throws<FormatException>(() => _converter.Convert(invalidLog));
        }
    }
}