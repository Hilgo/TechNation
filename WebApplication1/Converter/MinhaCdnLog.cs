using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace TechNationAPI.Converter
{
    public class MinhaCdnLog
    {
        public int ResponseSize { get; set; }
        public int StatusCode { get; set; }
        public string CacheStatus { get; set; }
        public string UriPath { get; set; }
        public string HttpMethod { get; set; }
        public double TimeTaken { get; set; }
        public DateTime Date { get; set; }
        public string Version { get; set; }
        private const string _version = "1.0";

        private static readonly Regex UriPathRegex = new Regex("\"([^\"]+)\"", RegexOptions.Compiled);
        private static readonly Regex HttpMethodRegex = new Regex(@"^(GET|POST|PUT|DELETE|PATCH|OPTIONS|HEAD)", RegexOptions.Compiled);

        public static MinhaCdnLog ConverterCdnAgora(string log)
        {
            string[] parts = log.Split('|');
            if (parts.Length != 5)
            {
                throw new FormatException("Formato do log incorreto. Experado 5 partes separados por '|'.");
            }

            if (!int.TryParse(parts[0], out int responseSize))
            {
                throw new FormatException("ResponseSize em formato inválido.");
            }

            if (!int.TryParse(parts[1], out int statusCode))
            {
                throw new FormatException("StatusCode em formato inválido.");
            }

            string cacheStatus = parts[2] == "INVALIDATE" ? "REFRESH_HIT" : parts[2];

            string uriPath = UriPathRegex.Match(parts[3]).Groups[1].Value;
            Match methodMatch = HttpMethodRegex.Match(uriPath);
            if (!methodMatch.Success)
            {
                throw new FormatException("método HTTP inválido em UriPath.");
            }
            string httpMethod = methodMatch.Value;

            string cleanedUriPath = uriPath.Substring(httpMethod.Length + 1).Split(' ')[0];

            if (!double.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out double timeTaken))
            {
                throw new FormatException("TimeTaken em formato inválido.");
            }

            return new MinhaCdnLog
            {
                ResponseSize = responseSize,
                StatusCode = statusCode,
                CacheStatus = cacheStatus,
                UriPath = cleanedUriPath,
                HttpMethod = httpMethod,
                TimeTaken = timeTaken,
                Date = DateTime.Now,
                Version = _version
            };
        }
    }
}
