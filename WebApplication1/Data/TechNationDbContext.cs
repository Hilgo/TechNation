using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using TechNationAPI.Data;
using TechNationAPI.Models;

namespace UsuariosApi.Data
{
    internal class TechNationDbContext : DbContext, ITechNationDbContext
    {
        public TechNationDbContext
           (DbContextOptions<TechNationDbContext> opts) : base(opts) { }

        public TechNationDbContext() : base() { }

        public virtual DbSet<Log> LogsTechNation { get; set; }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
