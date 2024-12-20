using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TechNationAPI.Services
{
    public class FileLogger : IFileLogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
        }

        public async Task LogAsync(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    await writer.WriteLineAsync($"[{DateTime.Now}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Ao escrever log: {ex.Message}");
            }
        }

        public IEnumerable<string> ReadLogFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
