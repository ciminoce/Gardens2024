using Microsoft.EntityFrameworkCore.Storage;

namespace Gardens2024.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Gardens2024DbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(Gardens2024DbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
                _transaction?.Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
