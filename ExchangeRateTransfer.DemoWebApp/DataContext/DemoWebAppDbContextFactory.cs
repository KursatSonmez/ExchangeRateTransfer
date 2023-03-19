using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExchangeRateTransfer.DemoWebApp.DataContext
{
    public class DemoWebAppDbContextFactory : IDesignTimeDbContextFactory<DemoWebAppDbContext>
    {
        public DemoWebAppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DemoWebAppDbContext>();

            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            bool isDevelopment = environmentName == "Development";

            Console.WriteLine($"DemoWebAppDbContextFactory - Environment: {environmentName}, IsDevelopment: {isDevelopment}");

            string connStr = configuration.GetConnectionString("DbContext");

            optionsBuilder.UseSqlServer(connStr, b => b.MigrationsAssembly("ExchangeRateTransfer.DemoWebApp"));

            return DemoWebAppDbContext.Create(optionsBuilder.Options);
        }
    }
}
