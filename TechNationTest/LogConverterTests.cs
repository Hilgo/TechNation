using NUnit.Framework;
using System;
using TechNationAPI.Converter;

namespace TechNationTest
{
    [TestFixture]
    public class TechNationTests
    {
        private ILogConverter _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new MinhaCdnToAgoraConverter();
        }

        [TestCase("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2", "MINHA CDN GET 200 /robots.txt 100 312 HIT")]
        [TestCase("101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4", "MINHA CDN POST 200 /myImages 319 101 MISS")]
        [TestCase("199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9", "MINHA CDN GET 404 /not-found 143 199 MISS")]
        [TestCase("312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1", "MINHA CDN GET 200 /robots.txt 245 312 REFRESH_HIT")]
        public void Convert_ValidLog_ReturnsAgoraFormat(string minhaCdnLog, string expectedAgoraLog)
        {
            // Act
            string actualAgoraLog = _converter.Convert(minhaCdnLog);

            // Assert
            Assert.That(actualAgoraLog, Is.EqualTo(expectedAgoraLog));
        }

        [TestCase("312|200|HIT|\"INVALID /robots.txt HTTP/1.1\"|100.2")]
        [TestCase("312|INVALID|HIT|\"GET /robots.txt HTTP/1.1\"|100.2")]
        [TestCase("INVALID|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2")]
        [TestCase("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|INVALID")]
        public void Convert_InvalidLog_ThrowsFormatException(string invalidLog)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => _converter.Convert(invalidLog));
        }
    }
}
