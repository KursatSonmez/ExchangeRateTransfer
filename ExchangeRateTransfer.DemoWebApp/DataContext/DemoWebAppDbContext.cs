using ExchangeRateTransfer.DemoWebApp.TransferWrapper;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Entities;
using ExchangeRateTransfer.DemoWebApp.TransferWrapper.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DemoWebApp.DataContext
{
    public class DemoWebAppDbContext : DbContext, IExchangeRateTransferDbContext
    {
        public DemoWebAppDbContext() : base()
        {
        }

        public DemoWebAppDbContext(DbContextOptions<DemoWebAppDbContext> options) : base(options)
        {
        }

        public static DemoWebAppDbContext Create() => new();
        public static DemoWebAppDbContext Create(DbContextOptions<DemoWebAppDbContext> options)
             => new(options);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseExchangerateTransfer();
        }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
