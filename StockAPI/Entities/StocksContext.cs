using Microsoft.EntityFrameworkCore;

namespace StockAPI.Entities
{
    public class StocksContext: DbContext, IDockerStocksContext
    {
        public StocksContext(DbContextOptions<StocksContext> options):base(options) { }

        public DbSet<StockValues> StockValues { get; set; }
    }
}
