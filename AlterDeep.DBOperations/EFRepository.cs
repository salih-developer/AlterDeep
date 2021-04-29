using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AlterDeep.DBOperations
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public EFRepository(DeepContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null.");

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        #region IRepository Members
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {

            return _dbSet.Where(predicate);

        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).SingleOrDefault();
        }

        public T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var q = _dbSet.Where(predicate);
            foreach (var include in includes)
            {
                q= q.Include(include);
            }

            return q.SingleOrDefault();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(T entity)
        {
            // Eğer sizlerde genelde bir kayıtı silmek yerine IsDelete şeklinde bool bir flag alanı tutuyorsanız,
            // Küçük bir refleciton kodu yardımı ile bunuda otomatikleştirebiliriz.
            if (entity.GetType().GetProperty("IsDelete") != null)
            {
                T _entity = entity;

                _entity.GetType().GetProperty("IsDelete").SetValue(_entity, true);

                this.Update(_entity);
            }
            else
            {
                // Önce entity'nin state'ini kontrol etmeliyiz.
                var dbEntityEntry = _dbContext.Entry(entity);

                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                if (entity.GetType().GetProperty("IsDelete") != null)
                {
                    T _entity = entity;
                    _entity.GetType().GetProperty("IsDelete").SetValue(_entity, true);

                    this.Update(_entity);
                }
                else
                {
                    Delete(entity);
                }
            }
        }
        #endregion
    }
}