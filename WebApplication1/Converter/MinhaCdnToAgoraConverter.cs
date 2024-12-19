using System;
using TechNationAPI.Dtos;

namespace TechNationAPI.Converter
{
    public class MinhaCdnToAgoraConverter:ILogConverter
    {
        private const string Provider = "MINHA CDN";

        public string Convert(string log)
        {
            try
            {
                var minhaCdnLog = MinhaCdnLog.ConverterCdnAgora(log);
                return ConvertInternal(minhaCdnLog);
            }
            catch (FormatException)
            {
                throw new FormatException($"Formato de log inválido: {log}. Formato MinhaCDN : response-size|status-code|cache-status|http-method uri-path http-protocol|time-taken. Exemplo: 312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2"); 
            }
        }

        private string ConvertInternal(MinhaCdnLog minhaCdnLog) => $"{Provider} {minhaCdnLog.HttpMethod}" +
            $" {minhaCdnLog.StatusCode}" +
            $" {minhaCdnLog.UriPath}" +
            $" {Math.Round(minhaCdnLog.TimeTaken,0)}" +
            $" {minhaCdnLog.ResponseSize}" +
            $" {minhaCdnLog.CacheStatus}";

    }
}
