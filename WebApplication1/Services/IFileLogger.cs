using System.Collections.Generic;

namespace TechNationAPI.Services
{
    public interface IFileLogger
    {
        void Log(string message);
        IEnumerable<string> ReadLogFromFile(string filePath);
    }
}
