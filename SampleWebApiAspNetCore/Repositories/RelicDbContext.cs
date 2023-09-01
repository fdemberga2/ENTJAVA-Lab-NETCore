using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class RelicDbContext : DbContext
    {
        public RelicDbContext(DbContextOptions<RelicDbContext> options)
            : base(options)
        {
        }

        public DbSet<RelicEntity> RelicItems { get; set; } = null!;
    }
}
