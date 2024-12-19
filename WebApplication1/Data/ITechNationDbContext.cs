using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using TechNationAPI.Models;

namespace TechNationAPI.Data
{
    public interface ITechNationDbContext
    {
        DbSet<Log> LogsTechNation { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
