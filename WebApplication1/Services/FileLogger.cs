using System;
using System.Collections.Generic;
using System.IO;

namespace TechNationAPI.Services
{
    public class FileLogger : IFileLogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            // Crie o diretório de logs se ele não existir
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
        }

        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"[{DateTime.Now}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gravar no log: {ex.Message}");
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
