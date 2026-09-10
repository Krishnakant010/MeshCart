using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Ordering.Infrastructure.Data
{
    public class OrderContextFactory
        : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString =
                configuration.GetConnectionString("OrderingConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();

            optionsBuilder.UseSqlServer(
                connectionString,
                sql =>
                {
                    sql.MigrationsAssembly("Ordering.Infrastructure");
                });

            return new OrderContext(optionsBuilder.Options);
        }
    }

}