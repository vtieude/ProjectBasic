using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Core.DAL.EntityFramework
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        protected readonly DbContext DbContext;
        protected readonly IUserContext UserContext;
        private bool _disposed;
        protected ReadOnlyRepository(DbContext dbContext, IUserContext userContext)
        {
            DbContext = dbContext;
            UserContext = userContext;
        }

        public string GetUserId()
        {
            return UserContext.CurrentUser.UserId;
        }

        public string GetUserName()
        {
            return UserContext.CurrentUser.UserName;
        }

        public IEnumerable<T> FindAll()
        {
            return DbContext.Set<T>().AsNoTracking().ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> filter)
        {
            return DbContext.Set<T>().Where(filter).ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> filter)
        {
            return await DbContext.Set<T>().Where(filter).ToListAsync();
        }

        public IQueryable<T> FindAsQueryable()
        {
            return DbContext.Set<T>().AsQueryable();
        }

        public T FindOne(Expression<Func<T, bool>> filter)
        {
            return DbContext.Set<T>().Where(filter).SingleOrDefault();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter)
        {
            return await DbContext.Set<T>().Where(filter).SingleOrDefaultAsync();
        }

        public T FindFirst(Expression<Func<T, bool>> filter)
        {
            return DbContext.Set<T>().Where(filter).FirstOrDefault();
        }

        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> filter)
        {
            return await DbContext.Set<T>().Where(filter).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                DbContext.Dispose();
            }

            _disposed = true;
        }

        ~ReadOnlyRepository()
        {
            Dispose(false);
        }
    }

    public sealed class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class
    {
        private readonly bool _isAuditable;
        private bool _disposed;
        private readonly IUserContext _userContext;

        public Repository(DbContext dbContext, IUserContext userContext)
            : base(dbContext, userContext)
        {
            var type = typeof(T);
            _userContext = userContext;
            _isAuditable = type.GetProperty("SysChgDate") != null &&
                           type.GetProperty("SysChgLogin") != null &&
                           type.GetProperty("SysChgType") != null &&
                           type.GetProperty("SysChgCnt") != null;
        }


        public void Insert(T entity)
        {
            if (_isAuditable)
            {
                // https://github.com/aspnet/EntityFrameworkCore/issues/12064
                // Due to above issue, we have to handle at application level
                ((dynamic)entity).SysChgLogin = UserContext.CurrentUser.UserName;
                ((dynamic)entity).SysChgDate = DateTimeOffset.Now;
                ((dynamic)entity).SysChgType = "I";
                ((dynamic)entity).SysChgCnt = 1;
            }

            DbContext.Set<T>().Add(entity);
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            if (_isAuditable)
            {
                //https://github.com/aspnet/EntityFrameworkCore/issues/12064
                // due to above bug, we have to handle at application level
                foreach (var entity in entities)
                {
                    ((dynamic)entity).SysChgLogin = UserContext.CurrentUser.UserName;
                    ((dynamic)entity).SysChgDate = DateTimeOffset.Now;
                    ((dynamic)entity).SysChgType = "I";
                    ((dynamic)entity).SysChgCnt = 1;
                }
            }

            DbContext.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            if (_isAuditable)
            {
                // We use trigger to handle
                ((dynamic)entity).SysChgLogin = UserContext.CurrentUser.UserName;
                ((dynamic)entity).SysChgDate = DateTimeOffset.Now;
                ((dynamic)entity).SysChgType = "U";
                ((dynamic)entity).SysChgCnt = ((dynamic)entity).SysChgCnt + 1;
            }

            DbContext.Set<T>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Set<T>().Attach(entity);
            }

            DbContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (DbContext.Entry(entity).State == EntityState.Detached)
                {
                    DbContext.Set<T>().Attach(entity);
                }
            }

            DbContext.Set<T>().RemoveRange(entities);
        }

        public async Task SaveAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                Dispose(false);
            }

            if (disposing)
            {
                // Nothing in derived class to currently dispose
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        public bool IsModified(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Set<T>().Attach(entity);
            }

            return DbContext.Entry(entity).State == EntityState.Modified;
        }
    }
}
