using Microsoft.EntityFrameworkCore;

namespace StockAPI.Entities
{
    public class masterContext : DbContext
    {
        public masterContext(DbContextOptions<masterContext> options):base(options) { }

        public DbSet<StockValues> StockValues { get; set; }
    }
}
