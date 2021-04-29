using System;
using System.Linq;
using System.Linq.Expressions;

namespace AlterDeep.DBOperations
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate); 
        T GetById(int id);
        T Get(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}