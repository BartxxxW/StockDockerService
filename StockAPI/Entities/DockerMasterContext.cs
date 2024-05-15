using Microsoft.EntityFrameworkCore;

namespace StockAPI.Entities
{
    public class DockerMasterContext : DbContext
    {
        public DockerMasterContext(DbContextOptions<DockerMasterContext> options):base(options) { }

        public DbSet<StockValues> StockValues { get; set; }
    }
}
