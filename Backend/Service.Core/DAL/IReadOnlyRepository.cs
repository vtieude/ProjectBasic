using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Core.DAL
{
    public interface IReadOnlyRepository<T> : IDisposable
    {
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> filter);
        IQueryable<T> FindAsQueryable();
        T FindOne(Expression<Func<T, bool>> filter);
        Task<T> FindOneAsync(Expression<Func<T, bool>> filter);
        T FindFirst(Expression<Func<T, bool>> filter);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> filter);
    }
}
