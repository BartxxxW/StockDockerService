using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StockAPI.Entities
{
    public interface IExtendedInterface: IDockerStocksContext
    {
        DatabaseFacade Database { get; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity:class;
    }
    public interface IDockerStocksContext
    {
        DbSet<StockValues> StockValues { get; set; }
    }

    public class DockerStocksContext : DbContext, IDockerStocksContext
    {
        public DockerStocksContext(DbContextOptions<DockerStocksContext> options) : base(options) { }

        public DbSet<StockValues> StockValues { get; set; }
    }
}
