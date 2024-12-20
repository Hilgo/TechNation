using NUnit.Framework;
using System;
using TechNationAPI.Converter;

namespace TechNationTest
{
    [TestFixture]
    public class MinhaCdnLogTests
    {
        [TestCase("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2", 312, 200, "HIT", "/robots.txt", "GET", 100.2)]
        [TestCase("101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4", 101, 200, "MISS", "/myImages", "POST", 319.4)]
        [TestCase("199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9", 199, 404, "MISS", "/not-found", "GET", 142.9)]
        [TestCase("312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1", 312, 200, "REFRESH_HIT", "/robots.txt", "GET", 245.1)]
        public void ConverterCdnAgora_ValidLog_ReturnsExpectedMinhaCdnLog(string log, int expectedResponseSize, int expectedStatusCode, string expectedCacheStatus, string expectedUriPath, string expectedHttpMethod, double expectedTimeTaken)
        {
            // Act
            var result = MinhaCdnLog.ConverterCdnAgora(log);

            // Assert
            Assert.That(result.ResponseSize, Is.EqualTo(expectedResponseSize));
            Assert.That(result.StatusCode, Is.EqualTo(expectedStatusCode));
            Assert.That(result.CacheStatus, Is.EqualTo(expectedCacheStatus));
            Assert.That(result.UriPath, Is.EqualTo(expectedUriPath));
            Assert.That(result.HttpMethod, Is.EqualTo(expectedHttpMethod));
            Assert.That(result.TimeTaken, Is.EqualTo(expectedTimeTaken));
            Assert.That(result.Version, Is.EqualTo("1.0"));
            Assert.That(result.Date, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
        }

        [TestCase("312|200|HIT|\"INVALID /robots.txt HTTP/1.1\"|100.2")]
        [TestCase("312|INVALID|HIT|\"GET /robots.txt HTTP/1.1\"|100.2")]
        [TestCase("INVALID|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2")]
        [TestCase("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|INVALID")]
        [TestCase("312|200|HIT|\"GET /robots.txt HTTP/1.1\"")]
        [TestCase("312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2|EXTRA")]
        public void ConverterCdnAgora_InvalidLog_ThrowsFormatException(string invalidLog)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() => MinhaCdnLog.ConverterCdnAgora(invalidLog));
        }
    }
}
