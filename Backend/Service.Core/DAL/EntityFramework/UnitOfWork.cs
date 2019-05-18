using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Core.DAL.EntityFramework
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly T _dbContext;
        private readonly IDbContextTransaction _transaction;

        public UnitOfWork(T dbContext, string user)
        {
            _dbContext = dbContext;
            User = user;
            _transaction = _dbContext.Database.BeginTransaction();
            SetExecuteUser();
        }

        public UnitOfWork(T dbContext, string user, Action run)
            : this(dbContext, user)
        {
            Execute(run);
        }

        private string User { get; }

        public void Execute(Action run)
        {
            try
            {
                run.Invoke();
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public async Task Execute(Func<Task> run)
        {
            try
            {
                await run();
                Commit();
            }
            catch (Exception ex)
            {
                Rollback();
                throw;
            }
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        private void SetExecuteUser()
        {
            // _dbContext.Database.ExecuteSqlCommand($"EXEC sp_set_session_context 'user_name', {User}");
        }
    }
}
