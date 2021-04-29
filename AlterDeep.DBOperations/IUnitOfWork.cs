using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlterDeep.DBOperations
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
       Task<int> CommitAsync();
    }

    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DeepContext _dbContext;

        public EFUnitOfWork(DeepContext dbContext)
        {

            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null.");

            _dbContext = dbContext ?? new DeepContext(new DbContextOptions<DeepContext>());
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new EFRepository<T>(_dbContext);
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                // Burada DbEntityValidationException hatalarını handle edebiliriz.
                throw;
            }
        }
        
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}