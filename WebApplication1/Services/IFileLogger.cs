using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechNationAPI.Services
{
    public interface IFileLogger
    {
        Task LogAsync(string message);
        IEnumerable<string> ReadLogFromFile(string filePath);
    }
}
