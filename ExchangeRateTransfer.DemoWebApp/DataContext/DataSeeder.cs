using Microsoft.EntityFrameworkCore;

namespace ExchangeRateTransfer.DemoWebApp.DataContext
{
    public class DataSeeder : IDisposable
    {
        private readonly DemoWebAppDbContext _context;
        public DataSeeder(DemoWebAppDbContext context)
        {
            _context = context;

        }

        public void Seed()
        {
            if (_context.Database.IsInMemory())
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }
            else
                _context.Database.Migrate();
        }

        #region IDisposable

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // dispose managed resources...
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataSeeder() => Dispose(false);

        #endregion
    }
}
