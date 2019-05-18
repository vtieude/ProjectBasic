using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Core.DAL
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class
    {
        string GetUserId();
        string GetUserName();
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entities);
        void Save();
        Task SaveAsync();
        void Update(T entity);
        bool IsModified(T entity);
    }
}
