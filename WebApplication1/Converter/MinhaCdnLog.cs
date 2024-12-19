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

        public static MinhaCdnLog ConverterCdnAgora(string log)
        {
            string[] parts = log.Split('|');
            if (parts.Length != 5)
            {
                throw new FormatException();
            }

            string uriPath = Regex.Match(parts[3], "\"([^\"]+)\"").Groups[1].Value;
            Match methodMatch = Regex.Match(uriPath, @"^(GET|POST|PUT|DELETE|PATCH|OPTIONS|HEAD)");
            if (!methodMatch.Success) throw new FormatException();
            string httpMethod = methodMatch.Value;


            return new MinhaCdnLog
            {

                ResponseSize = int.Parse(parts[0]),
                StatusCode = int.Parse(parts[1]),
                CacheStatus = parts[2] == "INVALIDATE" ? "REFRESH_HIT" : parts[2],
                UriPath = uriPath.Substring(httpMethod.Length + 1).Split(' ')[0],
                HttpMethod = httpMethod,
                TimeTaken = double.Parse(parts[4], CultureInfo.InvariantCulture),
                Date = DateTime.Now,
                Version = _version

            };
        }
    }
}
